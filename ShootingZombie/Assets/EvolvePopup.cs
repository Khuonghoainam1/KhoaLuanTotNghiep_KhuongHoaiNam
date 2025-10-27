using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class EvolvePopup : MonoBehaviour
{
    public EvolveGroupChar groupCurrentChar;
    public EvolveGroupChar groupNewChar;
    public EvolveGroupChar groupCharMax;

    public Button evolveByGoldBtn;
    public Button evolveByAdsBtn;
    public Button maxBtn;
    public GameObject arrow;

    public TMP_Text itemDotPha;
    public Slider itemDotPhaSlider;
    public TMP_Text priceGold;

    public TMP_Text amountAds;
    public Slider sliderAds;

    public GameObject sliderPrice;

    public UserBot userBot;
    public Image iconEvolve;

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    public void SetUp(UserBot userBot)
    {
        this.gameObject.SetActive(true);
        this.userBot = userBot;

        //kiem tra xem da max evolve chua
        if(this.userBot.mucDotPha == 2)
        {
            groupCurrentChar.gameObject.SetActive(false);
            groupNewChar.gameObject.SetActive(false);

            groupCharMax.gameObject.SetActive(true);
            groupCharMax.SetUp(this.userBot);
            evolveByGoldBtn.gameObject.SetActive(false);
            evolveByAdsBtn.gameObject.SetActive(false);
            maxBtn.gameObject.SetActive(true);
            arrow.gameObject.SetActive(false);
            sliderPrice.SetActive(false);
            return;
        }

        arrow.gameObject.SetActive(true);
        groupCharMax.gameObject.SetActive(false);

        groupCurrentChar.gameObject.SetActive(true);
        groupNewChar.gameObject.SetActive(true);
        groupCurrentChar.SetUp(this.userBot);
        groupNewChar.SetUp(this.userBot);
        evolveByGoldBtn.gameObject.SetActive(true);
        evolveByAdsBtn.gameObject.SetActive(true);
        maxBtn.gameObject.SetActive(false);
        sliderPrice.SetActive(true);

        //price
        itemDotPhaSlider.maxValue = this.userBot.itemCanDeDotPha + this.userBot.itemCanDeDotPha * this.userBot.mucDotPha;
        itemDotPhaSlider.value = User.Instance[ItemID.itemDotPha];
        itemDotPha.text =User.Instance[ItemID.itemDotPha].ToString() + "/" + (this.userBot.itemCanDeDotPha + this.userBot.itemCanDeDotPha * this.userBot.mucDotPha).ToString();
        priceGold.text = (this.userBot.goldDotPha + this.userBot.goldDotPha * this.userBot.mucDotPha).ToKMB();

        //ads
        amountAds.text = this.userBot.adsDotPhaDaxem.ToString() + "/" + this.userBot.adsDotPha.ToString();
        sliderAds.maxValue = this.userBot.adsDotPha;
        sliderAds.value = this.userBot.adsDotPhaDaxem;
    }

    public void DotPhaBangAds()
    {
        if(this.userBot.adsDotPhaDaxem < this.userBot.adsDotPha)  //chua du ads
        {
            this.userBot.adsDotPhaDaxem += 1;
            switch (userBot.type)
            {
                case TypeBot.Player:
                    foreach (UserBot bot in User.Instance.UserPlayers1)
                    {
                        if (bot.id == userBot.id)
                        {
                            bot.adsDotPhaDaxem = userBot.adsDotPhaDaxem;
                            GameEvent.OnSelectSkin.Invoke(bot);
                            SetUp(bot);
                        }
                    }
                    break;
                case TypeBot.Pistol:
                    foreach (UserBot bot in User.Instance.UserBots1)
                    {
                        if (bot.id == userBot.id)
                        {
                            bot.adsDotPhaDaxem = userBot.adsDotPhaDaxem;
                            GameEvent.OnSelectSkin.Invoke(bot);
                            SetUp(bot);
                        }
                    }
                    break;
                case TypeBot.Riffle:
                    foreach (UserBot bot in User.Instance.UserBots2)
                    {
                        if (bot.id == userBot.id)
                        {
                            bot.adsDotPhaDaxem = userBot.adsDotPhaDaxem;
                            GameEvent.OnSelectSkin.Invoke(bot);
                            SetUp(bot);
                        }
                    }
                    break;
                case TypeBot.Bazoka:
                    foreach (UserBot bot in User.Instance.UserBots3)
                    {
                        if (bot.id == userBot.id)
                        {
                            bot.adsDotPhaDaxem = userBot.adsDotPhaDaxem;
                            GameEvent.OnSelectSkin.Invoke(bot);
                            SetUp(bot);
                        }
                    }
                    break;
            }
            AudioManager.instance.Play("levelUp");
        }
        else
        {
            this.userBot.adsDotPhaDaxem = 0;
            userBot.mucDotPha += 1;
            userBot.star += 1;
            switch (userBot.type)
            {
                case TypeBot.Player:
                    foreach (UserBot bot in User.Instance.UserPlayers1)
                    {
                        if (bot.id == userBot.id)
                        {
                            bot.mucDotPha = userBot.mucDotPha;
                            bot.star = userBot.star;
                            if (userBot.mucDotPha == 1)
                            {
                                bot.damage += bot.damageDotPha1;
                                bot.crit += bot.critDotPha1;
                            }
                            else if (userBot.mucDotPha == 2)
                            {
                                bot.damage += bot.damageDotPha2;
                                bot.crit += bot.critDotPha2;
                            }
                            GameEvent.OnSelectSkin.Invoke(bot);
                            SetUp(bot);
                        }
                    }
                    break;
                case TypeBot.Pistol:
                    foreach (UserBot bot in User.Instance.UserBots1)
                    {
                        if (bot.id == userBot.id)
                        {
                            bot.mucDotPha = userBot.mucDotPha;
                            bot.star = userBot.star;
                            if (userBot.mucDotPha == 1)
                            {
                                bot.damage += bot.damageDotPha1;
                                bot.crit += bot.critDotPha1;
                            }
                            else if (userBot.mucDotPha == 2)
                            {
                                bot.damage += bot.damageDotPha2;
                                bot.crit += bot.critDotPha2;
                            }
                            GameEvent.OnSelectSkin.Invoke(bot);
                            SetUp(bot);
                        }
                    }
                    break;
                case TypeBot.Riffle:
                    foreach (UserBot bot in User.Instance.UserBots2)
                    {
                        if (bot.id == userBot.id)
                        {
                            bot.mucDotPha = userBot.mucDotPha;
                            bot.star = userBot.star;
                            if (userBot.mucDotPha == 1)
                            {
                                bot.damage += bot.damageDotPha1;
                                bot.crit += bot.critDotPha1;
                            }
                            else if (userBot.mucDotPha == 2)
                            {
                                bot.damage += bot.damageDotPha2;
                                bot.crit += bot.critDotPha2;
                            }
                            GameEvent.OnSelectSkin.Invoke(bot);
                            SetUp(bot);
                        }
                    }
                    break;
                case TypeBot.Bazoka:
                    foreach (UserBot bot in User.Instance.UserBots3)
                    {
                        if (bot.id == userBot.id)
                        {
                            bot.mucDotPha = userBot.mucDotPha;
                            bot.star = userBot.star;
                            if (userBot.mucDotPha == 1)
                            {
                                bot.damage += bot.damageDotPha1;
                                bot.crit += bot.critDotPha1;
                            }
                            else if (userBot.mucDotPha == 2)
                            {
                                bot.damage += bot.damageDotPha2;
                                bot.crit += bot.critDotPha2;
                            }
                            GameEvent.OnSelectSkin.Invoke(bot);
                            SetUp(bot);
                        }
                    }
                    break;
            }
            AudioManager.instance.Play("levelUp2");
        }
      

    }

    public void DotPhaCharacter()
    {
        //item dot pha check
        if (User.Instance[ItemID.Gold] < (this.userBot.goldDotPha + this.userBot.goldDotPha * this.userBot.mucDotPha))
        {
            //open shop
            PopupManager.Instance.OpenPopup<PopupShop>(PopupID.PopupShop);
            return;
        }
        else if (User.Instance[ItemID.itemDotPha] < (this.userBot.itemCanDeDotPha + this.userBot.itemCanDeDotPha * this.userBot.mucDotPha))
        {
            iconEvolve.color = Color.red;
            iconEvolve.DOColor(Color.white, 0.5f).OnComplete(() => { iconEvolve.color = Color.red; iconEvolve.DOColor(Color.white, 0.5f); });

            return;
        }

        User.Instance[ItemID.itemDotPha] -= (this.userBot.itemCanDeDotPha + this.userBot.itemCanDeDotPha * this.userBot.mucDotPha);
        User.Instance[ItemID.Gold] -= (this.userBot.goldDotPha + this.userBot.goldDotPha * this.userBot.mucDotPha);
        userBot.mucDotPha += 1;
        userBot.star += 1;
        switch (userBot.type)
        {
            case TypeBot.Player:
                foreach (UserBot bot in User.Instance.UserPlayers1)
                {
                    if (bot.id == userBot.id)
                    {
                        bot.mucDotPha = userBot.mucDotPha;
                        bot.star = userBot.star;
                        if (userBot.mucDotPha == 1)
                        {
                            bot.damage += bot.damageDotPha1;
                            bot.crit += bot.critDotPha1;
                        }
                        else if(userBot.mucDotPha == 2)
                        {
                            bot.damage += bot.damageDotPha2;
                            bot.crit += bot.critDotPha2;
                        }
                        GameEvent.OnSelectSkin.Invoke(bot);
                        SetUp(bot);
                    }
                }
                break;
            case TypeBot.Pistol:
                foreach (UserBot bot in User.Instance.UserBots1)
                {
                    if (bot.id == userBot.id)
                    {
                        bot.mucDotPha = userBot.mucDotPha;
                        bot.star = userBot.star;
                        if (userBot.mucDotPha == 1)
                        {
                            bot.damage += bot.damageDotPha1;
                            bot.crit += bot.critDotPha1;
                        }
                        else if (userBot.mucDotPha == 2)
                        {
                            bot.damage += bot.damageDotPha2;
                            bot.crit += bot.critDotPha2;
                        }
                        GameEvent.OnSelectSkin.Invoke(bot);
                        SetUp(bot);
                    }
                }
                break;
            case TypeBot.Riffle:
                foreach (UserBot bot in User.Instance.UserBots2)
                {
                    if (bot.id == userBot.id)
                    {
                        bot.mucDotPha = userBot.mucDotPha;
                        bot.star = userBot.star;
                        if (userBot.mucDotPha == 1)
                        {
                            bot.damage += bot.damageDotPha1;
                            bot.crit += bot.critDotPha1;
                        }
                        else if (userBot.mucDotPha == 2)
                        {
                            bot.damage += bot.damageDotPha2;
                            bot.crit += bot.critDotPha2;
                        }
                        GameEvent.OnSelectSkin.Invoke(bot);
                        SetUp(bot);
                    }
                }
                break;
            case TypeBot.Bazoka:
                foreach (UserBot bot in User.Instance.UserBots3)
                {
                    if (bot.id == userBot.id)
                    {
                        bot.mucDotPha = userBot.mucDotPha;
                        bot.star = userBot.star;
                        if (userBot.mucDotPha == 1)
                        {
                            bot.damage += bot.damageDotPha1;
                            bot.crit += bot.critDotPha1;
                        }
                        else if (userBot.mucDotPha == 2)
                        {
                            bot.damage += bot.damageDotPha2;
                            bot.crit += bot.critDotPha2;
                        }
                        GameEvent.OnSelectSkin.Invoke(bot);
                        SetUp(bot);
                    }
                }
                break;
        }
        AudioManager.instance.Play("levelUp");
        User.Instance.Save();
    }
}
