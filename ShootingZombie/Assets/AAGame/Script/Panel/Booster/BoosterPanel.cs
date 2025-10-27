using System.Collections;
using System.Collections.Generic;
using Thanh.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BoosterPanel : Popup
{
    public Transform extraParent;
    public Transform parentBooster;
    public Button nothankBtn;
    [SerializeField] private Button btnResetBooster;
    [SerializeField] private Button btnCollectAll;


    private void OnEnable()
    {
        if (nothankBtn != null)
        {
            nothankBtn.onClick.RemoveListener(Close);
            nothankBtn.onClick.AddListener(Close);
        }
        if (this.popupID == PopupID.PopupBooster)
        {
            GameEvent.RessetBooster.RemoveAllListeners();
            GameEvent.RessetBooster.AddListener(() =>
            {
                Close();
                OnShow();
            });



            btnCollectAll.onClick.RemoveListener(ClickCollectAll);
            btnCollectAll.onClick.AddListener(ClickCollectAll);



            btnResetBooster.onClick.RemoveListener(ClickReset);
            btnResetBooster.onClick.AddListener(ClickReset);

            if(User.Instance[ItemID.TutPlay] == 0)
            {
                //btnCollectAll.gameObject.SetActive(false);
                btnResetBooster.gameObject.SetActive(false);
            }
            else
            {
                //btnCollectAll.gameObject.SetActive(true);
                btnResetBooster.gameObject.SetActive(true);
            }
        }


    }
    public void ClickReset()
    {
        GameEvent.RessetBooster.Invoke();
    }

    public void ClickCollectAll()
    {
        GameEvent.CollectAll.Invoke();
    }

    public override void OnShow()
    {
        base.OnShow();
        InitBooster();
        //block pointer
        GameManager.Instance.isBlockPointer = true;
        //checking auto select booster
        if (GameManager.Instance.isAutoPlay)
        {
            StartCoroutine(AutoSelectBooster());
        }
        GameManager.Instance.isSelectBooster = false;
    }

    public override void Close()
    {
        foreach (Transform obj in parentBooster)
        {
            //obj.gameObject.SetActive(false);
            Destroy(obj.gameObject);
        }
        foreach (Transform obj in extraParent)
        {
            //obj.gameObject.SetActive(false);
            Destroy(obj.gameObject);
        }

       //GameManager.Instance.isSelectVipBooster = true;
        GameManager.Instance.isBlockPointer = false;
        base.Close();
    }

    public void InitBooster()
    {
        int indexRandom;
        
        if (this.popupID == PopupID.PopupBooster)
        {
            List<Booster> listBoosterTemp = new List<Booster>();
            listBoosterTemp.AddRange(BoosterManager.instance.GetListBoosterRandom());
            indexRandom = Random.Range(0, listBoosterTemp.Count);

            for (int i = 0; i < listBoosterTemp.Count; i++)
            {
                GameObject boost = Instantiate(Resources.Load<GameObject>("UI/Booster/ButtonBooster"), parentBooster);
                boost.GetComponent<BoosterButton>().SetUp(listBoosterTemp[i], this.popupID);
                if (i == indexRandom)
                {
                    boost.GetComponent<BoosterButton>().ShowTut();
                }
            }


            //extra
            GameObject boostExtra = Instantiate(Resources.Load<GameObject>("UI/Booster/ButtonBooster"), extraParent);
            boostExtra.GetComponent<BoosterButton>().SetUp(BoosterManager.instance.GetBoosterExtraRandom(), this.popupID,true);
        }
    }

    IEnumerator AutoSelectBooster()
    {
        yield return new WaitForSeconds(1);

        if (GameManager.Instance.isSelectBooster == false)
        {
            BoosterButton[] boosterButtons = parentBooster.GetComponentsInChildren<BoosterButton>();
            boosterButtons[Random.Range(0, boosterButtons.Length)].Select();
        }
    }
}

