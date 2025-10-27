using UnityEngine;

public class GlobalData : MonoBehaviour
{

    public bool isOpenUpgrade;
    public ItemID bossToFight;

    public int levelToPlay;
    public bool isAutoPlay;

    public static GameMode gameMode = GameMode.Home;








    public static GlobalData instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        
    }
}
