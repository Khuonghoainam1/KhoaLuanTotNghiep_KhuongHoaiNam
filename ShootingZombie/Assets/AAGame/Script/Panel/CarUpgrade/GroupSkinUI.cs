using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yurowm.GameCore;

public class GroupSkinUI : MonoBehaviour
{
    public Transform groupSkin;
    public GameObject[] buttonBot;
    public Button unlockSkin;
    public Button equipBtn;
    public Button equipedBtn;
    public IconHero[] iconHeroes;
    public GameObject[] focus;
    public TMPro.TMP_Text price;
    public TMPro.TMP_Text goldNeedToUp;
    public Button levelUp;
    public Button lockButtonLvUp;
    public TMPro.TMP_Text nameHero;
    public EvolvePopup evolvePopup;
    public PopupCarUpgrade popupCar;
    public TMPro.TMP_Text goldNeedTut;
    public Image iconPrice;
    public Sprite[] spriteIcon;
    public TMPro.TMP_Text evolvePrice;
    public NewSkinUnlockPop unlockPop;

    private void OnEnable()
    {
        GameEvent.OnSelectSkin.RemoveListener(OnSelectSkin);
        GameEvent.OnSelectSkin.AddListener(OnSelectSkin);
        unlockSkin.onClick.RemoveListener(UnlockSkin);
        equipBtn.onClick.RemoveListener(OnEquipSkin);

        unlockSkin.onClick.AddListener(UnlockSkin);
        equipBtn.onClick.AddListener(OnEquipSkin);
        GameEvent.OnCarUnlockSlot.RemoveListener(SetUpIconHero);
        GameEvent.OnCarUnlockSlot.AddListener(SetUpIconHero);
        SetUpIconHero();
        PlayerBtn();
    }

    public void SetFocusFocus(int focusIndex)
    {
        foreach (GameObject obj in focus)
        {
            obj.SetActive(false);
        }
        focus[focusIndex].SetActive(true);
    }

    public void SetUpIconHero()
    {
        //for (int i = 0; i < User.Instance.Car.slot + 1; i++)
        //{
        //    iconHeroes[i].gameObject.SetActive(true);
        //}
    }

    //private void OnEnable()
    //{
    //    PlayerBtn();
    //}

    public void Open()
    {
        this.gameObject.SetActive(true);
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
        foreach (GameObject obj in focus)
        {
            obj.SetActive(false);
        }
    }

    public void PlayerBtn()
    {
        SetFocusFocus(0);
        ClearItem();
        foreach (UserBot bot in User.Instance.UserPlayers1)
        {
            ItemSkin itemSkin = ContentPoolable.Emit(ItemID.itemSkin) as ItemSkin;
            itemSkin.transform.parent = groupSkin.transform;
            itemSkin.transform.localScale = Vector3.one;
            itemSkin.SetUp(bot);
            if (itemSkin.bot.isUsing)
            {
                itemSkin.Select();
            }
        }
    }

    public void Bot1Btn()
    {
        SetFocusFocus(1);
        ClearItem();
        foreach (UserBot bot in User.Instance.UserBots1)
        {
            ItemSkin itemSkin = ContentPoolable.Emit(ItemID.itemSkin) as ItemSkin;
            itemSkin.transform.parent = groupSkin.transform;
            itemSkin.transform.localScale = Vector3.one;
            itemSkin.SetUp(bot);
            if (itemSkin.bot.isUsing)
            {
                itemSkin.Select();
            }
        }

        if(User.Instance[ItemID.TutUpgradeCar] < 2)
        {
            popupCar.carUI.bot1.transform.parent = popupCar.parentOrigin;
            popupCar.tutSelectHero.SetActive(false);
            popupCar.carUI.tutSelectBot1.SetActive(false);
            popupCar.tutUpHeroUp.SetActive(true);
        }
    }

    public void Bot2Btn()
    {
        SetFocusFocus(2);
        ClearItem();
        foreach (UserBot bot in User.Instance.UserBots2)
        {
            ItemSkin itemSkin = ContentPoolable.Emit(ItemID.itemSkin) as ItemSkin;
            itemSkin.transform.parent = groupSkin.transform;
            itemSkin.transform.localScale = Vector3.one;
            itemSkin.SetUp(bot);
            if (itemSkin.bot.isUsing)
            {
                itemSkin.Select();
            }
        }
    }

