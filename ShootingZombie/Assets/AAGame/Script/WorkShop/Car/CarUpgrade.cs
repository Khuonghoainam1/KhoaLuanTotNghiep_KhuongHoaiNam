using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CarUpgrade", menuName = "CarUpgrade/CarData", order = 1)]
[System.Serializable]
public class CarUpgrade : ScriptableObject
{
    public List<CarData> listCarData;
}

[System.Serializable]
public class CarData
{
    public ItemID carID;
    public int level;
    public int hp;
    public int exp;
    public int coin;
}


//public enum GunName
//{
//    AR_36,
//    AR_4A1,
//    AR_74,
//    AR_80,
//    AR_44,
//    AR_1,
//}