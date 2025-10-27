using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class DailyRewardCellView : MonoBehaviour
{
    [SerializeField]
    private Image bgRoot;
    [SerializeField]
    private Image imgBGLock;
    [SerializeField]
    private Image imgTick;
    [SerializeField]
    private TextMeshProUGUI textDay;
    public List<ItemValueCell> itemValueCells;
    [SerializeField]
    private Button btnCelDay;
    [SerializeField]
    private GameObject imgAds;
    DailyRewardState _state;
    public DailyRewardData data;
    public Animator animator;
    bool isCanClick = false;

    public DailyRewardState State
    {
        get { return _state; }
        set
        {
            _state = value;
            imgTick.gameObject.SetActive(_state == DailyRewardState.Claimed);
            imgBGLock.gameObject.SetActive(_state == DailyRewardState.Achieved);
           
            
            imgAds.SetActive(false);
            bgRoot.sprite = SpriteManager.Instance.GetSprite(ItemID.dayCoinOpen);

            if (_state == DailyRewardState.Achieved)
            {
                //animator.enabled = true;
                imgBGLock.gameObject.SetActive(_state == DailyRewardState.Achieved);
                isCanClick = true;
                StartCoroutine(ClaimeForToday());
              
            }
            else if (_state == DailyRewardState.CountDown)
            {
                //        animator.enabled = true;
               bgRoot.sprite = SpriteManager.Instance.GetSprite(ItemID.dayCoinOpen);
                isCanClick = true;
                imgAds.SetActive(true);
                 
            }
            else if (_state == DailyRewardState.Claimed)
            {
                imgTick.gameObject.SetActive(_state == DailyRewardState.Claimed);
                bgRoot.sprite = SpriteManager.Instance.GetSprite(ItemID.dayCoin);
                btnCelDay.interactable = false;
                //bgRoot.color = new Color32(10, 10, 10, 100);
            }
            else
            {
                //   animator.enabled = false;
               // bgRoot.sprite = SpriteManager.Instance.GetSprite(ItemID.dayCoinOpen);
                isCanClick = false;
            }
         /*   if (_state == DailyRewardState.CountDown)
            {
                //bgRoot.sprite = SpriteManager.Instance.GetSprite(ItemID.dayCoinOpen);

               
            }*/
            if (data.day == 6)
            {
                bgRoot.sprite = SpriteManager.Instance.GetSprite(ItemID.day7);
                if (_state == DailyRewardState.Achieved || _state == DailyRewardState.CountDown)
                {
                    bgRoot.sprite = SpriteManager.Instance.GetSprite(ItemID.day7Open);
                }
                CanClame();
                return;
            }
            /*switch (data.itemValues[0].item)
            {
                // change Bg to Gold
                case ItemID.Gold:
                   // bgRoot.sprite = SpriteManager.Instance.GetSprite(ItemID.dayCoinOpen);
                   if (_state == DailyRewardState.Achieved || _state == DailyRewardState.CountDown)
                    {
                        bgRoot.sprite = SpriteManager.Instance.GetSprite(ItemID.dayCoinOpen);
                    }
                    break;
            }*/
            CanClame();

        }
    }
    private IEnumerator ClaimeForToday()
    {
        yield return new WaitForSeconds(0.5f);
        OnClaim(true);
        btnCelDay.onClick.RemoveAllListeners();
    }
    public void RefreshUI()
    {
        State = data.state;
    }
    public void SetData(DailyRewardData _data)
    {
        data = _data;
        textDay.text = $"Day {data.day + 1} ";
        if (data.day < DailyRewardManager.LastDay)
        {
            for (int i = 0; i < itemValueCells.Count; i++)
            {
                if (i < data.itemValues.Count)
                {
                    itemValueCells[i].SetDataForItem(data.itemValues[i], "+");
                    itemValueCells[i].gameObject.SetActive(true);

                }
                else
                {
                    itemValueCells[i].gameObject.SetActive(false);
                }
            }
           
        }

        RefreshUI();
        TimeSchedule.Instance.OnResetStartTime?.AddListener(RefreshUI);

    }
    public void OnClaim(bool isToday)
    {
        bool isShow = false;
        // AudioAssistant.PlaySound("BtnClick");
        AudioManager.instance.Play("BtnClick");
        foreach (ItemValue item in data.itemValues)
        {

            isShow = true;

            int gold = item.value;
            
            if (gold > 0)
            {
                PopupManager.Instance.OpenPopup<PopupClaimItem>(PopupID.PopupClaimItem, (pop) =>
                {
                    ItemValue reward = new ItemValue(item.item, gold);
                    pop.SetData(reward, 1) ;
                });
                if (item.item == ItemID.thorAmount)
                {
                    PopupManager.Instance.OpenPopup<PopupClaimItem>(PopupID.PopupClaimItem, (pop) =>
                    {
                        ItemValue reward = new ItemValue(ItemID.thorAmount, gold);
                     
                        pop.SetData(reward);
                    });
                    User.Instance[ItemID.thorAmount] += gold;
                }
                else if (item.item == ItemID.Gold)
                {
                    PopupManager.Instance.OpenPopup<PopupClaimItem>(PopupID.PopupClaimItem, (pop) =>
                    {
                        ItemValue reward = new ItemValue(ItemID.Gold, gold);
                        pop.SetData(reward);
                    });
                    User.Instance[ItemID.Gold] += gold;
                }
              /*  else if (item.item != ItemID.Gold && item.item != ItemID.thorAmount)
                {
                    NhanSkin(item.item, User.Instance.UserBots1);
                    NhanSkin(item.item, User.Instance.UserBots2);
                    NhanSkin(item.item, User.Instance.UserBots3);
                    NhanSkin(item.item, User.Instance.UserPlayers1);
                    PopupClaimItem.Instane.btnX2.gameObject.SetActive(false) ;
                    PopupClaimItem.Instane.OkCollect.gameObject.SetActive(true) ;
                    PopupClaimItem.Instane.imgIcon.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    PopupManager.Instance.OpenPopup<PopupClaimItem>(PopupID.PopupClaimItem, (pop) =>
                    {
                        pop.setSkin = true;
                        ItemValue reward = new ItemValue(item.item, gold);
                        //pop.imgIcon.SetNativeSize();
                        pop.SetData(reward) ;
                    });
                }*/
                continue;
            }

            else
            {
                if (isShow) break;
                isShow = true;
                PopupManager.Instance.OpenPopup<PopupClaimItem>(PopupID.PopupClaimItem, (pop) =>
                {
                    ItemValue reward = data.itemValues[0].Clone();
                    pop.SetData(reward, 1);
                });
                RefreshUI();
            }
     
        }

        DailyRewardManager.Instance.ClaimReward(isToday);
    }


    public void NhanSkin(ItemID skinID,List<UserBot> userBots)
    {
        foreach (UserBot userBot in userBots)
        {
            if (userBot.id == skinID)
            {
                if (userBot.isUnlock)
                {
                    User.Instance[ItemID.Gold] += 1000;
                }
                else
                {
                    userBot.isUnlock = true;
                    User.Instance.Save();
                }
            }
        }

        Debug.Log(skinID);
    }


    private void CanClame()
    {
        if (isCanClick)
        {
            btnCelDay.onClick.RemoveAllListeners();
            btnCelDay.onClick.AddListener(() =>
            {
                if (_state == DailyRewardState.CountDown)
                {
                    imgAds.SetActive(false);

                    OnClaim(false);
                    // AdLocation.DailyReward;
                    btnCelDay.onClick.RemoveAllListeners();
                }
                else
                {
                    OnClaim(true);
                    btnCelDay.onClick.RemoveAllListeners();
                }
            });
        }
    }

}

