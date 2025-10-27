using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RewardConfig", menuName = "Reward/Normal", order = 1)]
public class RewardStageTableData : ScriptableObject
{
    public List<int> goldNormalMode = new List<int>();
    public List<int> dotPhaMin = new List<int>();
    public List<int> dotPhaMax = new List<int>();

    private void OnValidate()
    {
        for(int i = 0; i < 150; i++)
        {
            goldNormalMode.Add(0);
        }
        goldNormalMode[0] = 250;
        for(int i = 1;i < goldNormalMode.Count;i++)
        {
            goldNormalMode[i] = goldNormalMode[i - 1] + 150;
        }

        dotPhaMin[0] = 5;
        dotPhaMax[0] = 6;
        dotPhaMin[1] = 5;
        dotPhaMax[1] = 6;

        for(int i = 2; i <= 8; i++)
        {
            dotPhaMin[i] = 1;
            dotPhaMax[i] = 3;
        }

        dotPhaMin[9] = 3;
        dotPhaMax[9] = 6;

        for (int i = 10; i <= 18; i++)
        {
            dotPhaMin[i] = 2;
            dotPhaMax[i] = 5;
        }

        dotPhaMin[19] = 5;
        dotPhaMax[19] = 7;

        for (int i = 20; i <= 30; i++)
        {
            dotPhaMin[i] = 3;
            dotPhaMax[i] = 6;
        }
    }
}
