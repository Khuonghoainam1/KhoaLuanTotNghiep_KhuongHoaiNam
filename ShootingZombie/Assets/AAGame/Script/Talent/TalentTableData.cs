using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "TalentData", menuName = "Talent/TalentTable", order = 1)]
public class TalentTableData : ScriptableObject
{
    public List<LevelTalentData> levelTalentDatas = new List<LevelTalentData>();
    private void OnValidate()
    {
        foreach(LevelTalentData levelTalentData in levelTalentDatas)
        {
            levelTalentData.OnValidate();
        }
    }
}


[System.Serializable]
public class LevelTalentData
{
    public int level;
    public List<Talent> talents = new List<Talent>();

    public void OnValidate()
    {
        for(int i = 0; i < 3; i++)
        {
            talents[i].gold = 500 * level;
        }
        talents[0].talentType = TalentType.Damage;
        talents[0].amountUp = 0.5f * level;

        talents[1].talentType = TalentType.HP;
        talents[1].amountUp = 20f + (2 * level);

        talents[2].talentType = TalentType.Healing;
        talents[2].amountUp = 1;
    }
}

[System.Serializable]
public class Talent
{
    public TalentType talentType;
    [HideInInspector]
    public int level;
    public float amountUp;
    public int gold;
    [HideInInspector]
    public int indexTalent;
    [Header("SPECIAL TALENT")]
    public string des;
}

public enum TalentType
{
    Damage,
    HP,
    Healing,


    ReRoll,
    ATK,
    Coin,

}
