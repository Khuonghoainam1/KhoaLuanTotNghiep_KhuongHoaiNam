using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "WaveSetup/WaveData", order = 1)]
public class WaveSetup : ScriptableObject
{
    public EnemyType enemyType;
    public int enemyAmount;
    public List<ItemID> enemyID = new List<ItemID>();
    public bool isCollectMode;

    private void OnValidate()
    {
        if (enemyID.Count < enemyAmount)
        {
            enemyID.Clear();
            for (int i = 0; i < enemyAmount; i++)
            {
                if (isCollectMode)
                {
                    enemyID.Add(ItemID.enemy_gold_1);
                }
                else
                {
                    enemyID.Add(ItemType.Enemies.GetRandom());
                }
            }
        }
    }
}


public enum EnemyType
{
    Enemy_1,
    Boss,
}
