using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemystatsData", menuName = "EnemyStatsData/EnemyStart", order = 1)]
public class EnemyStatsData : ScriptableObject
{
    public List<EnemyStats> enemyStatsNormal;

    private void OnValidate()
    {
        //enemyStatsNormal.Clear();
        //for (int a = 0; a < 30; a++)
        //{
        //    EnemyStats temp = new EnemyStats();
        //    enemyStatsNormal.Add(temp);
        //}


        for (int i = 1; i < enemyStatsNormal.Count; i++)
        {
            enemyStatsNormal[i].atk = (int)(enemyStatsNormal[i-1].atk * 1.06f);
            if (i < 5)
            {
                enemyStatsNormal[i].hp = (int)(enemyStatsNormal[i-1].hp * 1.15f);
            }
            else
            {
                enemyStatsNormal[i].hp = (int)(enemyStatsNormal[i-1].hp * 1.25f);
            }
        }
    }
}

[System.Serializable]
public class EnemyStats
{
    public float hp;
    public float atk;
}