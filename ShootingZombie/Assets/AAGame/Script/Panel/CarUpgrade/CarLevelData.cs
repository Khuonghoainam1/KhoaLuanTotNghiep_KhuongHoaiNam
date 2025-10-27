using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CarLevelData", menuName = "CarData/CarData", order = 1)]
public class CarLevelData : ScriptableObject
{
    public float maxDmage;
    public float maxHP;
    public float maxDef;
    public float maxShield;
    public float maxSlot;
    public float maxCrit;
    public int levelSlot1;
    public int levelSlot2;
    public int levelSlot3;
    public List<UserCar> carLevelDatas = new List<UserCar>();

    private void OnValidate()
    {
        carLevelDatas[0].level = 0;
        carLevelDatas[0].exp = 100;
        carLevelDatas[0].slot = 0;
        carLevelDatas[0].skin = ItemID.car_5;
        carLevelDatas[0].gun = ItemID.gun_1;
        carLevelDatas[0].gold = 1000;
        carLevelDatas[0].damage = 30;
        carLevelDatas[0].hp = 500;
        carLevelDatas[0].crit = 5;
        carLevelDatas[0].def = 5;
        carLevelDatas[0].expNeedToNewSkin = 100;
        carLevelDatas[0].newSkin = ItemID.car_2;

        for (int i = 1; i < carLevelDatas.Count; i++)
        {
            carLevelDatas[i].level = carLevelDatas[i - 1].level + 1;
            carLevelDatas[i].exp = carLevelDatas[i - 1].exp * 1.2f;
            carLevelDatas[i].gold = carLevelDatas[i - 1].gold + 1500;
            carLevelDatas[i].damage = carLevelDatas[i - 1].damage * 1.15f;
            carLevelDatas[i].hp = carLevelDatas[i - 1].hp * 1.1f;
            carLevelDatas[i].crit = carLevelDatas[i - 1].crit * 1.01f;
            carLevelDatas[i].def = carLevelDatas[i - 1].def + 0.5f;

            //slot
            if (i >= 1 && i < 5)
            {
                carLevelDatas[i].slot = 1;
                carLevelDatas[i].skin = ItemID.car_6;
                carLevelDatas[i].gun = ItemID.gun_6;
                carLevelDatas[i].expNeedToNewSkin = 400;
                carLevelDatas[i].newSkin = ItemID.car_3;
            }
            else if (i >= 5 && i < 10)
            {
                carLevelDatas[i].slot = 2;
                carLevelDatas[i].skin = ItemID.car_3;
                carLevelDatas[i].gun = ItemID.gun_3;
                carLevelDatas[i].expNeedToNewSkin = 500;
                carLevelDatas[i].newSkin = ItemID.car_4;
            }
            else if (i >= 10)
            {
                carLevelDatas[i].slot = 3;
            }

            if(i >= 10 && i < 15)
            {
                carLevelDatas[i].skin = ItemID.car_4;
                carLevelDatas[i].expNeedToNewSkin = 500;
                carLevelDatas[i].newSkin = ItemID.car_1;
                carLevelDatas[i].gun = ItemID.gun_4;
            }
            else if (i >= 15 && i < 20)
            {
                carLevelDatas[i].skin = ItemID.car_1;
                carLevelDatas[i].expNeedToNewSkin = 500;
                carLevelDatas[i].newSkin = ItemID.car_2;
                carLevelDatas[i].gun = ItemID.gun_5;

            }
            else if (i >= 20 )
            {
                carLevelDatas[i].skin = ItemID.car_2;
                carLevelDatas[i].expNeedToNewSkin = 500;
                carLevelDatas[i].newSkin = ItemID.car_2;
                carLevelDatas[i].gun = ItemID.gun_2;
            }
        }

        maxDmage = carLevelDatas[carLevelDatas.Count - 1].damage;
        maxHP = carLevelDatas[carLevelDatas.Count - 1].hp;
        maxCrit = carLevelDatas[carLevelDatas.Count - 1].crit;
        maxDef = carLevelDatas[carLevelDatas.Count - 1].def;
    }
}
