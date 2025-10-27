using AA_Game;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Thanh.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using Spine;

public class PopupSpin : Popup
{
    public RectTransform arrow;
    public Button btnSpin;
    public UnityEngine.UI.Button btnSpinAds;
    public UnityEngine.UI.Button btnSpinGold;
    private Tween tween;
    [SerializeField] private RawImage imgRaw;

    private int priceSpinGold = 1000;
    [SerializeField] private TextMeshProUGUI txtPriceGold;
    [SerializeField] private Sprite nonEnoughGold, enoughGold;
    [SerializeField] private Sprite setOnReview;

    // [SerializeField] private UnityEngine.UI.Image lineProcess1;
    // [SerializeField] private UnityEngine.UI.Image lineProcess2;
    //  [SerializeField] private UnityEngine.UI.Image lineProcess3;

    public UnityEngine.UI.Slider priceSlider1;
    public UnityEngine.UI.Slider priceSlider2;
    public UnityEngine.UI.Slider priceSlider3;
    [SerializeField] private RewardProcess reward1;
    [SerializeField] private RewardProcess reward2;
    [SerializeField] private RewardProcess reward3;
    public SkeletonGraphic skeletonBtnSpinGold;

    public List<ItemValue> listReWard = new List<ItemValue>
    {
        //(ItemID.Gold,300), // cell 1
        //(ItemID.Gem,2), // cell 2
        //(ItemID.Gold,500), // cell 3
        //(ItemID.Gold,1000), // cell 4
        //(ItemID.Gold,5000), // cell 5
        //(ItemID.Gem,3), // cell 6
        //(ItemID.SpinSkin,0), // cell 7
        //(ItemID.Gem,5), // cell 8
    };

    /*    private ItemID[] listWeponLv1 = new ItemID[4]
        {
            ItemID.Rocket_1,
            ItemID.Wave_1,
            ItemID.Magma_1,
            ItemID.Shotgun_1,
        };
    */
    public List<SpinCell> listSpinCell = new List<SpinCell>();

    ScheduleTask spinFreeDayTask;
    public TextMeshProUGUI txtTimeRefresh;


    private void OnEnable()
    {
        priceSlider1.minValue = 0;
        priceSlider2.minValue = 5;
        priceSlider3.minValue = 10;
        priceSlider1.maxValue = 5;
        priceSlider2.maxValue = 10;
        priceSlider3.maxValue = 15;
        priceSlider1.DOValue(User.Instance.TotalSpin, 0.8f);
        priceSlider2.DOValue(User.Instance.TotalSpin, 0.8f);
        priceSlider3.DOValue(User.Instance.TotalSpin, 0.8f);


        if (tween != null)
        {
            tween.Kill();
        }

        for (int i = 0; i < listSpinCell.Count; i++)
        {
            listSpinCell[i].SetData(listReWard[i].item, listReWard[i].value);
        }

        spinFreeDayTask = TimeSchedule.Instance.GetScheduleTask(ScheduleID.FreeSpinDeLay);


        //spin free
        btnSpin.onClick.RemoveAllListeners();
        btnSpin.onClick.AddListener(OnSpinFree);

        //spin ads
        btnSpinAds.onClick.RemoveAllListeners();
        btnSpinAds.onClick.AddListener(OnSpinAds);

        //spin Gold
        btnSpinGold.onClick.RemoveAllListeners();
        btnSpinGold.onClick.AddListener(OnSpinGold);

        arrow.localRotation = Quaternion.Euler(Vector3.zero);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(arrow.DOLocalRotate(new Vector3(0f, 0f, 80), 10f, DG.Tweening.RotateMode.FastBeyond360));
        sequence.SetLoops(-1, LoopType.Restart);
        tween = sequence;
        Refresh();
        float totalSpin = User.Instance.TotalSpin;
        reward1.SetStatus(totalSpin >= 5);
        reward2.SetStatus(totalSpin >= 10);
        reward3.SetStatus(totalSpin >= 15);
        float percent = totalSpin / 15f;


        //lineProcess1.fillAmount = percent;


        priceSlider1.value = percent; priceSlider2.value = percent; priceSlider3.value = percent;


        StartCoroutine(RunRawImage());
        if (User.Instance[ItemID.Gold] < priceSpinGold)
        {
            skeletonBtnSpinGold.AnimationState.SetAnimation(0, "0_blue", true);
            skeletonBtnSpinGold.Skeleton.SetSlotsToSetupPose();
        }
        else
        {
            skeletonBtnSpinGold.AnimationState.SetAnimation(0, "0_green", true);
            skeletonBtnSpinGold.Skeleton.SetSlotsToSetupPose();
        }
    }

