using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "RewardEndless", menuName = "Reward/Endless", order = 1)]
public class RewardEndLess : ScriptableObject
{
    public List<WaveRewardEndless> waveRewardEndlesses = new List<WaveRewardEndless>();

    private void OnValidate()
    {
        for(int i = 0; i < 6; i++)
        {
            waveRewardEndlesses[i].manhDotPhaMin = i * 2;
            if(i > 0)
            {
                waveRewardEndlesses[i].manhDotPhaMax = waveRewardEndlesses[i].manhDotPhaMin + 2;
            }
            else
            {
                waveRewardEndlesses[i].manhDotPhaMax = 0;
            }
            waveRewardEndlesses[i].bua = i+2;
        }
    }
}

[System.Serializable]
public class WaveRewardEndless
{
    public int manhDotPhaMin;
    public int manhDotPhaMax;
    public int bua;
}
