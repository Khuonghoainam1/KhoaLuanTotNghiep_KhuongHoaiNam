using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "RewardConfig", menuName = "Reward/Collection", order = 1)]
public class RewardCollection : ScriptableObject
{
    public List<LevelBossReward> rewardConfig;
}

[System.Serializable]
public class LevelBossCollection
{
    public int gold;
    public ItemID skin;
}