    public void Bot3Btn()
    {
        SetFocusFocus(3);
        ClearItem();
        foreach (UserBot bot in User.Instance.UserBots3)
        {
            ItemSkin itemSkin = ContentPoolable.Emit(ItemID.itemSkin) as ItemSkin;
            itemSkin.transform.parent = groupSkin.transform;
            itemSkin.transform.localScale = Vector3.one;
            itemSkin.SetUp(bot);
            if (itemSkin.bot.isUsing)
            {
                itemSkin.Select();
            }
        }
    }

    public void ClearItem()
    {
        foreach (Transform item in groupSkin)
        {
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }
    }

    private UserBot userBotSelected;

    public void OnSelectSkin(UserBot bot)
    {
        userBotSelected = bot;

        nameHero.text = userBotSelected.name;
        price.text = ((int)userBotSelected.price).ToKMB();
        goldNeedToUp.text = ((int)userBotSelected.goldNeedToUp).ToKMB();
        goldNeedTut.text= ((int)userBotSelected.goldNeedToUp).ToKMB();

        if (userBotSelected.isUnlock)
        {
            unlockSkin.gameObject.SetActive(false);
            if (userBotSelected.isUsing)                  //da mo khoa nhung chua trang bi
            {
                equipBtn.gameObject.SetActive(false);
                equipedBtn.gameObject.SetActive(false);
            }
            else
            {
                equipBtn.gameObject.SetActive(false);
                equipedBtn.gameObject.SetActive(false);
            }

            levelUp.gameObject.SetActive(true);

            //kiem tra xem level max chua
            if (userBotSelected.level == userBotSelected.levelMax1 && userBotSelected.mucDotPha == 0)
            {
                //max 1,dung lai cho mo khoa dot pha 1
                //lockButtonLvUp.gameObject.SetActive(true);
                levelUp.gameObject.SetActive(false);
            }
            else if (userBotSelected.level == userBotSelected.levelMax2 && userBotSelected.mucDotPha == 1)
            {
                //max 2,dung lai cho mo khoa dot pha 2
               // lockButtonLvUp.gameObject.SetActive(true);
                levelUp.gameObject.SetActive(false);
            }
            else if (userBotSelected.level == userBotSelected.levelMax3)
            {
                //max 3,dung lai
                lockButtonLvUp.gameObject.SetActive(false);
                levelUp.gameObject.SetActive(false);
            }
            else
            {
                //lockButtonLvUp.gameObject.SetActive(false);
                levelUp.gameObject.SetActive(true);
            }

            if(userBotSelected.mucDotPha < 2)
            {
                lockButtonLvUp.gameObject.SetActive(true);
                evolvePrice.text =User.Instance[ItemID.itemDotPha].ToString() + "/" + (userBotSelected.itemCanDeDotPha * (userBotSelected.mucDotPha + 1)).ToString();
            }
            else
            {
                lockButtonLvUp.gameObject.SetActive(false);
            }

            OnEquipSkin();
        }
        else                                                //chua mo khoa
        {
            equipedBtn.gameObject.SetActive(false);
            unlockSkin.gameObject.SetActive(true);
            equipBtn.gameObject.SetActive(false);
            levelUp.gameObject.SetActive(false);
            lockButtonLvUp.gameObject.SetActive(false);

            //checking price
            if(bot.priceType == PriceType.Coin)
            {
                iconPrice.sprite = spriteIcon[0];
            }
            else
            {
                iconPrice.sprite = spriteIcon[1];
                price.text =bot.adsUnlock.ToString() + "/" + userBotSelected.price.ToString();
            }
        }
    }

