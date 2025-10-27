using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Drop", menuName = "DropSetUp/DropData", order = 1)]
public class PercentDropSetup : ScriptableObject
{
    public List<PercentDrop> levelDropPercent = new List<PercentDrop>();
}

[System.Serializable]
public class PercentDrop
{
    public List<Percent> percents = new List<Percent>();
    public int goldDrop;
}


[System.Serializable]
public class Percent
{
    public ItemID IDManhSung;
    public int percent;
}
