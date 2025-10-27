using UnityEngine;

public class LevelConfig : MonoBehaviour
{
    public LevelSetUp[] levelSetUps;
    public LevelBossSetUp[] levelBossSetUps;
    public LevelSetUp[] levelSetUpsCollect;

    public static LevelConfig instance;

    private void Awake()
    {
        instance = this;
    }
}
