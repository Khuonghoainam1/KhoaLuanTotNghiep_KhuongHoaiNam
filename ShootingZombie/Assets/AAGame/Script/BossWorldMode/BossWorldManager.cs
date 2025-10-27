using AA_Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yurowm.GameCore;

public class BossWorldManager : ManagerBase
{
    //public GameState gameState;
    //public CameraFollow cam;
    //public CameraFollow UIcam;
    //[HideInInspector]
    //public TrainManager trainManager;
    //public List<BossWorldBase> listBoss = new List<BossWorldBase>();
    //public int enemiesCurrentAmount;


    //public static BossWorldManager instance;

    //private void Awake()
    //{
    //    instance = this;
    //}

    //private void OnDestroy()
    //{
    //    GameEvent.OnPlayerWin.RemoveListener(Win);
    //    GameEvent.OnPlayerLose.RemoveListener(PlayerDie);
    //    GameEvent.OnShowRevive.RemoveListener(OnShowPopupRevive);
    //    GameEvent.OnRevive.RemoveListener(OnRevive);
    //    GameEvent.OnCloseRevive.RemoveListener(OnCloseRevive);
    //}

    //private void Start()
    //{
    //    GameEvent.OnPlayerWin.AddListener(Win);
    //    GameEvent.OnPlayerLose.AddListener(PlayerDie);
    //    GameEvent.OnShowRevive.AddListener(OnShowPopupRevive);
    //    GameEvent.OnRevive.AddListener(OnRevive);
    //    GameEvent.OnCloseRevive.AddListener(OnCloseRevive);
    //    listBoss.Clear();
    //    SpawnCar();
    //    trainManager.SpawnBot();
    //    trainManager.SpawnPlayer();
    //    trainManager.ChangeState(new ReadyFightingBossState(trainManager));
    //    SpawnBoss();
    //    CarStart();
    //    ManagerBase.gameState = GameState.Playing;
    //}

    //public void OnEnemyDie()
    //{
    //    enemiesCurrentAmount -= 1;
    //    if (enemiesCurrentAmount <= 0)
    //    {
    //        GameEvent.OnPlayerWin.Invoke();
    //    }
    //}

    //public void PlayerDie()
    //{
    //    gameState = GameState.Lose;
    //    ManagerBase.gameState = GameState.Lose;
    //    PopupManager.Instance.OpenPopup<RevivePopup>(PopupID.PopupRevive);
    //}

    //public void Win()
    //{
    //    ManagerBase.gameState = GameState.Win;
    //    PopupManager.Instance.OpenPopup<PopUpGameWin>(PopupID.PopupGameWin, (pop) => pop.SetData(true, 100, 3));
    //}

    //public void Lose()
    //{
    //    ManagerBase.gameState = GameState.Lose;
    //    PopupManager.Instance.OpenPopup<PopUpGameDefect>(PopupID.PopupGameDefeact, (pop) => pop.SetData(false, 100, 1));
    //}

    //public void Revive()
    //{
    //    StartCoroutine(DoRevive());
    //}

    //IEnumerator DoRevive()
    //{
    //    gameState = GameState.Playing;
    //    ManagerBase.gameState = GameState.Playing;
    //    trainManager.ReviveBossWorld();
    //    yield return new WaitForSeconds(2f);
    //    GameEvent.OnReviveGame.Invoke();
    //    PopupManager.Instance.CloseCurrentPopup();
    //}

    //public void OnShowPopupRevive()
    //{

    //}
    //public void OnRevive()
    //{
    //    Revive();
    //}
    //public void OnCloseRevive()
    //{
    //    Lose();
    //}


    //public void SpawnMap(Vector3 position)
    //{
    //    Item bgMapBoss = ContentPoolable.Emit(ItemType.Map_Boss[0]) as Item;
    //}

    //public void SpawnCar()
    //{
    //    Vector3 pos = new Vector3(0, -8, 0);
    //    GameObject train = Instantiate(Resources.Load<GameObject>("Car/" + /*User.Instance.Car.carID.ToString()*/ ItemID.car_1.ToString()), pos, transform.rotation);
    //    trainManager = train.GetComponent<TrainManager>();
    //    cam.Target = train;
    //    UIcam.Target = train;
    //}

    //public void SpawnBoss()
    //{
    //    EnemyPool boss = ContentPoolable.Emit(ItemID.boss_world_1) as EnemyPool;
    //    BossWorldBase bossWorld = boss.gameObject.GetComponent<BossWorldBase>();
    //    bossWorld.target = trainManager;
    //    listBoss.Add(bossWorld);
    //    enemiesCurrentAmount += 1;
    //}

    //public void CarStart()
    //{
    //    GameScene.main.checker.SetData(trainManager);
    //}
}
