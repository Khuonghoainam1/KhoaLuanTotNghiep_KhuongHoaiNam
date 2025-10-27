using Thanh.Core;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class PopupCarUpgrade : Popup
{
    public Button upgradeCarBtn;
    public Button carButton;
    public Button skinsButton;
    public GameObject maxCar;
    public GameObject groupStatsCar;
    public GroupSkinUI groupSkin;
    public CarUI carUI;

    public UserCar userCar;
    public CarLevelData carLevelData;
    public HeroesLevelData heroesLevelData;
    public ParticleSystem particleSystemUpgrade;
    public GameObject groupExpCar;
    public Sprite[] selectBtn; //0 normal ,1- select
    public GameObject[] plusObject;
    public TMP_Text goldNeed;
    public Slider sliderExpCar;
    public Image nextSkin;
    public Image currentSkin;

    public GameObject[] fxUnlockSlot;

    public GameObject tutCarUpgrade;
    public GameObject tutTabHeroUp;
    public GameObject tutUpHeroUp;

    public Transform parentBotTut;
    public Transform parentOrigin;
    public GameObject tutSelectHero;

    public void OnEnable()
    {
        upgradeCarBtn.onClick.RemoveListener(Upgrade);
        carButton.onClick.RemoveListener(OpenCarTab);
        skinsButton.onClick.RemoveListener(OpenSkinsTab);

        upgradeCarBtn.onClick.AddListener(Upgrade);
        carButton.onClick.AddListener(OpenCarTab);
        skinsButton.onClick.AddListener(OpenSkinsTab);
    }

    public void SetPlus()
    {
        if(User.Instance.Car.slot == 1)
        {
            plusObject[0].SetActive(false);
            plusObject[1].SetActive(true);
            plusObject[2].SetActive(true);
        }
        if (User.Instance.Car.slot == 2)
        {
            plusObject[0].SetActive(false);
            plusObject[1].SetActive(false);
            plusObject[2].SetActive(true);
        }
        if (User.Instance.Car.slot == 3)
        {
            plusObject[0].SetActive(false);
            plusObject[1].SetActive(false);
            plusObject[2].SetActive(false);
        }
    }

    public override void OnShow()
    {
        base.OnShow();
        userCar = User.Instance.Car;

        #region HIDE BUTTON UPGRADE IF MAX
        if(User.Instance.Car.level == carLevelData.carLevelDatas.Count-1)
        {
            upgradeCarBtn.gameObject.SetActive(false);
            maxCar.gameObject.SetActive(true);
        }
        else
        {
            upgradeCarBtn.gameObject.SetActive(true);
            maxCar.gameObject.SetActive(false);
        }
        #endregion
        OpenCarTab();
        SetPlus();
        sliderExpCar.maxValue = userCar.expNeedToNewSkin;
        sliderExpCar.DOValue(User.Instance[ItemID.expCarCurrent],0.5f);
        nextSkin.sprite = Resources.Load<Sprite>("CarIcon/" + userCar.newSkin);
        currentSkin.sprite = Resources.Load<Sprite>("CarIcon/" + userCar.skin);

        if(User.Instance[ItemID.TutUpgradeCar] < 1)
        {
            tutCarUpgrade.SetActive(true);
        }
        else
        {
            tutCarUpgrade.SetActive(false);
        }

        if (User.Instance[ItemID.TutUpgradeCar] == 1)
        {
            tutTabHeroUp.SetActive(true);
        }
        else
        {
            tutTabHeroUp.SetActive(false);
        }
    }

    public void Upgrade()
    {
        if (User.Instance[ItemID.Gold] < userCar.gold)
        {
            //open shop gold
            PopupManager.Instance.OpenPopup<PopupShop>(PopupID.PopupShop);
            return;
        }

        //save tut
        if (User.Instance[ItemID.TutUpgradeCar] < 2)
        {
            tutCarUpgrade.SetActive(false);
            tutTabHeroUp.SetActive(true);
        }
        if (User.Instance[ItemID.TutUpgradeCar] < 1)
        {
            User.Instance[ItemID.TutUpgradeCar] = 1;
        }

        //exp
        User.Instance[ItemID.expCarCurrent] += 100;
        if(User.Instance[ItemID.expCarCurrent] == userCar.expNeedToNewSkin)
        {
            User.Instance[ItemID.expCarCurrent] = 0;
        }     

        User.Instance[ItemID.Gold] -= (int)userCar.gold;
        User.Instance.Car = carLevelData.carLevelDatas[userCar.level+1];
        userCar = User.Instance.Car;

        sliderExpCar.maxValue = userCar.expNeedToNewSkin;
        sliderExpCar.value = User.Instance[ItemID.expCarCurrent];
        currentSkin.sprite = Resources.Load<Sprite>("CarIcon/" + userCar.skin);
        nextSkin.sprite = Resources.Load<Sprite>("CarIcon/" + userCar.newSkin);

        // nang cap toi level co bot
        if (userCar.level == carLevelData.levelSlot1)
        {
            //fxUnlockSlot[0].SetActive(true);
            //Destroy(fxUnlockSlot[0],2f);
            User.Instance.UserBots1[0].isUnlock = true;
            User.Instance.UserBots1[0].isUsing = true;
            User.Instance.UserBot1Using = User.Instance.UserBots1[0];
            GameEvent.OnCarUnlockSlot.Invoke();

            //show pop new skin
            groupSkin.unlockPop.gameObject.SetActive(true);
            groupSkin.unlockPop.SetUp(User.Instance.UserBots1[0],true);
        }
        else if (userCar.level == carLevelData.levelSlot2)
        {
            //fxUnlockSlot[1].SetActive(true);
            //Destroy(fxUnlockSlot[1], 2f);
            User.Instance.UserBots2[0].isUnlock = true;
            User.Instance.UserBots2[0].isUsing = true;
            User.Instance.UserBot2Using = User.Instance.UserBots2[0];
            GameEvent.OnCarUnlockSlot.Invoke();

            //show pop new skin
            groupSkin.unlockPop.gameObject.SetActive(true);
            groupSkin.unlockPop.SetUp(User.Instance.UserBots2[0],true);
        }
        else if (userCar.level == carLevelData.levelSlot3)
        {
            //fxUnlockSlot[2].SetActive(true);
            //Destroy(fxUnlockSlot[2], 2f);
            User.Instance.UserBots3[0].isUnlock = true;
            User.Instance.UserBots3[0].isUsing = true;
            User.Instance.UserBot3Using = User.Instance.UserBots3[0];
            GameEvent.OnCarUnlockSlot.Invoke();

            //show pop new skin
            groupSkin.unlockPop.gameObject.SetActive(true);
            groupSkin.unlockPop.SetUp(User.Instance.UserBots3[0],true);
        }
        User.Instance.Save();
        #region HIDE BUTTON UPGRADE IF MAX
        if (User.Instance.Car.level == carLevelData.carLevelDatas.Count-1)
        {
            upgradeCarBtn.gameObject.SetActive(false);
            maxCar.gameObject.SetActive(true);
        }
        else
        {
            upgradeCarBtn.gameObject.SetActive(true);
            maxCar.gameObject.SetActive(false);
        }
        #endregion
        GameEvent.OnCarLevelUp.Invoke();
        SetPlus();


        particleSystemUpgrade.Simulate(0.0f, true, true);
        particleSystemUpgrade.Play();
        goldNeed.text = ((int)userCar.gold).ToKMB();
        AudioManager.instance.Play("levelUp");
    }

    public void OpenCarTab()
    {
        groupStatsCar.SetActive(true);
        groupSkin.Close();
        carUI.HideBot();
        groupExpCar.SetActive(true);
        carButton.GetComponent<Image>().sprite = selectBtn[1];
        skinsButton.GetComponent<Image>().sprite = selectBtn[0];
        goldNeed.text = ((int)userCar.gold).ToKMB();
        #region HIDE BUTTON UPGRADE IF MAX
        if (User.Instance.Car.level == carLevelData.carLevelDatas.Count - 1)
        {
            upgradeCarBtn.gameObject.SetActive(false);
            maxCar.gameObject.SetActive(true);
        }
        else
        {
            upgradeCarBtn.gameObject.SetActive(true);
            maxCar.gameObject.SetActive(false);
        }
        #endregion
    }

    public void OpenSkinsTab()
    {
        if (User.Instance[ItemID.TutUpgradeCar] < 2)
        {
            tutTabHeroUp.SetActive(false);
            //tutUpHeroUp.SetActive(true);
            tutSelectHero.SetActive(true);
            carUI.bot1.transform.parent = parentBotTut;
        }
        else
        {
            tutSelectHero.SetActive(false);
            carUI.bot1.transform.parent = parentOrigin;
        }
        carButton.GetComponent<Image>().sprite = selectBtn[0];
        skinsButton.GetComponent<Image>().sprite = selectBtn[1];
        groupExpCar.SetActive(false);
        groupStatsCar.SetActive(false);
        groupSkin.Open();
        carUI.SpawnBot();
        upgradeCarBtn.gameObject.SetActive(false);
        maxCar.gameObject.SetActive(false);
    }

    public void SpawnSkinGroup(int indexBot)
    {
        if(indexBot == 0)
        {
            //skin player
        }
        else if(indexBot == 1)
        {
            //skin bot 1
        }
        else if(indexBot == 2)
        {
            //skin bot 2
        }
        else
        {
            //skin bot3
        }
    }
}
