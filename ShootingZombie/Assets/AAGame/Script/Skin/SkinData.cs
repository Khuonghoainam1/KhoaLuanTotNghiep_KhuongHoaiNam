using UnityEngine;

[CreateAssetMenu(fileName = "Skin", menuName = "Skin/SkinData", order = 1)]
public class SkinData : ScriptableObject
{
    public ItemID skinID;
    public string skinName;
    public Sprite skinIcon;
}
