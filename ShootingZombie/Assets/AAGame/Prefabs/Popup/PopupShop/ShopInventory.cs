using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[CreateAssetMenu(fileName = "ShopInventory", menuName = "ShopIventory/ShopData", order = 1)]
[System.Serializable]
public class ShopInventory : ScriptableObject
{
    public ItemValue itemcell;
    public Sprite spriteIcon;
    public double txtPr;
    

}
