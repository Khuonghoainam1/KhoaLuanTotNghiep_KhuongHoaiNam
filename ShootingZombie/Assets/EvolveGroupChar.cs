using UnityEngine;
using TMPro;
using Spine.Unity;
using UnityEngine.UI;

public class EvolveGroupChar : MonoBehaviour
{
    public bool isNewChar;
    public TMP_Text level;
    public TMP_Text damage;
    public TMP_Text crit;
    public TMP_Text textDamageUp;
    public TMP_Text textCritUp;
    public GameObject[] stars;
    public BotUI botUI;
    public TMP_Text nameHero;
    public bool isCharMax;
    //public SkeletonDataAsset[] skeletonDataAsset; // PLAYER----->-BOT1----->-BOT2----->-BOT3
    //public SkeletonGraphic skeletonGraphic;

    public Slider levelSlider;
    public Slider atkSlider;
    public Slider critSlider;

    public UserBot userBot;

    public void SetStar(int currentStar)
    {
        foreach(GameObject star in stars)
        {
            star.SetActive(false);
            for(int i =0;i< currentStar; i++)
            {
                stars[i].SetActive(true);
            }
        }
    }

    public void SetUp(UserBot userBot)
    {
        this.userBot = userBot;
   

        damage.text = userBot.damage.ToString();
        crit.text = userBot.crit.ToString();

        if(userBot.mucDotPha == 0)
        {
            level.text = "Level " + userBot.level + "/" + userBot.levelMax1; 
        }
        else if (userBot.mucDotPha == 1)
        {
            level.text = "Level " + userBot.level + "/" + userBot.levelMax2;
        }
        else if (userBot.mucDotPha == 2)
        {
            level.text = "Level " + userBot.level + "/" + userBot.levelMax3;
        }
        SetStar(userBot.star);
        botUI.SetSkin(this.userBot);

        if (isNewChar)
        {
            UserBot userBotNew = new UserBot(this.userBot);
            userBotNew.mucDotPha += 1;
            userBotNew.star += 1;
            SetStar(userBotNew.star);

            if (userBotNew.mucDotPha == 1)
            {
                userBotNew.damage += userBot.damageDotPha1;
                userBotNew.crit += userBot.critDotPha1;

                damage.text = userBotNew.damage.ToString();
                crit.text = userBotNew.crit.ToString();
                textDamageUp.text = "+" + userBot.damageDotPha1.ToString();
                textCritUp.text = "+" + userBot.critDotPha1.ToString();

                level.text = "Level " + userBot.level + "/" + userBot.levelMax2;
            }
            else if (userBotNew.mucDotPha == 2)
            {
                userBotNew.damage += userBot.damageDotPha2;
                userBotNew.crit += userBot.critDotPha2;

                damage.text = userBotNew.damage.ToString();
                crit.text = userBotNew.crit.ToString();
                textDamageUp.text = "+" + userBot.damageDotPha2.ToString();
                textCritUp.text = "+" + userBot.critDotPha2.ToString();

                level.text = "Level " + userBot.level + "/" + userBot.levelMax3;
            }
            else if (userBot.mucDotPha == 3)
            {
                level.text = "Level " + userBot.level + "/" + userBot.levelMax3;
            }

            botUI.SetSkin(userBotNew);
        }

        if(nameHero != null)
        {
            nameHero.text = userBot.name.ToUpper();
        }

        if (isCharMax)
        {
            levelSlider.maxValue = userBot.levelMax3;
            levelSlider.value = userBot.level;
            level.text = userBot.level.ToString() + "/" + userBot.levelMax3.ToString();

            atkSlider.maxValue = userBot.damageMax;
            atkSlider.maxValue = userBot.damage;
            damage.text = userBot.damage.ToString() + "/" + userBot.damageMax.ToString();

            critSlider.maxValue = userBot.critMax;
            critSlider.value = userBot.crit;
            crit.text = userBot.crit.ToString() + "/" + userBot.critMax.ToString();
        }
    }
}
