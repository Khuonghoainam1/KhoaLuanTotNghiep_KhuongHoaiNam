using MyGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Thanh.Core;
using UnityEngine;
using UnityEngine.UI;
using Yurowm.GameCore;

public class PopupManager : Singleton<PopupManager>
{
    [SerializeField] private PopupID[] preInstantsPopups;
    [SerializeField] private List<Popup> popups;
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasScaler canvasScaler;
    public static Action<PopupID, Popup> OnShowDone;
    public static Action<PopupID> OnCloseDoneEvent;
    public PopupID showingPopup
    {
        get
        {
            List<Popup> openingPopups = GetOpeningPopups();
            if (openingPopups != null && openingPopups.Count > 0)
            {
                return openingPopups[openingPopups.Count - 1].popupID;
            }
            return PopupID.None;
        }
    }

    private bool isInit;


    private void Start()
    {
        OnPreLoadPopup();
    }

    private void OnPreLoadPopup()
    {
        for (int i = 0; i < preInstantsPopups.Length; i++)
        {
            var idPopup = preInstantsPopups[i];
            var popupPrefab = ContentPoolable.GetPrefab<Popup>(x => x.popupID == idPopup);
            var popup = Instantiate(popupPrefab, transform) as Popup;
            popup.transform.SetParent(transform);
            RectTransform popupRect = popup.GetComponent<RectTransform>();
            popupRect.Reset();
            popupRect.offsetMin = Vector2.zero;
            popupRect.offsetMax = Vector2.zero;
            popups.Add(popup);
        }
        isInit = true;
    }

    private void LateUpdate()
    {
        if (!isInit)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Nếu có popup thì đóng lại.
            if (hasPopup())
            {
                CloseCurrentPopup();
                return;
            }
        }
    }

    public void OnSetupCamera(Camera camera)
    {
        canvas.worldCamera = camera;
        canvas.planeDistance = 0.5f;
        canvas.sortingLayerName = "UI";
        canvas.sortingOrder = 20;
        if(camera.aspect >= 2.1f)
        {
            canvasScaler.matchWidthOrHeight = 1;
        }
        else
        {
            canvasScaler.matchWidthOrHeight = 0;
        }

        DisableAllPopup();
    }

    public bool IsPopupActive(PopupID popupID)
    {
        bool result = false;
        for (int i = 0; i < popups.Count; i++)
        {
            if (popups[i].popupID == popupID && popups[i].gameObject.activeInHierarchy == true)
                return true;
        }
        return result;
    }

    public bool hasPopup()
    {
        return GetOpeningPopups().Count > 0;
    }

    public List<Popup> GetOpeningPopups()
    {
        return popups.Where(x => x != null && x.gameObject.activeInHierarchy).ToList();
    }

    public void OpenPopup<T>(PopupID popupID, Action<T> onOpened = null, float delay = 0f) where T : Popup
    {
        if (popupID == showingPopup)
        {
            return;
        }

        StartCoroutine(IEOpenSyncPopup<T>(popupID, onOpened, delay));
    }

    private IEnumerator IEOpenSyncPopup<T>(PopupID popupID, Action<T> onOpened, float delay) where T : Popup
    {
        var isShowLoading = false;

        var popup = GetPopup(popupID);

        if (popup == null)
        {
            var popupPrefab = ContentPoolable.GetPrefab<Popup>(x => x.popupID == popupID);
            popup = Instantiate(popupPrefab, transform) as Popup;
            popup.transform.SetParent(transform);
            RectTransform popupRect = popup.GetComponent<RectTransform>();
            popupRect.Reset();
            popupRect.offsetMin = Vector2.zero;
            popupRect.offsetMax = Vector2.zero;
        }

        if (delay > 0)
        {
            isShowLoading = true;
            OpenPopup<Popup>(PopupID.PopupLoading);
            yield return new WaitForSeconds(delay);
        }
        popups.Add(popup);

        if (popup.gameObject.activeInHierarchy)
        {
            if (isShowLoading)
            {
                popups.Find(x => x.popupID == PopupID.PopupLoading).Close();
                isShowLoading = false;
            }
        }

        if (onOpened != null)
        {
            onOpened(popup.GetComponent<T>());
        }

        popup.onOpen.AddListener(delegate
        {
            if (isShowLoading)
            {
                OnClosePopup<Popup>(PopupID.PopupLoading);
                isShowLoading = false;
            }
        });

        popup.transform.SetAsLastSibling();
        popup.OnShow();

        GameEvent.OnPopupChanged?.Invoke(GetOpeningPopups().Count);

        if (popup.animator != null)
        {
            AnimatorClipInfo[] cl = popup.animator.GetCurrentAnimatorClipInfo(0);
            if (cl.Length > 0)
                yield return WaitForSecondsCache.GetRealTime(cl[0].clip.length);
        }

        popup.OnShowDone();
        OnShowDone?.Invoke(popupID, popup);
    }

    public Popup GetPopup(PopupID popupID)
    {
        var popup = popups.Find(x => x.popupID == popupID);
        return popup;
    }

    public void CloseCurrentPopup()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var pup = transform.GetChild(i).GetComponent<Popup>();
            if (pup == null || !pup.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (pup.isPopupGameWin || pup.isClosing)
            {
                break;
            }
            pup.Close();
            StartCoroutine(pup.IEClose());
            break;
        }
    }

    public void DisableAllPopup()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var pup = transform.GetChild(i).GetComponent<Popup>();
            if (pup == null || !pup.gameObject.activeInHierarchy)
            {
                continue;
            }

            pup.gameObject.SetActive(false);
        }
    }

    public void OnClosePopup<T>(PopupID popupID, Action<T> onOpened = null) where T : Popup
    {
        var popup = popups.Find(x => x.popupID == popupID);
        if (popup.gameObject.activeInHierarchy)
        {
            if (onOpened != null)
            {
                onOpened(popup.GetComponent<T>());
            }
            popup.Close();
        }
    }

    public void OnCloseDone(PopupID popupID)
    {
        OnCloseDoneEvent?.Invoke(popupID);
        GameEvent.OnPopupChanged?.Invoke(GetOpeningPopups().Count);
    }
}

public enum PopupID
{
    PopupLoading,
    PopupSelectMode,
    PopupGameOver,
    PopupPlaying,
    PopupSpin,
    PopupWin,
    PopupSkinShop,
    PopupNotice,
    PopupUnlockSkin,
    PopupClaimItem,
    PopupDailyReward,
    PopupEndGame,
    PopupSetting,
    PopupSelectWeapon,
    PopupLuckySpin,
    PopupBooster,
    PopupBoosterVip,
    PopupWorkShop,
    PopupRevive,
    PopupCarUpgrade,
    PopupGameWin,
    PopupGameDefeact,
    PopupSelectMap,
    PopupSetingGameplay,
    PopupSelectBoss,
    PopupBoosterUnchanged,
    PopupWarning,
    PopupNewUnlockSkin,
    PopupHeroes,
    PopupSpinWheel,
    //
    //Popup panel GameMod
    PopupReviewReward,
    PopupTalent,
    PopupInforTalent,
    PopupCollection,
    PopupStageNormal,
    PopupShop,
    PopupDefeat2,
    PopupTryHero,

    None,
    PopupGDPR = 999999,

}
[System.Serializable]
public class ItemDataUnlock
{
    public ItemID id;
    public int value;
    public string description;
    public ItemDataUnlock(ItemID id, int value, string description)
    {
        this.id = id;
        this.value = value;
        this.description = description;
    }
}