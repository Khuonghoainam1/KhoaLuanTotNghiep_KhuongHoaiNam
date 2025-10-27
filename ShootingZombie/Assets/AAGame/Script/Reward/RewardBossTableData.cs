using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RewardConfig", menuName = "Reward/BossMode", order = 1)]
public class RewardBossTableData : ScriptableObject
{
    public List<LevelBossReward> rewardConfig;
}

[System.Serializable]
public class LevelBossReward
{
    public int gold;
    public int ticket;
    public ItemID skin;
}
