using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

public class ItemStatsBot : MonoBehaviour
{
    public Slider slider;
    public TMP_Text value;
    public TypeStatsBot typeStats;
    private UserBot userBot;

    private void OnEnable()
    {
        GameEvent.OnSelectSkin.RemoveListener(OnSelectSkin);
        GameEvent.OnSelectSkin.AddListener(OnSelectSkin);

        GameEvent.OnLevelUpChar.RemoveListener(OnSelectSkin);
        GameEvent.OnLevelUpChar.AddListener(OnSelectSkin);


        OnSelectSkin(User.Instance.UserPlayerUsing);
    }

    public void OnSelectSkin(UserBot userBot)
    {
        this.userBot = userBot;

        switch (typeStats)
        {
            case TypeStatsBot.Damage:
                slider.maxValue = userBot.damageMax;
                value.text = Math.Round((double)userBot.damage, 1).ToString();

                if (userBot.isUnlock)
                {
                    slider.DOValue(userBot.damage, 0.5f);
                }
                else
                {
                    value.text = Math.Round((double)userBot.damageMax, 1).ToString();
                    slider.DOValue(userBot.damageMax, 0.5f);
                }
                break;
            case TypeStatsBot.Level:
                slider.maxValue = userBot.levelMax3;
                value.text = userBot.level.ToString();

                if (userBot.isUnlock)
                {
                    slider.DOValue(userBot.level, 0.5f);
                }
                else
                {
                    value.text = userBot.levelMax3.ToString();
                    slider.DOValue(userBot.levelMax3, 0.5f);
                }
                break;
            case TypeStatsBot.Crit:
                slider.maxValue = userBot.critMax;
                value.text = Math.Round((double)userBot.crit, 1).ToString();

                if (userBot.isUnlock)
                {
                    slider.DOValue(userBot.crit, 0.5f);
                }
                else
                {
                    value.text = Math.Round((double)userBot.critMax, 1).ToString();
                    slider.DOValue(userBot.critMax, 0.5f);
                }
                break;
        }
    }

    public enum TypeStatsBot
    {
        Level,
        Damage,
        Crit,
    }
}