    public void LevelUpCharacter()
    {
        //gold check
        if (User.Instance[ItemID.Gold] < userBotSelected.goldNeedToUp)
        {
            //open shop
            PopupManager.Instance.OpenPopup<PopupShop>(PopupID.PopupShop);
            return;
        }

        if (User.Instance[ItemID.TutUpgradeCar] < 2)
        {
            popupCar.tutUpHeroUp.SetActive(false);
            User.Instance[ItemID.TutUpgradeCar] = 2;
            GameScene.main.homePanel.tutUpgradeCar.SetActive(false);
        }

        User.Instance[ItemID.Gold] -= userBotSelected.goldNeedToUp;
        userBotSelected.level += 1;
        switch (userBotSelected.type)
        {
            case TypeBot.Player:
                foreach (UserBot bot in User.Instance.UserPlayers1)
                {
                    if (bot.id == userBotSelected.id)
                    {
                        bot.level = userBotSelected.level;
                        bot.damage += 3f;
                        bot.crit += 0.5f;
                        bot.goldNeedToUp += 150;
                        GameEvent.OnLevelUpChar.Invoke(bot);
                        GameEvent.OnSelectSkin.Invoke(bot);
                    }
                }
                break;
            case TypeBot.Pistol:
                foreach (UserBot bot in User.Instance.UserBots1)
                {
                    if (bot.id == userBotSelected.id)
                    {
                        bot.level = userBotSelected.level;
                        bot.damage += 3f;
                        bot.crit += 0.5f;
                        bot.goldNeedToUp += 150;
                        GameEvent.OnLevelUpChar.Invoke(bot);
                        GameEvent.OnSelectSkin.Invoke(bot);
                    }
                }
                break;
            case TypeBot.Riffle:
                foreach (UserBot bot in User.Instance.UserBots2)
                {
                    if (bot.id == userBotSelected.id)
                    {
                        bot.level = userBotSelected.level;
                        bot.damage += 3f;
                        bot.crit += 0.5f;
                        bot.goldNeedToUp += 150;
                        GameEvent.OnLevelUpChar.Invoke(bot);
                        GameEvent.OnSelectSkin.Invoke(bot);
                    }
                }
                break;
            case TypeBot.Bazoka:
                foreach (UserBot bot in User.Instance.UserBots3)
                {
                    if (bot.id == userBotSelected.id)
                    {
                        bot.level = userBotSelected.level;
                        bot.damage += 3f;
                        bot.crit += 0.5f;
                        bot.goldNeedToUp += 150;
                        GameEvent.OnLevelUpChar.Invoke(bot);
                        GameEvent.OnSelectSkin.Invoke(bot);
                    }
                }
                break;
        }
        AudioManager.instance.Play("levelUp");
        User.Instance.Save();
    }


    public void DotPhaCharacter()
    {
        evolvePopup.SetUp(userBotSelected);
        AudioManager.instance.Play("BtnClick");
       // AudioManager.instance.Play("Dotpha");
        //item dot pha check
        //if (User.Instance[ItemID.itemDotPha] < userBotSelected.itemCanDeDotPha * userBotSelected.mucDotPha)
        //{
        //    //open shop
        //    PopupManager.Instance.OpenPopup<PopupShop>(PopupID.PopupShop);
        //    return;
        //}


        //userBotSelected.mucDotPha += 1;
        //switch (userBotSelected.type)
        //{
        //    case TypeBot.Player:
        //        foreach (UserBot bot in User.Instance.UserPlayers1)
        //        {
        //            if (bot.id == userBotSelected.id)
        //            {
        //                bot.mucDotPha = userBotSelected.mucDotPha;
        //                bot.damage += 10 * bot.mucDotPha;
        //                bot.crit += 4 * bot.mucDotPha;
        //                GameEvent.OnSelectSkin.Invoke(bot);
        //            }
        //        }
        //        break;
        //    case TypeBot.Pistol:
        //        foreach (UserBot bot in User.Instance.UserBots1)
        //        {
        //            if (bot.id == userBotSelected.id)
        //            {
        //                bot.mucDotPha = userBotSelected.mucDotPha;
        //                bot.damage += 5 * bot.mucDotPha;
        //                bot.crit += 3 * bot.mucDotPha;
        //                GameEvent.OnSelectSkin.Invoke(bot);
        //            }
        //        }
        //        break;
        //    case TypeBot.Riffle:
        //        foreach (UserBot bot in User.Instance.UserBots2)
        //        {
        //            if (bot.id == userBotSelected.id)
        //            {
        //                bot.mucDotPha = userBotSelected.mucDotPha;
        //                bot.damage += 5 * bot.mucDotPha;
        //                bot.crit += 3 * bot.mucDotPha;
        //                GameEvent.OnSelectSkin.Invoke(bot);
        //            }
        //        }
        //        break;
        //    case TypeBot.Bazoka:
        //        foreach (UserBot bot in User.Instance.UserBots3)
        //        {
        //            if (bot.id == userBotSelected.id)
        //            {
        //                bot.mucDotPha = userBotSelected.mucDotPha;
        //                bot.damage += 5 * bot.mucDotPha;
        //                bot.crit += 3 * bot.mucDotPha;
        //                GameEvent.OnSelectSkin.Invoke(bot);
        //            }
        //        }
        //        break;
        //}

        //User.Instance.Save();
    }