    void Refresh()
    {
        if (spinFreeDayTask.IsFinished)
        {
            User.Instance.IsSpinDailyAds = true;
        }

        //float percent = (float)User.Instance.TotalSpin / 5f;
        if (isRotate)
        {
            priceSlider1.DOValue(User.Instance.TotalSpin, 0.8f);
            priceSlider2.DOValue(User.Instance.TotalSpin, 0.8f);
            priceSlider3.DOValue(User.Instance.TotalSpin, 0.8f).OnComplete(() =>
            {
                isRotate = false;

                switch (User.Instance.TotalSpin)
                {
                    case 5:

                        ItemValue reward = new ItemValue(ItemID.Gold, 10000);
                        PopupManager.Instance.OpenPopup<PopupClaimItem>(PopupID.PopupClaimItem, (pop) =>
                        {
                            pop.SetData(reward, 1);
                            reward1.SetStatus(true);

                        });
                        User.Instance[ItemID.Gold] += reward.value;
                        //PopupManager.Instance.OpenPopup<PopupUnlockSkin>(PopupID.PopupUnlockSkin,
                        //    (pop) => { pop.SetData(ItemID.Rocket_3); });
                        //reward1.SetStatus(true);
                        /* int randomGold = Random.Range(1000, 2000);
                         User.Instance[ItemID.Gold] += randomGold;
                         */
                        // ItemValue reward = new ItemValue(ItemID.Gold, randomGold);

                        break;
                    case 10:

                        ItemValue rewardhammer = new ItemValue(ItemID.thorAmount, 15);
                        PopupManager.Instance.OpenPopup<PopupClaimItem>(PopupID.PopupClaimItem, (pop) =>
                        {
                            pop.SetData(rewardhammer, 1);
                            reward1.SetStatus(true);
                        });
                        User.Instance[ItemID.Gold] += rewardhammer.value;
                        reward2.SetStatus(true);
                        break;
                    case 15:

                        ItemID _skin;
                        //_skin = User.Instance.UserPlayers1.GetRandom().id;
                        _skin = ItemType.SkinsMain.GetRandom();
                        ItemValue Skin = new ItemValue(_skin, 1);
                        PopupClaimItem.Instane.btnX2.gameObject.SetActive(false);
                        PopupClaimItem.Instane.OkCollect.gameObject.SetActive(true);
                        PopupManager.Instance.OpenPopup<PopupClaimItem>(PopupID.PopupClaimItem, (pop) =>
                        {
                            pop.SetData(Skin, 1);
                            reward1.SetStatus(true); 
                            NhanSkin(Skin.item, User.Instance.UserPlayers1);
                        });
                    
                        reward3.SetStatus(true);
                        break;
                }

            });
            if (User.Instance.TotalSpin > 15)
            {
                User.Instance.TotalSpin = 1;
                priceSlider1.DOValue(User.Instance.TotalSpin, 0.8f);
                priceSlider2.DOValue(User.Instance.TotalSpin, 0.8f);
                priceSlider3.DOValue(User.Instance.TotalSpin, 0.8f);
                reward1.SetStatus(false);
                reward2.SetStatus(false);
                reward3.SetStatus(false);
            }
        }

        txtPriceGold.text = $"{priceSpinGold}";
        //btnSpinGold.image.sprite = User.Instance[ItemID.Gold] < priceSpinGold ? nonEnoughGold : enoughGold;
        if (User.Instance[ItemID.Gold] < priceSpinGold)
        {
            skeletonBtnSpinGold.AnimationState.SetAnimation(0, "0_blue", true);
            skeletonBtnSpinGold.Skeleton.SetSlotsToSetupPose();
        }
        else
        {
            skeletonBtnSpinGold.AnimationState.SetAnimation(0, "0_green", true);
            skeletonBtnSpinGold.Skeleton.SetSlotsToSetupPose();
        }
        btnSpin.gameObject.SetActive(spinFreeDayTask.IsFinished);
        btnSpinAds.gameObject.SetActive(/*true*/!spinFreeDayTask.IsFinished /*&& User.Instance.IsSpinDailyAds*/);
        // btnSpinAds.gameObject.SetActive(false);
        btnSpinGold.gameObject.SetActive(true/*!spinFreeDayTask.IsFinished && !User.Instance.IsSpinDailyAds*/);
    }

