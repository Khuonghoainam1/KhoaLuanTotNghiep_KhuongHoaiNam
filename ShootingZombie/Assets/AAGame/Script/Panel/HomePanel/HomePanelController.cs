using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Thanh.Core;
using DG.Tweening;
using System.Collections.Generic;
using System;
using AA_Game;

public class HomePanelController : MonoBehaviour
{
    public Button playButton;
    public Button workShop;
    public Button carUpgrade;
    public Button btnSlectmap;
    public Button btnSettings;
    public Button bossWorld;
    public Button collectFuel;
    public Button endLessBtn;
    public Button upgradeHomeStationBtn;
    [SerializeField] private Button btnDailyReward;
    public TMP_Text levelText;
    public TMP_Text thorAmountTopUpgrade;
    public TMP_Text ticketAmountText;

    public Image thorImage;
    public TMP_Text thorAmount;
    public Button freeSkinBtn;
    public Button heroesBtn;
    public Button talentBtn;
    public Button selectModeBtn;
    public Button btnSpinWheel;
    public Button btnCollection;
    public Button btnPopupShop;

    public Button shopCoin;
    public Button shopHammer;

    public GameObject tutUpgradeCar;
    public GameObject tutTalent;

    private void OnEnable()
    {
        /*playButton.onClick.RemoveListener(PlayButton);
        playButton.onClick.AddListener(PlayButton);*/

        carUpgrade.onClick.RemoveListener(OpenCarUpgrade);
        carUpgrade.onClick.AddListener(OpenCarUpgrade);

        workShop.onClick.RemoveListener(OpenWorkShop);
        workShop.onClick.AddListener(OpenWorkShop);

        btnSlectmap.onClick.RemoveListener(OpenSelectMap);
        btnSlectmap.onClick.AddListener(OpenSelectMap);

        btnSettings.onClick.RemoveListener(OpenSetting);
        btnSettings.onClick.AddListener(OpenSetting);

        btnDailyReward.onClick.RemoveListener(OpenDailyReward);
        btnDailyReward.onClick.AddListener(OpenDailyReward);

        //bossWorld.onClick.RemoveListener(OpenSelectBoss);
        //bossWorld.onClick.AddListener(OpenSelectBoss);

        //endLessBtn.onClick.RemoveListener(PlayModeEndLess);
        //endLessBtn.onClick.AddListener(PlayModeEndLess);

        //collectFuel.onClick.RemoveListener(OpenCollectFuel);
        //collectFuel.onClick.AddListener(OpenCollectFuel);

        upgradeHomeStationBtn.onClick.RemoveListener(UpdrageHomeStation);
        upgradeHomeStationBtn.onClick.AddListener(UpdrageHomeStation);

        freeSkinBtn.onClick.RemoveListener(FreeSkinBtn);
        freeSkinBtn.onClick.AddListener(FreeSkinBtn);

        heroesBtn.onClick.RemoveListener(OpenHeroesPopup);
        heroesBtn.onClick.AddListener(OpenHeroesPopup);

        talentBtn.onClick.RemoveListener(OpenTalentPopup);
        talentBtn.onClick.AddListener(OpenTalentPopup);

        selectModeBtn.onClick.RemoveListener(OpenPopupSelectMode);
        selectModeBtn.onClick.AddListener(OpenPopupSelectMode);

        btnSpinWheel.onClick.RemoveListener(OpenSpinWheel);
        btnSpinWheel.onClick.AddListener(OpenSpinWheel);

        btnCollection.onClick.RemoveListener(OpenCollection);
        btnCollection.onClick.AddListener(OpenCollection);

        btnPopupShop.onClick.RemoveListener(OpenPopupShop);
        btnPopupShop.onClick.AddListener(OpenPopupShop);

        shopCoin.onClick.RemoveListener(OpenPopupShop);
        shopCoin.onClick.AddListener(OpenPopupShop);

        shopHammer.onClick.RemoveListener(OpenPopupShop);
        shopHammer.onClick.AddListener(OpenPopupShop);

        GameEvent.OnMoveToPlay.AddListener(() => { this.transform.localScale = Vector3.zero; });

        levelText.text = "STAGE " + (User.Instance[ItemID.PlayingLevel] + 1).ToString();

        if (User.Instance[ItemID.IsAutoPlay] == 1 && GameScene.prevSceneName == SceneName.GameScene)
        {
            gameObject.transform.localScale = Vector3.zero;
        }

        SetupThorUI(false);
        if (User.Instance[ItemID.levelHomeStation] >= 5)
        {
            upgradeHomeStationBtn.gameObject.SetActive(false);
        }


        GameEvent.OnUpgradeGara.Invoke();

        //off button free skin
        //if (User.Instance[ItemID.solanfreeSkin] == 3)
        //{
        //    freeSkinBtn.gameObject.SetActive(false);
        //}
        //List<ItemID> skinRemain = new List<ItemID>();
        //skinRemain.AddRange(ItemType.Skins);
        //foreach (ItemID skin in User.Instance.ListCharSkin())
        //{
        //    skinRemain.Remove(skin);
        //}
        //if (skinRemain.Count == 0)
        //{
        //    freeSkinBtn.gameObject.SetActive(false);
        //}

        if(User.Instance[ItemID.TutUpgradeCar] < 2)
        {
            tutUpgradeCar.SetActive(true);
        }
        else
        {
            tutUpgradeCar.SetActive(false);
        }

        if (User.Instance[ItemID.tutTalent] < 1 && User.Instance[ItemID.PlayingLevel] == 2)
        {
            tutTalent.SetActive(true);
        }
        else
        {
            tutTalent.SetActive(false);
        }


    }
    private void Start()
    {
        if (GlobalData.instance.isOpenUpgrade == true)
        {
            PopupManager.Instance.OpenPopup<PopupCarUpgrade>(PopupID.PopupCarUpgrade);
            GlobalData.instance.isOpenUpgrade = false;
        }

    }