    public void UnlockSkin()
    {
        if(userBotSelected.priceType == PriceType.Coin)
        {
            if (User.Instance[ItemID.Gold] < userBotSelected.price)
            {
                //open shop
                PopupManager.Instance.OpenPopup<PopupShop>(PopupID.PopupShop);
                return;
            }
        }
        else
        {
            if (userBotSelected.adsUnlock < userBotSelected.price)
            {
                //xem ads sau do + 1 ads da xem
                userBotSelected.adsUnlock += 1;
                switch (userBotSelected.type)
                {
                    case TypeBot.Player:
                        foreach (UserBot bot in User.Instance.UserPlayers1)
                        {
                            if (bot.id == userBotSelected.id)
                            {
                                bot.adsUnlock = userBotSelected.adsUnlock;
                                GameEvent.OnSelectSkin.Invoke(bot);
                            }
                        }
                        break;
                    case TypeBot.Pistol:
                        foreach (UserBot bot in User.Instance.UserBots1)
                        {
                            if (bot.id == userBotSelected.id)
                            {
                                bot.adsUnlock = userBotSelected.adsUnlock;
                                GameEvent.OnSelectSkin.Invoke(bot);
                            }
                        }
                        break;
                    case TypeBot.Riffle:
                        foreach (UserBot bot in User.Instance.UserBots2)
                        {
                            if (bot.id == userBotSelected.id)
                            {
                                bot.adsUnlock = userBotSelected.adsUnlock;
                                GameEvent.OnSelectSkin.Invoke(bot);
                            }
                        }
                        break;
                    case TypeBot.Bazoka:
                        foreach (UserBot bot in User.Instance.UserBots3)
                        {
                            if (bot.id == userBotSelected.id)
                            {
                                bot.adsUnlock = userBotSelected.adsUnlock;
                                GameEvent.OnSelectSkin.Invoke(bot);
                            }
                        }
                        break;
                }

                if(userBotSelected.adsUnlock < userBotSelected.price)
                {
                    return;
                }
            }
        }



        switch (userBotSelected.type)
        {
            case TypeBot.Player:
                foreach (UserBot bot in User.Instance.UserPlayers1)
                {
                    if (bot.id == userBotSelected.id)
                    {
                        foreach (UserBot bot1 in User.Instance.UserPlayers1)
                        {
                            bot1.isUsing = false;
                        }
                        bot.isUnlock = true;
                        bot.isUsing = true;
                        User.Instance.UserPlayerUsing = bot;
                        if (userBotSelected.priceType == PriceType.Coin)
                        {
                            User.Instance[ItemID.Gold] -= (int)userBotSelected.price;
                        }
                        GameEvent.OnSelectSkin.Invoke(bot);
                        GameEvent.OnEquipSkin.Invoke(bot);
                    }
                }
                break;
            case TypeBot.Pistol:
                foreach (UserBot bot in User.Instance.UserBots1)
                {
                    if (bot.id == userBotSelected.id)
                    {
                        foreach (UserBot bot1 in User.Instance.UserBots1)
                        {
                            bot1.isUsing = false;
                        }
                        bot.isUnlock = true;
                        bot.isUsing = true;
                        User.Instance.UserBot1Using = bot;
                        if (userBotSelected.priceType == PriceType.Coin)
                        {
                            User.Instance[ItemID.Gold] -= (int)userBotSelected.price;
                        }
                        GameEvent.OnSelectSkin.Invoke(bot);
                        GameEvent.OnEquipSkin.Invoke(bot);
                    }
                }
                break;
            case TypeBot.Riffle:
                foreach (UserBot bot in User.Instance.UserBots2)
                {
                    if (bot.id == userBotSelected.id)
                    {
                        foreach (UserBot bot1 in User.Instance.UserBots2)
                        {
                            bot1.isUsing = false;
                        }
                        bot.isUnlock = true;
                        bot.isUsing = true;
                        User.Instance.UserBot2Using = bot;
                        if (userBotSelected.priceType == PriceType.Coin)
                        {
                            User.Instance[ItemID.Gold] -= (int)userBotSelected.price;
                        }
                        GameEvent.OnSelectSkin.Invoke(bot);
                        GameEvent.OnEquipSkin.Invoke(bot);
                    }
                }
                break;
            case TypeBot.Bazoka:
                foreach (UserBot bot in User.Instance.UserBots3)
                {
                    if (bot.id == userBotSelected.id)
                    {
                        foreach (UserBot bot1 in User.Instance.UserBots3)
                        {
                            bot1.isUsing = false;
                        }
                        bot.isUnlock = true;
                        bot.isUsing = true;
                        User.Instance.UserBot3Using = bot;
                        if (userBotSelected.priceType == PriceType.Coin)
                        {
                            User.Instance[ItemID.Gold] -= (int)userBotSelected.price;
                        }
                        GameEvent.OnSelectSkin.Invoke(bot);
                        GameEvent.OnEquipSkin.Invoke(bot);
                    }
                }
                break;
        }
        unlockPop.gameObject.SetActive(true);
        unlockPop.SetUp(userBotSelected);
       AudioManager.instance.Play("levelUp");
    }