    void OnSpinAds()
    {
        if (isRotate)
        {
            return;
        }

        // AudioAssistant.PlaySound("BtnClick");
        AudioManager.instance.Play("BtnClick");
        //BuyManager.Instance.Buy(new List<ItemValueFloat> { new ItemValueFloat(ItemID.Ads, 1) }, null, (isSuccess) =>
        //{
        //   if (isSuccess)
        //    {
        btnSpinAds.gameObject.SetActive(false);
        User.Instance.IsSpinDailyAds = false;
        Spin();
        //    }
        //    else
        //    {
        //        PopupManager.Instance.OpenPopup<PopupNotice>(PopupID.PopupNotice,
        //            (pop) => { pop.SetData("RETRY", "Video Ads not available"); });
        //    }
        //}, AdLocation.Spin);
    }

    void OnSpinFree()
    {
        //AudioAssistant.PlaySoundWhile("BtnClick",x =>);
        if (isRotate)
        {
            return;
        }

        // AudioAssistant.PlaySound("BtnClick");
        AudioManager.instance.Play("BtnClick");
        btnSpin.gameObject.SetActive(false);
        Spin();
        /*    if (User.Instance[ItemID.Gold] >= priceSpinGold)
           {
               User.Instance[ItemID.Gold] -= priceSpinGold;
           }*/
        spinFreeDayTask.Start();
    }

    private void OnSpinGold()
    {
        if (isRotate)
        {
            return;
        }
        //if (!success) return;
        //new ItemValueFloat(ItemID.Gold, priceSpinGold);
        if (User.Instance[ItemID.Gold] >= priceSpinGold)
        {
            User.Instance[ItemID.Gold] -= priceSpinGold;
            btnSpinGold.gameObject.SetActive(false);
            //  btnSpinGold.interactable = true;

            Spin();
            // AudioAssistant.PlaySound("BtnClick");
            AudioManager.instance.Play("BtnClick");
        }
        /* else
         {
            // btnSpinGold.interactable = false;
         }*/
        /* BuyManager.Instance.Buy(new List<ItemValueFloat>
         {

         }, null, (success) =>
         {
             if (!success) return;
             btnSpinGold.gameObject.SetActive(false);
             Spin();
         }, AdLocation.Spin);*/
    }

    void Spin()
    {
        tween.Kill();
        isRotate = true;
        //AudioAssistant.PlaySoundWhile("LuckySpin", () => isRotate);
        AudioManager.instance.Play("musicspin");
        int index = Random.Range(0, listReWard.Count);
        //int index = 7;

        int rotateCount = 6;
        float z = GetRotate(index) - (360f * rotateCount) - 90;
        arrow.DOLocalRotate(new Vector3(0f, 0f, z), rotateCount * 0.5f)
            .SetEase(SpinTypes[Random.Range(0, SpinTypes.Length)])
            .OnComplete(() =>
            {

                OnClaim(index);
            });
        //   Debug.Log(User.Instance.TotalSpin);
    }