    public void SetupThorUI(bool isUp)
    {
        thorAmount.text = User.Instance[ItemID.thorAmount].ToString();
        thorAmountTopUpgrade.text = User.Instance[ItemID.thorUpgraded].ToString() + " / " + StationManager.Instance.thorToLevelUp[User.Instance[ItemID.levelHomeStation]].ToString();
        float x = User.Instance[ItemID.thorUpgraded];
        float y = StationManager.Instance.thorToLevelUp[User.Instance[ItemID.levelHomeStation]];

        if (isUp == false)
        {
            thorImage.DOFillAmount(x / y, 1f);
        }
        else
        {
            thorImage.DOFillAmount(0.99f, 1f).OnComplete(() =>
            {
                thorImage.fillAmount = 0;
                GameEvent.OnUpgradeGara.Invoke();
                if (User.Instance[ItemID.levelHomeStation] >= 5)
                {
                    upgradeHomeStationBtn.gameObject.SetActive(false);
                }
            });
        }
    }

    public void UpdrageHomeStation()
    {
        //if (User.Instance[ItemID.thorAmount] > 0)
        //{
        //    int soluongCan = StationManager.Instance.thorToLevelUp[User.Instance[ItemID.levelHomeStation]] - User.Instance[ItemID.thorUpgraded];

        //    if (User.Instance[ItemID.thorAmount] < soluongCan)
        //    {
        //        User.Instance[ItemID.thorUpgraded] += User.Instance[ItemID.thorAmount];
        //        User.Instance[ItemID.thorAmount] = 0;
        //        User.Instance.Save();
        //        SetupThorUI(false);
        //    }
        //    else if (User.Instance[ItemID.thorAmount] == soluongCan)
        //    {
        //        User.Instance[ItemID.thorAmount] = 0;
        //        User.Instance[ItemID.thorUpgraded] = 0;
        //        User.Instance[ItemID.levelHomeStation] += 1;
        //        User.Instance.Save();
        //        SetupThorUI(true);
        //    }
        //    else
        //    {
        //        User.Instance[ItemID.thorAmount] = User.Instance[ItemID.thorAmount] - soluongCan;
        //        User.Instance[ItemID.thorUpgraded] = 0;
        //        User.Instance[ItemID.levelHomeStation] += 1;
        //        User.Instance.Save();
        //        SetupThorUI(true);
        //    }

        //}

        if (User.Instance[ItemID.thorAmount] > 0)
        {
            User.Instance[ItemID.thorAmount] -= 1;
            User.Instance[ItemID.thorUpgraded] += 1;
            User.Instance.Save();
            if(User.Instance[ItemID.thorUpgraded] == StationManager.Instance.thorToLevelUp[User.Instance[ItemID.levelHomeStation]])
            {
                User.Instance[ItemID.levelHomeStation] += 1;
                User.Instance[ItemID.thorUpgraded] = 0;
                User.Instance.Save();
                SetupThorUI(true);
            }
            else
            {
                SetupThorUI(false);
            }
        }


        AudioManager.instance.Play("levelUp2");

    }

    void OpenCollection()
    {
        AudioManager.instance.Play("BtnClick");
        PopupManager.Instance.OpenPopup<PopupCollection>(PopupID.PopupCollection);
    }


   /* public void PlayButton()
    {

        GlobalData.instance.levelToPlay = User.Instance[ItemID.PlayingLevel];
        GlobalData.gameMode = GameMode.Normal;
        AudioManager.instance.Play("BtnClick");
        GlobalData.instance.isAutoPlay = true;
        GameEvent.OnMoveToPlay.Invoke();
    }
*/
    public void PlayBySelectLevel()
    {
        AudioManager.instance.Play("BtnClick");
        StartCoroutine(GameManager.Instance.StartGame());   
        gameObject.transform.localScale = Vector3.zero;
    }

