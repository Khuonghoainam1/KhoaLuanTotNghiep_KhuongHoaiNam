using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunInventory", menuName = "GunInventory/GunData", order = 1)]
[System.Serializable]
public class GunInventory : ScriptableObject
{
    public ItemID gunID;
    public GunName gunName;
    public bool isUnlock;
    public ItemValue itemcell;
    public List<PieceNeed> pieceNeed;
}

[System.Serializable]
public class PieceNeed
{
    public ItemID IDPiece;
    public int amount;
}


public enum GunName
{
    AR_36,
    AR_4A1,
    AR_74,
    AR_80,
    AR_44,
    AR_1,



    SUNGPHU_1=500,
    SUNGPHU_2,
    SUNGPHU_3,
    SUNGPHU_4,
    SUNGPHU_5,
    SUNGPHU_6,
}