    void OnClaim(int index)
    {
        User.Instance.TotalSpin++;
        var itemReward = listReWard[index];
        int gold = itemReward.value;
        if (itemReward.item == ItemID.Gold)
        {
            PopupManager.Instance.OpenPopup<PopupClaimItem>(PopupID.PopupClaimItem,
                (pop) =>
                {
                    pop.SetData(itemReward);
                    pop.onClose.AddListener(Refresh);
                    //isRotate = false;
                });
            User.Instance[ItemID.Gold] += gold;

        }

        else if (itemReward.item == ItemID.thorAmount)
        {

            PopupManager.Instance.OpenPopup<PopupClaimItem>(PopupID.PopupClaimItem,
                (pop) =>
                {
                    pop.SetData(itemReward);
                    pop.onClose.AddListener(Refresh);
                    //isRotate = false;
                });
            User.Instance[ItemID.thorAmount] += gold;
        }
       else if (itemReward.item != ItemID.Gold && itemReward.item != ItemID.thorAmount)
        {
            itemReward.item = ItemType.SkinsBot.GetRandom();
            PopupManager.Instance.OpenPopup<PopupClaimItem>(PopupID.PopupClaimItem,
               (pop) =>
               {
                   pop.setSkin = true;
                   ItemValue reward = new ItemValue(itemReward.item, gold);
                   //pop.imgIcon.SetNativeSize();
                 
                   pop.SetData(reward);
               });
         /*   NhanSkin(itemReward.item, User.Instance.UserBots1);
            NhanSkin(itemReward.item, User.Instance.UserBots2);
            NhanSkin(itemReward.item, User.Instance.UserBots3);*/
        }
        if (gold > 0)
        {
            PopupManager.Instance.OpenPopup<PopupClaimItem>(PopupID.PopupClaimItem, (pop) =>
            {
                ItemValue reward = new ItemValue(itemReward.item, gold);
                pop.SetData(reward, 1);
            });


        }



     /*   if (itemReward.item != ItemID.Gold && itemReward.item != ItemID.thorAmount)
        {
            itemReward.item = ItemType.SkinsBot.GetRandom();
            NhanSkin(itemReward.item, User.Instance.UserBots1);
            NhanSkin(itemReward.item, User.Instance.UserBots2);
            NhanSkin(itemReward.item, User.Instance.UserBots3);
            NhanSkin(itemReward.item, User.Instance.UserPlayers1);
            PopupManager.Instance.OpenPopup<PopupClaimItem>(PopupID.PopupClaimItem,
               (pop) =>
               {
                   pop.setSkin = true;
                   ItemValue reward = new ItemValue(itemReward.item, 1);
                   //pop.imgIcon.SetNativeSize();
                   pop.SetData(reward);
               });
        }*/
     
        //if(itemReward.item != ItemID.Gold && itemReward.item != ItemID.thorAmount)
        //{
            
        //    ItemValue Skin = new ItemValue(itemReward.item, 1);
        //    PopupClaimItem.Instane.btnX2.gameObject.SetActive(false);
        //    PopupClaimItem.Instane.OkCollect.gameObject.SetActive(true);
        //    PopupManager.Instance.OpenPopup<PopupClaimItem>(PopupID.PopupClaimItem, (pop) =>
        //    {
        //        pop.SetData(Skin, 1);
        //        reward1.SetStatus(true);
        //        NhanSkin(itemReward.item, User.Instance.UserBots1);
        //        NhanSkin(itemReward.item, User.Instance.UserBots2);
        //        NhanSkin(itemReward.item, User.Instance.UserBots3);
        //        NhanSkin(itemReward.item, User.Instance.UserPlayers1);
        //    });

        //}

        /*else
        {
            ItemID idRandom = listWeponLv1.GetRandom();
            if (User.Instance[idRandom] == 1)
            {
                PopupManager.Instance.OpenPopup<PopupClaimItem>(PopupID.PopupClaimItem, (pop) =>
                {
                    int randomGold = Random.Range(1000, 2000);
                    ItemValue reward = new ItemValue(ItemID.Gold, randomGold);
                    pop.SetData(reward);
                    pop.onClose.AddListener(Refresh);
                });
            }
            else
            {
                PopupManager.Instance.OpenPopup<PopupUnlockSkin>(PopupID.PopupUnlockSkin,
                    (pop) =>
                    {
                        pop.SetData(idRandom);
                        pop.onClose.AddListener(Refresh);
                    });
            }
        }*/
    }
    public void NhanSkin(ItemID skinID, List<UserBot> userBots)
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
    float GetRotate(int index)
    {
        return 45 * index + Random.Range(-20f, 20f);
    }

    Ease[] SpinTypes = new Ease[7]
    {
        Ease.OutSine,
        Ease.OutQuad,
        Ease.OutCubic,
        Ease.OutQuart,
        Ease.OutQuint,
        Ease.OutExpo,
        Ease.OutCirc
    };


    bool isRotate = false;

    private void Update()
    {
        if (spinFreeDayTask != null)
        {
            if (spinFreeDayTask.IsFinished)
            {
                txtTimeRefresh.text = "Ready";
            }
            else
            {
                txtTimeRefresh.text = TimeSchedule.GetStringTime(spinFreeDayTask.remainSeconds);
            }
        }
    }

    public override void Close()
    {
        if (isRotate)
        {
            return;
        }

        base.Close();
        //  MenuScene.main.CheckNotification();
        isRotate = false;
    }

    IEnumerator RunRawImage()
    {
        float x = 0;
        float speed = 0.2f;
        while (true)
        {
            x += Time.deltaTime;
            Rect a = imgRaw.uvRect;
            a.x -= Time.deltaTime * speed;
            a.y -= Time.deltaTime * speed;
            imgRaw.uvRect = a;
            yield return null;
        }
    }
}