    public void OnEquipSkin()
    {
        switch (userBotSelected.type)
        {
            case TypeBot.Player:
                foreach (UserBot bot in User.Instance.UserPlayers1)
                {
                    if (bot.id == userBotSelected.id)
                    {
                        foreach (UserBot bot1 in User.Instance.UserPlayers1)
                        {
                            bot1.isUsing = false;
                        }
                        bot.isUsing = true;
                        User.Instance.UserPlayerUsing = bot;
                        GameEvent.OnEquipSkin.Invoke(bot);
                        //GameEvent.OnSelectSkin.Invoke(bot);
                    }
                }
                break;
            case TypeBot.Pistol:
                foreach (UserBot bot in User.Instance.UserBots1)
                {
                    if (bot.id == userBotSelected.id)
                    {
                        foreach (UserBot bot1 in User.Instance.UserBots1)
                        {
                            bot1.isUsing = false;
                        }
                        bot.isUsing = true;
                        User.Instance.UserBot1Using = bot;
                        GameEvent.OnEquipSkin.Invoke(bot);
                       // GameEvent.OnSelectSkin.Invoke(bot);
                    }
                }
                break;
            case TypeBot.Riffle:
                foreach (UserBot bot in User.Instance.UserBots2)
                {
                    if (bot.id == userBotSelected.id)
                    {
                        foreach (UserBot bot1 in User.Instance.UserBots2)
                        {
                            bot1.isUsing = false;
                        }
                        bot.isUsing = true;
                        User.Instance.UserBot2Using = bot;
                        GameEvent.OnEquipSkin.Invoke(bot);
                        //GameEvent.OnSelectSkin.Invoke(bot);
                    }
                }
                break;
            case TypeBot.Bazoka:
                foreach (UserBot bot in User.Instance.UserBots3)
                {
                    if (bot.id == userBotSelected.id)
                    {
                        foreach (UserBot bot1 in User.Instance.UserBots3)
                        {
                            bot1.isUsing = false;
                        }
                        bot.isUsing = true;
                        User.Instance.UserBot3Using = bot;
                        GameEvent.OnEquipSkin.Invoke(bot);
                        //GameEvent.OnSelectSkin.Invoke(bot);
                    }
                }
                break;
        }
    }
}
