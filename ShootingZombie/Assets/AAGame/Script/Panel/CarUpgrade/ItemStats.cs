using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class ItemStats : MonoBehaviour
{
    public Slider slider;
    public UserCar userCar;
    public Stats typeStats;
    public TMPro.TMP_Text maxValue;
    public CarLevelData carLevelData;

    private void OnEnable()
    {
        GameEvent.OnCarLevelUp.RemoveListener(SetUp);
        GameEvent.OnCarLevelUp.AddListener(SetUp);
    }

    public void Start()
    {
        SetUp();
    }

    public void SetUp()
    {
        userCar = User.Instance.Car;

        switch (typeStats)
        {
            case Stats.Damage:
                maxValue.text= Math.Round((double)userCar.damage, 1).ToString();
                slider.maxValue = carLevelData.maxDmage;
                slider.DOValue(userCar.damage,1f);
                break;
            case Stats.HP:
                maxValue.text = Math.Round((double)userCar.hp, 1).ToString();
                slider.maxValue = carLevelData.maxHP;
                slider.DOValue(userCar.hp, 1f);
                break;
            case Stats.Def:
                maxValue.text = Math.Round((double)userCar.def, 1).ToString();
                slider.maxValue = carLevelData.maxDef;
                slider.DOValue(userCar.def, 1f);
                break;
            case Stats.Crit:
                maxValue.text = Math.Round((double)userCar.crit, 1).ToString();
                slider.maxValue = carLevelData.maxCrit;
                slider.DOValue(userCar.crit, 1f);
                break;
            case Stats.Slot:
                maxValue.text = userCar.slot.ToString();
                slider.maxValue = carLevelData.maxSlot;
                slider.DOValue(userCar.slot, 1f);
                break;
            case Stats.Level:
                maxValue.text = (userCar.level +1 ).ToString();
                slider.maxValue = 30;
                slider.DOValue(userCar.level, 1f);
                break;
        }
    }
}


public enum Stats
{
    Level,
    Damage,
    HP,
    Def,
    Crit,
    Slot,
}