    public void OpenSpinWheel()
    {
        AudioManager.instance.Play("BtnClick");
        PopupManager.Instance.OpenPopup<PopupSpin>(PopupID.PopupSpinWheel);    
    }
    public void OpenPopupShop()
    {
        AudioManager.instance.Play("BtnClick");
        PopupManager.Instance.OpenPopup<PopupShop>(PopupID.PopupShop);
    }
    public void OpenSetting()
    {
        AudioManager.instance.Play("BtnClick");
        PopupManager.Instance.OpenPopup<PopupSetting>(PopupID.PopupSetting);
    }

    public void OpenWorkShop()
    {
        AudioManager.instance.Play("BtnClick");
        PopupManager.Instance.OpenPopup<InventoryManager>(PopupID.PopupWorkShop);
    }

    public void OpenSelectMap()
    {
        AudioManager.instance.Play("BtnClick");
        GlobalData.gameMode = GameMode.Normal;
        User.Instance.Save();
        PopupManager.Instance.OpenPopup<PopupSelectLevel>(PopupID.PopupSelectMap);
    }

    public void OpenCarUpgrade()
    {
        AudioManager.instance.Play("BtnClick");
        PopupManager.Instance.OpenPopup<PopupCarUpgrade>(PopupID.PopupCarUpgrade);
    }

    public void OpenDailyReward()
    {
        AudioManager.instance.Play("BtnClick");
        PopupManager.Instance.OpenPopup<PopupDailyReward>(PopupID.PopupDailyReward);
    }

    public void OpenSelectBoss()
    {
        AudioManager.instance.Play("BtnClick");
        GlobalData.gameMode = GameMode.BossWorld;
        User.Instance.Save();
        PopupManager.Instance.OpenPopup<PopupSelectBoss>(PopupID.PopupSelectBoss);
       
        Debug.Log(User.Instance.GameMode);
    }

    public void OpenCollectFuel()
    {

        AudioManager.instance.Play("BtnClick");
        GlobalData.gameMode = GameMode.CollectFuel;
        User.Instance.Save();
        PopupManager.Instance.OpenPopup<PopupReviewReward>(PopupID.PopupReviewReward, (pop) => {
            pop.Items[0].gameObject.SetActive(true);
            pop.Items[1].gameObject.SetActive(true);
            pop.Items[2].gameObject.SetActive(false);
            //icon Object
            pop.SetData(1);


        });
        Debug.Log(User.Instance.GameMode);
    }

    public void PlayModeEndLess()
    {
        AudioManager.instance.Play("BtnClick");
        GlobalData.gameMode = GameMode.Endless;
        User.Instance.Save();
        PopupManager.Instance.OpenPopup<PopupReviewReward>(PopupID.PopupReviewReward, (pop) => {
            pop.Items[0].gameObject.SetActive(true);
            pop.Items[1].gameObject.SetActive(false);
            pop.Items[2].gameObject.SetActive(false);
            pop.SetData(2);
        });
    }



    /// <summary>
    /// Free skin by ads
    /// </summary>
    public void FreeSkinBtn()
    {
        User.Instance[ItemID.solanfreeSkin] += 1;
        if (User.Instance[ItemID.solanfreeSkin] == 3)
        {
            freeSkinBtn.gameObject.SetActive(false);
        }

        //random skin ko trung voi skin cu
        List<ItemID> skinRemain = new List<ItemID>();
        skinRemain.AddRange(ItemType.Skins);
        foreach (ItemID skin in User.Instance.ListCharSkin())
        {
            skinRemain.Remove(skin);
        }

        ItemID skinRandom = skinRemain.GetRandom();
        User.Instance.ListCharSkin().Add(skinRandom);
        User.Instance.CurrentPlayerSkin = skinRandom;
        User.Instance.Save();

        if (User.Instance.ListCharSkin().Count == ItemType.Skins.Count)
        {
            freeSkinBtn.gameObject.SetActive(false);
        }
        //show popup new skin
        PopupManager.Instance.OpenPopup<PopupNewUnlockSkin>(PopupID.PopupNewUnlockSkin, (pop) => pop.SetData(skinRandom.ToString()));
        //thay skin cho user

        GameEvent.OnUnlockNewSkin.Invoke(null);
    }


    public void OpenHeroesPopup()
    {
        AudioManager.instance.Play("BtnClick");
        PopupManager.Instance.OpenPopup<HeroesPopup>(PopupID.PopupHeroes, (pop) => pop.SetSkin(User.Instance.CurrentPlayerSkin.ToString()));
    }

    public void OpenTalentPopup()
    {
        AudioManager.instance.Play("BtnClick");
        PopupManager.Instance.OpenPopup<PopupTalent>(PopupID.PopupTalent);
    }

    public void OpenPopupSelectMode()
    {
        AudioManager.instance.Play("BtnClick");
        PopupManager.Instance.OpenPopup<PopupSelectMode>(PopupID.PopupSelectMode);
    }
}
