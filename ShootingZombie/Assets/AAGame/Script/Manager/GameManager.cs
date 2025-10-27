using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yurowm.GameCore;

public class GameManager : Singleton<GameManager>
{
    [Header("BG MODE")]
    public GameObject Parallax;
    public GameObject bgBossWorldMode;
    public BackGroundItem _currentMap;
    public GameState gameState;
    public CameraFollow cam;
    public CameraFollow UIcam;
    [HideInInspector]
    public TrainManager trainManager;
    private int mapIndex;
    [HideInInspector]
    public bool isSelectBooster;
    public bool isSelectTryHero;
    [HideInInspector]
    public int enemiesCurrentAmount;
    [HideInInspector]
    public int totalEnemyInLevel;
    private LevelSetUp levelPlaying;
    [HideInInspector]
    public bool isWaitingForUpgrade;
    [HideInInspector]
    public bool isGoingToStation;
    public GameObject upgradePlace;
    public List<EnemyBase> listEnemy = new List<EnemyBase>();

    //drop
    public PercentDropSetup dropSetUp;
    public PercentDropSetup dropSetupNormal;
    public PercentDropSetup dropSetupCollectMode;
    public List<ItemDrop> listItemDrop = new List<ItemDrop>();
    public int perCentDropGold;
    public int numberMul;


    //mode auto
    public bool isAutoPlay;

    //block pointer
    public bool isBlockPointer;

    //star
    public int starAmount = 0;
    public float timeInGame;

    //Tut
    public GameObject TutGamePlay;

    //booster
    private bool isSelectLuckier;
    private bool isUsedReroll;



    //chi so cua talent
    public float damageTalent;
    public float hpTalent;
    public float healingTalent;
    public float percentCointTalent;
    public bool isRerollTalent;
    public float percentATKTalent;



    //time
    public bool isCaculateTime;

    //gold
    public int goldReward = 500;
    public int ticketReward;
    public int waveOver;

    //dot pha
    public int itemDotPha;

    public RewardStageTableData rewardStage;
    public RewardBossTableData rewardBoss;
    public RewardCollection rewardCollection;
    public RewardEndLess rewardEndless;

    //Enemy stats

    public int mapNumber;

    private void Start()
    {
        //set map
        int x = (int)(User.Instance[ItemID.PlayingLevel] / 5);
        if(x % 2 == 0)
        {
            mapNumber = 0;
        }
        else
        {
            mapNumber = 1;
        }
        GameEvent.OnSetupMap.Invoke();
        



        GameEvent.OnEnemyDie.RemoveListener(OnEnemyDie);
        GameEvent.OnEnemyDie.AddListener(OnEnemyDie);
        GameEvent.OnPlayerLose.RemoveListener(PlayerDie);
        GameEvent.OnPlayerLose.AddListener(PlayerDie);
        GameEvent.OnShowRevive.AddListener(OnShowPopupRevive);
        GameEvent.OnRevive.AddListener(OnRevive);
        GameEvent.OnCloseRevive.AddListener(OnCloseRevive);
        GameEvent.OnUnlockTalent.RemoveListener(CaculateTalent);
        GameEvent.OnUnlockTalent.AddListener(CaculateTalent);


        goldReward = 0;
        ticketReward = 0;
        waveOver = 0;
        CaculateTalent();



        if (User.Instance[ItemID.TutPlay] < 4)
        {
            GlobalData.gameMode = GameMode.Normal;
            GlobalData.instance.isAutoPlay = true;
        }


        if (GlobalData.gameMode == GameMode.Home)
        {
            HomeStart();
        }
        else if (GlobalData.gameMode == GameMode.Normal)
        {
            NormalModeStart();
        }
        else if (GlobalData.gameMode == GameMode.BossWorld)
        {
            BosssWorldModeStart();
        }
        else if (GlobalData.gameMode == GameMode.CollectFuel)
        {
            CollectFuelModeStart();
        }
        else if (GlobalData.gameMode == GameMode.Endless)
        {
            EndlessModeStart();
        }
        if (GlobalData.gameMode == GameMode.CollectFuel)
        {
            dropSetUp = dropSetupCollectMode;
        }
        else
        {
            dropSetUp = dropSetupNormal;
        }


        if (User.Instance[ItemID.Music] == 1)
        {
            AudioManager.instance.MuteAdio(false, "Music");
        }
        else
        {
            AudioManager.instance.MuteAdio(true, "Music");



        }
        if (User.Instance[ItemID.Sound] == 1)
        {
            AudioManager.instance.MuteAdio(false, "Sound");
        }
        else
        {
            AudioManager.instance.MuteAdio(true, "Sound");
        }
    }


    //==============================================================================HOME START==================================================================================//
    public void HomeStart()
    {
        Parallax.SetActive(true);
        SpawnMap(Vector3.zero);
        SpawnGasStation(new Vector3(_currentMap.EndPoint.position.x, 0, 0));
        SpawnCar(new Vector3(-38.5f, -3.4f, 0));
        trainManager.ChangeState(new CarStartState(trainManager));
        cam.GetComponent<CameraFollow>().Offset = new Vector3(0.4f, 4, 0);
        AudioManager.instance.Play("trainRunning");
        AudioManager.instance.VolumBySlider(1, 1);
        AudioManager.instance.Stop("gameplay");
    }


    //==============================================================================NORMAL MODE==================================================================================//
    /// <summary>
    /// Call on Start function if game mode equal Normal Mode
    /// </summary>
    public void NormalModeStart()
    {
        //==========SPAWN ENVIROMENT===========//
        Parallax.SetActive(true);
        SpawnMap(Vector3.zero);
        SpawnMap(new Vector3(_currentMap.EndPoint.position.x, 0, 0));
        SpawnCar(new Vector3(-38.5f, -4f, 0));


        trainManager.ChangeState(new CarStartRunState(trainManager));
        trainManager.SpawnBot();
        trainManager.SpawnPlayer();
        trainManager.SpawnGun();

        cam.GetComponent<CameraFollow>().Offset = new Vector3(9, 4, 0);


        //==========START GAME==========//
        if (User.Instance[ItemID.PlayingLevel] >= 9)
        {
            levelPlaying = LevelConfig.instance.levelSetUps[10];
            foreach(WaveSetup waveSetup in levelPlaying.waveSetups)
            {
                waveSetup.enemyAmount = 15;
                for(int i = 0;i< waveSetup.enemyAmount; i++)
                {
                    waveSetup.enemyID[i] = ItemType.Enemies.GetRandom();
                }
            }
        }
        else
        {
            levelPlaying = LevelConfig.instance.levelSetUps[GlobalData.instance.levelToPlay];
        }

        //GameScene.main.popupPlaying.OnShow();
        //GameScene.main.popupPlaying.btnAutoPlay.gameObject.SetActive(false);
        //GameScene.main.popupPlaying.btnOpenBoosterList.gameObject.SetActive(false);
        StartGameDelay();
        AudioManager.instance.Stop("trainRunning");
        AudioManager.instance.Play("gameplay");
    }




    /// <summary>
    /// Start game by select level from level map normal
    /// </summary>
    public void StartGameDelay()
    {
        GameScene.main.homePanel.PlayBySelectLevel();
    }

    /// <summary>
    /// Start normal game
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartGame()
    {
        GlobalData.instance.isAutoPlay = false;

        BoosterManager.instance.listBoost.Clear();
        BoosterManager.instance.boostersSelected.Clear();
        yield return new WaitForSeconds(2.5f);

        //
        PopupManager.Instance.OpenPopup<PopupTryHero>(PopupID.PopupTryHero);
        isSelectTryHero = false;
        yield return new WaitUntil(() => isSelectTryHero == true);
        yield return new WaitForSeconds(1f);

        PopupManager.Instance.OpenPopup<BoosterPanel>(PopupID.PopupBooster);
        isSelectBooster = false;
        yield return new WaitUntil(() => isSelectBooster == true);

        if (isRerollTalent == true && isUsedReroll == false)
        {
            PopupManager.Instance.OpenPopup<BoosterPanel>(PopupID.PopupBooster);
            isUsedReroll = true;
            yield return new WaitUntil(() => isSelectBooster == true);
        }

        yield return new WaitForSeconds(1f);
        //goi stage
        PopupManager.Instance.OpenPopup<PopupStage>(PopupID.PopupStageNormal);
        //
        yield return new WaitForSeconds(0.5f);
        GameScene.main.popupPlaying.OnShow();
        GameScene.main.popupPlaying.btnAutoPlay.gameObject.SetActive(false);
        if (User.Instance[ItemID.TutPlay] < 4)
        {
            GameScene.main.popupPlaying.btnOpenBoosterList.gameObject.SetActive(false);
        }


        gameState = GameState.Playing;
        GameScene._main.popupPlaying.bgTimePlay.SetActive(true);
        if (User.Instance[ItemID.TutGamePlay] == 0)
        {
            StartCoroutine(ActiveTut());
        }

        GameScene.main.checker.SetData(trainManager);

        GameEvent.OnStartGame.Invoke();
        StartCoroutine(SpawnEnemies());

        totalEnemyInLevel = 0;
        for (int i = 0; i < levelPlaying.waveSetups.Length; i++)
        {
            for (int x = 0; x < levelPlaying.waveSetups[i].enemyAmount; x++)
            {
                totalEnemyInLevel += 1;
            }
        }

        yield return new WaitForSeconds(2f);
        GameScene.main.homePanel.gameObject.SetActive(false);
        if (User.Instance.IndexBtnTutData != 0)
        {
            GameScene._main.popupPlaying.btnAutoPlay.gameObject.SetActive(true);
        }

    }


    //==============================================================================BOSS WORLD MODE==================================================================================//
    /// <summary>
    /// If game mode equal boss world,this function call on start 
    /// </summary>
    public void BosssWorldModeStart()
    {

        bgBossWorldMode.SetActive(true);
        cam.Offset = new Vector3(7.5f, 5, 0);
        UIcam.Offset = new Vector3(7.5f, 5, 0);
        SpawnCar(new Vector3(-2, -10, 0));
        trainManager.SpawnBot();
        trainManager.SpawnPlayer();
        trainManager.ChangeState(new ReadyFightingBossState(trainManager));
        SpawnBoss();
        GameScene.main.homePanel.gameObject.SetActive(false);
        GameScene.main.popupPlaying.gameObject.SetActive(true);
        AudioManager.instance.Stop("trainRunning");
        AudioManager.instance.Play("gameplay");
        StartCoroutine(StartBossWorld());
    }


    /// <summary>
    /// Reality start game boss world
    /// </summary>
    /// <returns></returns>
    IEnumerator StartBossWorld()
    {

        yield return new WaitForSeconds(0.2f);
        PopupManager.Instance.OpenPopup<PopupWarning>(PopupID.PopupWarning);
        yield return new WaitForSeconds(2f);

        PopupManager.Instance.OpenPopup<PopupTryHero>(PopupID.PopupTryHero);
        isSelectTryHero = false;
        yield return new WaitUntil(() => isSelectTryHero == true);
        yield return new WaitForSeconds(1f);

        PopupManager.Instance.OpenPopup<BoosterPanel>(PopupID.PopupBooster);
        yield return new WaitUntil(() => isSelectBooster == true);
        //talent 
        if (isRerollTalent == true && isUsedReroll == false)
        {
            PopupManager.Instance.OpenPopup<BoosterPanel>(PopupID.PopupBooster);
            isUsedReroll = true;
            yield return new WaitUntil(() => isSelectBooster == true);
        }
        trainManager.ChangeState(new FightingBossState(trainManager));
        GameScene.main.checker.SetData(trainManager);
        gameState = GameState.Playing;
        GameEvent.OnStartGame.Invoke();
    }

    /// <summary>
    /// Spawn boss on boss world mode
    /// </summary>
    public void SpawnBoss()
    {
        EnemyPool boss = ContentPoolable.Emit(GlobalData.instance.bossToFight) as EnemyPool;
        EnemyBase bossWorld = boss.gameObject.GetComponent<EnemyBase>();
        bossWorld.target = trainManager;
        listEnemy.Add(bossWorld);
        totalEnemyInLevel += 1;
        enemiesCurrentAmount += 1;
    }



    //==============================================================================COLLECT MODE==================================================================================//
    /// <summary>
    ///Start mode collect 
    /// </summary>
    public void CollectFuelModeStart()
    {
        levelPlaying = LevelConfig.instance.levelSetUpsCollect[0];
        Parallax.SetActive(true);
        SpawnMap(Vector3.zero);
        SpawnMap(new Vector3(_currentMap.EndPoint.position.x, 0, 0));
        //SpawnGasStation(new Vector3(_currentMap.EndPoint.position.x, 0, 0));
        //SpawnTramDungNghi();
        SpawnCar(new Vector3(40, -4, 0));
        trainManager.ChangeState(new ReadyFollowEnemyState(trainManager));
        cam.GetComponent<CameraFollow>().Offset = new Vector3(5.6f, 4, 0);
        trainManager.SpawnBot(true);
        trainManager.SpawnPlayer(true);
        GameScene.main.homePanel.gameObject.SetActive(false);
        GameScene.main.popupPlaying.gameObject.SetActive(true);
        AudioManager.instance.Stop("trainRunning");
        AudioManager.instance.Play("gameplay");
        StartCoroutine(StartCollect());

        totalEnemyInLevel = 0;
        for (int i = 0; i < levelPlaying.waveSetups.Length; i++)
        {
            for (int x = 0; x < levelPlaying.waveSetups[i].enemyAmount; x++)
            {
                totalEnemyInLevel += 1;
            }
        }
    }


    /// <summary>
    /// Reality start game boss world
    /// </summary>
    /// <returns></returns>
    IEnumerator StartCollect()
    {
        yield return new WaitForSeconds(2f);

        PopupManager.Instance.OpenPopup<PopupTryHero>(PopupID.PopupTryHero);
        isSelectTryHero = false;
        yield return new WaitUntil(() => isSelectTryHero == true);
        yield return new WaitForSeconds(1f);


        PopupManager.Instance.OpenPopup<BoosterPanel>(PopupID.PopupBooster);
        yield return new WaitUntil(() => isSelectBooster == true);
        //talent 
        if (isRerollTalent == true && isUsedReroll == false)
        {
            PopupManager.Instance.OpenPopup<BoosterPanel>(PopupID.PopupBooster);
            isUsedReroll = true;
            yield return new WaitUntil(() => isSelectBooster == true);
        }
        //trainManager.ChangeState(new CarStartRunState(trainManager));
        GameScene.main.checker.SetData(trainManager);
        timeInGame = 60;
        gameState = GameState.Playing;
        GameEvent.OnStartGame.Invoke();
        cam.GetComponent<CameraFollow>().Offset = new Vector3(15, 4, 0);
        UIcam.GetComponent<CameraFollow>().Offset = new Vector3(15, 4, 0);
        yield return new WaitForSeconds(1f);
        GameScene._main.popupPlaying.btnAutoPlay.gameObject.SetActive(true);
        //spawn enemy
        StartCoroutine(SpawnEnemiesCollectMode());
    }



    //==============================================================================ENDLESSS MODE==================================================================================//

    /// <summary>
    /// Call on Start function if game mode equal Endless Mode
    /// </summary>
    private void EndlessModeStart()
    {

        Parallax.SetActive(true);
        SpawnMap(Vector3.zero);
        SpawnMap(new Vector3(_currentMap.EndPoint.position.x, 0, 0));
        //SpawnGasStation(new Vector3(_currentMap.EndPoint.position.x, 0, 0));
        SpawnCar(new Vector3(-35, -4, 0));





        //trainManager.ChangeState(new CarStartState(trainManager));
        trainManager.ChangeState(new CarStartRunState(trainManager));
        trainManager.SpawnBot();
        trainManager.SpawnPlayer();
        trainManager.SpawnGun();


        cam.GetComponent<CameraFollow>().Offset = new Vector3(0, 4, 0);
        GameScene.main.homePanel.gameObject.SetActive(false);
        GameScene.main.popupPlaying.gameObject.SetActive(true);
        levelPlaying = new LevelSetUp();
        //tao 30 wave 
        for (int i = 1; i < 10000; i++)
        {
            WaveSetup waveRandom = new WaveSetup();
            if (i <= 15)
            {
                waveRandom.enemyAmount = 5 + i;
                if (i <= 4)
                {
                    for (int y = 0; y < waveRandom.enemyAmount; y++)
                    {
                        Debug.Log(ItemType.EnemiesBasic.Count);
                        waveRandom.enemyID.Add(ItemType.EnemiesBasic.GetRandom());
                    }
                }
                else
                {
                    if ((i > 4) && (i % 5 != 0))
                    {
                        for (int y = 0; y < waveRandom.enemyAmount; y++)
                        {
                            waveRandom.enemyID.Add(ItemType.Enemies.GetRandom());
                        }
                    }
                }
            }
            else
            {
                waveRandom.enemyAmount = 20;
                if (i % 5 != 0)
                {
                    for (int y = 0; y < waveRandom.enemyAmount; y++)
                    {
                        waveRandom.enemyID.Add(ItemType.Enemies.GetRandom());
                    }
                }
            }

            if ((i % 5 == 0) && i != 0)
            {
                waveRandom.enemyAmount = 1;
                waveRandom.enemyID.Add(ItemType.BossNormal.GetRandom());
            }

            levelPlaying.listEndlessWave.Add(waveRandom);
        }

        AudioManager.instance.Stop("trainRunning");
        AudioManager.instance.Play("gameplay");
        AudioManager.instance.VolumBySlider(1, 1);
        StartCoroutine(StartEndlessGame());
    }


    /// <summary>
    /// Start normal game
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartEndlessGame()
    {

        BoosterManager.instance.listBoost.Clear();
        BoosterManager.instance.boostersSelected.Clear();
        yield return new WaitForSeconds(2f);

        PopupManager.Instance.OpenPopup<PopupTryHero>(PopupID.PopupTryHero);
        isSelectTryHero = false;
        yield return new WaitUntil(() => isSelectTryHero == true);

        yield return new WaitForSeconds(1f);

        PopupManager.Instance.OpenPopup<BoosterPanel>(PopupID.PopupBooster);
        isSelectBooster = false;
        GameScene.main.popupPlaying.OnShow();
        yield return new WaitUntil(() => isSelectBooster == true);
        //talent 
        if (isRerollTalent == true && isUsedReroll == false)
        {
            PopupManager.Instance.OpenPopup<BoosterPanel>(PopupID.PopupBooster);
            isUsedReroll = true;
            yield return new WaitUntil(() => isSelectBooster == true);
        }
        GameEvent.OnStartGame.Invoke();
        //yield return new WaitForSeconds(3f);
        cam.GetComponent<CameraFollow>().Offset = new Vector3(9, 4, 0);
        gameState = GameState.Playing;
        ManagerBase.gameState = GameState.Playing;




        // TraimStart();
        GameScene.main.checker.SetData(trainManager);



        totalEnemyInLevel = 9999999;
        StartCoroutine(SpawnEnemies());
        yield return new WaitForSeconds(2f);
        GameScene._main.popupPlaying.btnAutoPlay.gameObject.SetActive(true);
    }






    public void PauseGame()
    {
        gameState = GameState.Pausing;
        ManagerBase.gameState = GameState.Pausing;
        Time.timeScale = 0;
    }
    public void TimeGameTut()
    {
        // gameState = GameState.TutPauseGame;
        Time.timeScale = 0.1f;
    }
    public void EndTut()
    {
        Time.timeScale = 1f;
    }
    public void ResumeGame()
    {
        gameState = GameState.Playing;
        ManagerBase.gameState = GameState.Playing;
        Time.timeScale = 1;
        //trainManager.StartShootings = true;
    }

    public void WinGame()
    {

        starAmount = 0;
        gameState = GameState.Win;
        AudioManager.instance.Stop("gameplay");
        AudioManager.instance.Play("WinGame");
        //DropItem(true);
        GameEvent.OnPlayerWin.Invoke();

        //======FOR MODE======//
        if (GlobalData.gameMode == GameMode.Normal)
        {
            WinNormalMode();
            if (User.Instance[ItemID.PlayingLevel] == GlobalData.instance.levelToPlay)
            {
                User.Instance[ItemID.PlayingLevel] += 1;
            }
        }
        else if (GlobalData.gameMode == GameMode.BossWorld)
        {
            WinBossWorldMode();
            if (User.Instance[ItemID.LevelBossMode] == GlobalData.instance.levelToPlay)
            {
                User.Instance[ItemID.LevelBossMode] += 1;
            }
        }
        else if (GlobalData.gameMode == GameMode.CollectFuel)
        {
            WinCollectMode();
        }


        //======FOR TUT======//
        if (User.Instance.IndexBtnTutData == 0)
        {
            User.Instance.IndexBtnTutData = 1;
            User.Instance[ItemID.TutBooster] = 1;
        }
        //=======TUT UPGRADE====//
        if (User.Instance[ItemID.Gold] >= 1000)
        {
            User.Instance.IndexBtnTutData = 2;
        }

    }

    private void OnApplicationQuit()
    {
        GlobalData.gameMode = GameMode.Home;
        User.Instance.Save();
    }

    /// <summary>
    /// Win game on normal mode
    /// </summary>
    public void WinNormalMode()
    {
        #region STAR CACULATE
        starAmount += 1;
        if (timeInGame <= 120f)
        {
            starAmount += 1;
        }
        StarLevel starLevel = new StarLevel();
        starLevel.level = GlobalData.instance.levelToPlay;
        starLevel.starAmount = this.starAmount;

        if (GlobalData.instance.levelToPlay <= User.Instance[ItemID.PlayingLevel])
        {
            foreach (StarLevel level in User.Instance.ListStarLevel())
            {
                if (level.level == starLevel.level)
                {
                    if (level.starAmount < starLevel.starAmount)
                    {
                        User.Instance.ListStarLevel()[User.Instance.ListStarLevel().IndexOf(level)] = starLevel;
                        User.Instance.Save();
                        break;
                    }
                }
            }
        }

        if (GlobalData.instance.levelToPlay == User.Instance[ItemID.PlayingLevel])
        {
            StarLevel newLevel = new StarLevel();
            newLevel.level = GlobalData.instance.levelToPlay + 1;    //neu con level cao hon
            newLevel.starAmount = 0;
            User.Instance.ListStarLevel().Add(newLevel);
            User.Instance.Save();
        }
        #endregion

        #region GOLD
        goldReward += rewardStage.goldNormalMode[GlobalData.instance.levelToPlay];
        if(GlobalData.instance.levelToPlay <= 30)
        {
            itemDotPha += Random.Range(rewardStage.dotPhaMin[GlobalData.instance.levelToPlay], rewardStage.dotPhaMax[GlobalData.instance.levelToPlay]);
        }
        else
        {
            int x = Random.Range(3, 30);
            itemDotPha += Random.Range(rewardStage.dotPhaMin[x], rewardStage.dotPhaMax[x]);
        }
        #endregion

        SpawnTramDungNghi(new Vector3(_currentMap.EndPoint.position.x, 0, 0));
        trainManager.ChangeState(new CarMovingHomeState(trainManager));
    }


    /// <summary>
    /// Win game on boss world mode
    /// </summary>
    public void WinBossWorldMode()
    {
        starAmount = 3;
        if(GlobalData.instance.levelToPlay == User.Instance[ItemID.LevelBossMode])
        {
            User.Instance[ItemID.LevelBossMode] += 1;
        }

        if(GlobalData.instance.levelToPlay > rewardBoss.rewardConfig.Count)
        {
            goldReward += rewardBoss.rewardConfig[Random.Range(0, rewardBoss.rewardConfig.Count)].gold;
            ticketReward += rewardBoss.rewardConfig[Random.Range(0, rewardBoss.rewardConfig.Count)].ticket;
        }
        else
        {
            goldReward += rewardBoss.rewardConfig[GlobalData.instance.levelToPlay - 1].gold;
            ticketReward += rewardBoss.rewardConfig[GlobalData.instance.levelToPlay - 1].ticket;
        }
        StartCoroutine(DelayShowPopupWin());
    }

    /// <summary>
    /// Win game on collect mode
    /// </summary>
    public void WinCollectMode()
    {
        starAmount = 3;
        SpawnTramDungNghi(new Vector3(_currentMap.EndPoint.position.x, 0, 0));
        trainManager.ChangeState(new CarMovingHomeState(trainManager));
        goldReward += Random.Range(1000, 5000);

        int levelReward = User.Instance[ItemID.PlayingLevel];
        if(levelReward >= 29)
        {
            levelReward = 29;
        }
        int min = rewardStage.goldNormalMode[levelReward] - 200;
        int max = rewardStage.goldNormalMode[levelReward] + 300;
        goldReward += Random.Range(min,max);

        // goldReward += rewardCollection.rewardConfig[Random.Range(0, 1)].gold;
        /* goldReward += rewardStage.goldNormalMode[GlobalData.instance.levelToPlay];
         if (GlobalData.instance.levelToPlay <= 30)
         {
             itemDotPha += Random.Range(rewardStage.dotPhaMin[GlobalData.instance.levelToPlay], rewardStage.dotPhaMax[GlobalData.instance.levelToPlay]);
         }
         else
         {
             int x = Random.Range(3, 30);
             itemDotPha += Random.Range(rewardStage.dotPhaMin[x], rewardStage.dotPhaMax[x]);
         }*/
        StartCoroutine(DelayShowPopupWin());
    }

    IEnumerator DelayShowPopupWin()
    {
        yield return new WaitForSeconds(2f);
        if (GlobalData.gameMode == GameMode.CollectFuel)
        {
            PopupManager.Instance.OpenPopup<PopUpGameWin>(PopupID.PopupGameWin);
        }
        else
        {
            PopupManager.Instance.OpenPopup<PopUpGameWin>(PopupID.PopupGameWin);
        }
    }

    public void PlayerDie()
    {
        gameState = GameState.Lose;
        ManagerBase.gameState = GameState.Lose;
        PopupManager.Instance.OpenPopup<RevivePopup>(PopupID.PopupRevive);
    }

    public void LoseGame()
    {
        gameState = GameState.Lose;
        DropItem(false);
        AudioManager.instance.Stop("gameplay");
        AudioManager.instance.Play("loser");
        //======FOR MODE======//
        if (GlobalData.gameMode == GameMode.Normal)
        {
            LoseNormalMode();
        }
        else if (GlobalData.gameMode == GameMode.BossWorld)
        {
            LoseBossWorldMode();
        }
        else if (GlobalData.gameMode == GameMode.CollectFuel)
        {
            LoseCollectMode();
        }
        else if (GlobalData.gameMode == GameMode.Endless)
        {
            LoseEndlessMode();
        }
    }

    public void LoseNormalMode()
    {
        goldReward += rewardStage.goldNormalMode[GlobalData.instance.levelToPlay]/3;
        PopupManager.Instance.OpenPopup<PopUpGameDefect>(PopupID.PopupGameDefeact);
    }

    public void LoseBossWorldMode()
    {
        goldReward += rewardBoss.rewardConfig[0].gold;
        ticketReward += rewardBoss.rewardConfig[0].ticket;
        goldReward += rewardStage.goldNormalMode[GlobalData.instance.levelToPlay] / 3;
        goldReward = (int)(goldReward / 10);
        ticketReward = (int)(ticketReward / 10);
        PopupManager.Instance.OpenPopup<PopUpGameDefect>(PopupID.PopupGameDefeact);
    }

    public void LoseCollectMode()
    {
        PopupManager.Instance.OpenPopup<PopUpGameDefect>(PopupID.PopupGameDefeact);
    }

    public int bua;
    public void LoseEndlessMode()
    {
        goldReward += waveOver * 10;
        ticketReward = (int)(ticketReward / 10);
        goldReward += rewardStage.goldNormalMode[GlobalData.instance.levelToPlay] / 3;

        int indexWaveRewardEndless;
        indexWaveRewardEndless = (int)(waveOver / 5);
        if(indexWaveRewardEndless > 5)
        {
            indexWaveRewardEndless = 5;
        }

        itemDotPha = Random.Range(rewardEndless.waveRewardEndlesses[indexWaveRewardEndless].manhDotPhaMin, rewardEndless.waveRewardEndlesses[indexWaveRewardEndless].manhDotPhaMax);
        bua = rewardEndless.waveRewardEndlesses[indexWaveRewardEndless].bua;
        PopupManager.Instance.OpenPopup<PopUpGameDefect>(PopupID.PopupGameDefeact);
    }





    public void Revive()
    {
        StartCoroutine(DoRevive());
    }

    IEnumerator DoRevive()
    {
        gameState = GameState.Playing;
        ManagerBase.gameState = GameState.Playing;
        trainManager.Revive();
        yield return new WaitForSeconds(2f);
        GameEvent.OnReviveGame.Invoke();
        PopupManager.Instance.CloseCurrentPopup();
    }

    public void OnShowPopupRevive()
    {

    }
    public void OnRevive()
    {
        Revive();
    }
    public void OnCloseRevive()
    {
        LoseGame();
    }


    void Update()
    {
        if (gameState == GameState.Playing)
        {
            if(GlobalData.gameMode == GameMode.CollectFuel)
            {
                timeInGame -= Time.deltaTime;
                int minutes = Mathf.FloorToInt(timeInGame / 60);
                int seconds = Mathf.FloorToInt(timeInGame % 60);
                GameScene._main.popupPlaying.textTime.text = string.Format("{0:00}:{1:00}", minutes, seconds);
                if(timeInGame <= 0)
                {
                    timeInGame = 0;
                    GameScene._main.popupPlaying.textTime.text = "00:00";
                    WinGame();
                }
            }
            else
            {
                timeInGame += Time.deltaTime;
                int minutes = Mathf.FloorToInt(timeInGame / 60);
                int seconds = Mathf.FloorToInt(timeInGame % 60);
                GameScene._main.popupPlaying.textTime.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
        }


        if (trainManager != null && _currentMap != null)
        {
            float dis = Vector3.Distance(trainManager.transform.position, _currentMap.EndPoint.position);
            if (Vector3.Distance(trainManager.transform.position, _currentMap.EndPoint.position) <= 60)
            {
                SpawnMap(new Vector3(_currentMap.EndPoint.position.x, 0, 0));
            }
        }
    }

    
    public void SpawnMap(Vector3 position)
    {
        BackGroundItem newMap = null;
        if (mapNumber == 0)
        {
            newMap = ContentPoolable.Emit(ItemType.Map_1[mapIndex]) as BackGroundItem;
        }
        else
        {
            newMap = ContentPoolable.Emit(ItemID.map_2_1) as BackGroundItem;
        }

        mapIndex += 1;
        if (mapIndex >= ItemType.Map_1.Count - 2)
        {
            mapIndex = 0;
        }
        newMap.transform.localScale = Vector3.one * 2;
        newMap.transform.position = position;


        if (GlobalData.gameMode == GameMode.CollectFuel)
        {
            newMap.EndPoint.transform.localPosition = new Vector3(-newMap.EndPoint.transform.localPosition.x, newMap.EndPoint.transform.localPosition.y, newMap.EndPoint.transform.localPosition.z);
        }

        _currentMap = newMap;
    }

    public void SpawnGasStation(Vector3 position)
    {
        BackGroundItem newMap = ContentPoolable.Emit(ItemID.map_1_GasStation) as BackGroundItem;
        newMap.transform.localScale = Vector3.one * 2;
        newMap.transform.position = position;
        _currentMap = newMap;
        upgradePlace = newMap.gameObject;
        if (GlobalData.gameMode == GameMode.CollectFuel)
        {
            _currentMap.EndPoint.transform.localPosition = new Vector3(-_currentMap.EndPoint.transform.localPosition.x, _currentMap.EndPoint.transform.localPosition.y, _currentMap.EndPoint.transform.localPosition.z);
        }
    }


    public void SpawnTramDungNghi(Vector3 position)
    {
        BackGroundItem newMap = ContentPoolable.Emit(ItemID.map_1_TramDungNghi) as BackGroundItem;
        newMap.transform.localScale = Vector3.one * 2;
        newMap.transform.position = position;
        _currentMap = newMap;
        upgradePlace = newMap.gameObject;
        if (GlobalData.gameMode == GameMode.CollectFuel)
        {
            _currentMap.EndPoint.transform.localPosition = new Vector3(-_currentMap.EndPoint.transform.localPosition.x, _currentMap.EndPoint.transform.localPosition.y, _currentMap.EndPoint.transform.localPosition.z);
        }
    }


    public void SpawnCar(Vector3 pos)
    {
        GameObject train = Instantiate(Resources.Load<GameObject>("Car/car_1"), pos, transform.rotation);
        trainManager = train.GetComponent<TrainManager>();
        cam.Target = train;
        UIcam.Target = train;
    }


    public void TraimStart()
    {
        trainManager.ChangeState(new CarStartRunState(trainManager));
        GameScene.main.checker.SetData(trainManager);
    }

    /// <summary>
    /// For normal mode
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(0);

        int amountLevel = 0;
        if (GlobalData.gameMode == GameMode.Normal)
        {
            amountLevel = levelPlaying.waveSetups.Length;
        }
        else if (GlobalData.gameMode == GameMode.Endless)
        {
            amountLevel = levelPlaying.listEndlessWave.Count;
        }


        for (int i = 0; i < amountLevel; i++)
        {
            //for booster
            waveOver = i;
            isSelectBooster = false;

            int waveNeedToBooster = 0;
            if (GlobalData.gameMode == GameMode.Normal)
            {
                waveNeedToBooster = 3;
            }
            else if (GlobalData.gameMode == GameMode.Endless)
            {
                waveNeedToBooster = 5;
            }

            if ((i % waveNeedToBooster == 0) && (i != 0))
            {
                yield return new WaitUntil(() => enemiesCurrentAmount == 0);
                GameManager.Instance.isGoingToStation = true;
                GameManager.Instance.trainManager.ChangeState(new CarMovingToUpgradeState(GameManager.Instance.trainManager));

                SpawnTramDungNghi(new Vector3(_currentMap.EndPoint.position.x, 0, 0));

                yield return new WaitUntil(() => isWaitingForUpgrade == true);
                PopupManager.Instance.OpenPopup<BoosterPanel>(PopupID.PopupBooster);
                yield return new WaitUntil(() => isSelectBooster == true);

                //su dung booster luckier
                if (BoosterManager.instance.listBoost.Contains(NameBooster.Luckier) && isSelectLuckier == false)
                {
                    PopupManager.Instance.OpenPopup<BoosterPanel>(PopupID.PopupBooster);
                    isSelectLuckier = true;
                    yield return new WaitUntil(() => isSelectBooster == true);
                }





                yield return new WaitForSeconds(1f);
            }
            else
            {
                if (i == 0)
                {
                    if (GlobalData.gameMode == GameMode.Endless)
                    {
                        yield return new WaitForSeconds(0f);
                    }
                    else
                    {
                        yield return new WaitForSeconds(4f);
                    }
                }
                else
                {
                    yield return new WaitUntil(() => enemiesCurrentAmount == 0);
                }
            }
            //for spawn enemy

            int amoutEnemyInWave = 0;
            if (GlobalData.gameMode == GameMode.Normal)
            {
                amoutEnemyInWave = levelPlaying.waveSetups[i].enemyAmount;
            }
            else if (GlobalData.gameMode == GameMode.Endless)
            {
                amoutEnemyInWave = levelPlaying.listEndlessWave[i].enemyAmount;
            }


            for (int x = 0; x < amoutEnemyInWave; x++)
            {
                yield return new WaitForSeconds(Random.Range(0.5f, 1f));
                yield return new WaitUntil(() => gameState == GameState.Playing);
                EnemyPool enemy = null;

                //for mode
                if (GlobalData.gameMode == GameMode.Normal)
                {
                    enemy = ContentPoolable.Emit(levelPlaying.waveSetups[i].enemyID[x]) as EnemyPool;
                }
                else if (GlobalData.gameMode == GameMode.Endless)
                {
                    enemy = ContentPoolable.Emit(levelPlaying.listEndlessWave[i].enemyID[x]) as EnemyPool;
                    EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();

                    if (i <= 10)
                    {
                        for (int z = 0; z < i; z++)
                        {
                            enemyBase.healthBase = enemyBase.healthBase + enemyBase.healthBase * 0.1f;
                        }
                    }
                    else
                    {
                        for (int y = 0; y <= 10; y++)
                        {
                            enemyBase.healthBase = enemyBase.healthBase + enemyBase.healthBase * 0.1f;
                        }
                        for (int p = 11; p <= i; p++)
                        {
                            enemyBase.healthBase = enemyBase.healthBase + enemyBase.healthBase * 0.3f;
                        }
                    }
                    enemyBase.health = enemyBase.healthBase;

                    //damage up for wave endless
                    for (int d = 0; d < i; d++)
                    {
                        enemyBase.damage = enemyBase.damage + enemyBase.damage * 0.1f;
                    }
                }



                Vector3 pos = new Vector3(trainManager.transform.position.x + Random.Range(20, 28), Random.Range(-5, 0), 0);
                enemy.transform.position = pos;
                enemy.GetComponent<EnemyBase>().target = trainManager;
                enemiesCurrentAmount += 1;
                listEnemy.Add(enemy.GetComponent<EnemyBase>());


                //set pos
                if (enemy.GetComponent<EnemyBase>().typeEnemy == TypeEnemy.Enemy_Fly)
                {
                    enemy.transform.position = new Vector3(trainManager.transform.position.x + Random.Range(25, 30), Random.Range(10, 15), 0);
                }
                else if (enemy.GetComponent<EnemyBase>().typeEnemy == TypeEnemy.Enemy_Basic || enemy.GetComponent<EnemyBase>().typeEnemy == TypeEnemy.Enemy_Tank)
                {
                    enemy.transform.position = new Vector3(pos.x - 10, pos.y, pos.z);
                }
                else if (enemy.GetComponent<EnemyBase>().typeEnemy == TypeEnemy.Enemy_Motobike)
                {
                    enemy.transform.position = new Vector3(pos.x + 13, pos.y, pos.z);
                }
            }
        }
    }



    /// <summary>
    /// For CollectMode mode
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnEnemiesCollectMode()
    {
        yield return new WaitForSeconds(0);
        for (int i = 0; i < levelPlaying.waveSetups.Length; i++)
        {
            //for booster
            isSelectBooster = false;
            //if ((i % 3 == 0) && (i != 0))
            //{
            //    yield return new WaitUntil(() => enemiesCurrentAmount == 0);
            //    GameManager.Instance.isGoingToStation = true;
            //    GameManager.Instance.trainManager.ChangeState(new CarMovingToUpgradeState(GameManager.Instance.trainManager));
            //    if (i != 0)
            //    {
            //        SpawnTramDungNghi(new Vector3(_currentMap.EndPoint.position.x, 0, 0));
            //    }
            //    yield return new WaitUntil(() => isWaitingForUpgrade == true);
            //    PopupManager.Instance.OpenPopup<BoosterPanel>(PopupID.PopupBooster);
            //    yield return new WaitUntil(() => isSelectBooster == true);

            //    //su dung booster luckier
            //    if (BoosterManager.instance.listBoost.Contains(NameBooster.Luckier))
            //    {
            //        PopupManager.Instance.OpenPopup<BoosterPanel>(PopupID.PopupBooster);
            //        yield return new WaitUntil(() => isSelectBooster == true);
            //    }

            //    yield return new WaitForSeconds(1f);
            //}
            //else
            //{
            //    if (i == 0)
            //    {
            //        yield return new WaitForSeconds(4f);
            //    }
            //    else
            //    {
            //        yield return new WaitUntil(() => enemiesCurrentAmount == 0);
            //    }
            //}


            //for spawn enemy
            for (int x = 0; x < levelPlaying.waveSetups[i].enemyAmount; x++)
            {
                yield return new WaitForSeconds(Random.Range(0.5f, 1f));
                yield return new WaitUntil(() => enemiesCurrentAmount < 6);
                yield return new WaitUntil(() => gameState == GameState.Playing);
                EnemyPool enemy = ContentPoolable.Emit(levelPlaying.waveSetups[i].enemyID[x]) as EnemyPool;
                Vector3 pos = new Vector3(trainManager.transform.position.x + Random.Range(30, 35), Random.Range(-5, 0), 0);
                enemy.transform.position = pos;
                enemy.GetComponent<EnemyBase>().target = trainManager;
                if (enemy.GetComponent<EnemyBase>().typeEnemy == TypeEnemy.Enemy_Fly)
                {
                    enemy.transform.position = new Vector3(pos.x, pos.y + Random.Range(5, 8), pos.z);
                }
                enemiesCurrentAmount += 1;
                listEnemy.Add(enemy.GetComponent<EnemyBase>());
                enemy.GetComponent<EnemyBase>().ChangeState(new LeaveTheCarState(enemy.GetComponent<EnemyBase>()));
            }
        }
    }

    public void OnEnemyDie(EnemyBase enemy)
    {
        listEnemy.Remove(enemy);  //SUA LAI EVEN ENEMY DIE
        enemiesCurrentAmount -= 1;
        totalEnemyInLevel -= 1;

        if (totalEnemyInLevel <= 0)
        {
            if(GlobalData.gameMode != GameMode.CollectFuel)
            {
                WinGame();
            }
        }

        if (GlobalData.gameMode != GameMode.CollectFuel)
        {
            goldReward += 2;
        }
    }


    /// <summary>
    /// Drop item when win game
    /// </summary>
    public void DropItem(bool isWin)
    {
        listItemDrop.Clear();



        for (int i = 0; i < 5; i++)
        {
            CaculateDrop(i, isWin);
        }
    }

    /// <summary>
    /// Caculate drop percent by level
    /// </summary>
    /// <param name="indexManh"></param>
    /// <returns></returns>
    public int CaculateDrop(int indexManh, bool isWin)  //0-naosung   1-bangsung   2-vodan   3-loxo  4-conoc
    {
        int amount = 0;
        int perCentDrop;

        //ti le nho bang 1/3 neu thua
        if (isWin)
        {
            perCentDrop = dropSetUp.levelDropPercent[GlobalData.instance.levelToPlay].percents[indexManh].percent;
        }
        else
        {
            perCentDrop = (int)(dropSetUp.levelDropPercent[GlobalData.instance.levelToPlay].percents[indexManh].percent / 3);
        }

        //neu nhu so random nho honw so ti le thi drop
        if (Random.Range(0, 100) < perCentDrop)
        {
            amount = Random.Range(5, 10);

            switch (dropSetUp.levelDropPercent[GlobalData.instance.levelToPlay].percents[indexManh].IDManhSung)
            {
                case ItemID.Manh_NaoSung:
                    amount = Random.Range(5, 11);
                    break;
                case ItemID.Manh_BangSung:
                    amount = Random.Range(1, 6);
                    break;
                case ItemID.Manh_VoDan:
                    amount = Random.Range(1, 6);
                    break;
                case ItemID.Manh_LoXo:
                    amount = Random.Range(1, 5);
                    break;
                case ItemID.Manh_ConOc:
                    amount = Random.Range(1, 3);
                    break;
            }
        }

        if (amount > 0)
        {
            ItemDrop item = new ItemDrop();
            item.IDManhSung = dropSetUp.levelDropPercent[GlobalData.instance.levelToPlay].percents[indexManh].IDManhSung;
            item.amount = amount;
            listItemDrop.Add(item);
        }

        return amount;
    }

    public void SaveItemDrop(int numberMul)
    {
        for (int i = 0; i < numberMul; i++)
        {
            foreach (ItemDrop item in listItemDrop)
            {
                User.Instance[item.IDManhSung] += item.amount;
                User.Instance.Save();
            }
        }
    }


    //===================TUT===================//
    public bool isTutPlayAtive;
    IEnumerator ActiveTut()
    {
        yield return new WaitForSeconds(5f);
        if (User.Instance[ItemID.TutPlay] == 0)
        {
            TimeGameTut();
            TutGamePlay.gameObject.SetActive(true);
            isTutPlayAtive = true;
        }
        yield return new WaitUntil(() => isTutPlayAtive == false);
        if (User.Instance[ItemID.TutPlay] == 1)
        {
            yield return new WaitForSeconds(2f);
            Time.timeScale = 0.2f;
            User.Instance[ItemID.TutBoosterVip] = 1;
            GameEvent.OnSetTrueTutVip.Invoke(NameBooster.Drone);
        }
    }


    public void CaculateTalent()
    {
        damageTalent = 0;
        hpTalent = 0;
        healingTalent = 0;
        percentCointTalent = 0;
        percentATKTalent = 0;
        isRerollTalent = false;

        foreach (Talent talent in User.Instance.UserTalents())
        {
            if (talent.talentType == TalentType.Damage)
            {
                damageTalent += talent.amountUp;
            }
            else if (talent.talentType == TalentType.HP)
            {
                hpTalent += talent.amountUp;
            }
            else if (talent.talentType == TalentType.Healing)
            {
                healingTalent += talent.amountUp;
            }
            else if (talent.talentType == TalentType.Coin)
            {
                percentCointTalent += talent.amountUp;
            }
            else if (talent.talentType == TalentType.ReRoll)
            {
                isRerollTalent = true;
            }
            else if (talent.talentType == TalentType.ATK)
            {
                percentATKTalent += talent.amountUp;
            }
        }
    }
    public Camera GameCamera;
    public Camera UICamera;
    [SerializeField] private RectTransform tempcam;
    private float heightScenes;
    private float widthScenes;
    private Vector3 posArrowInScenes;
    [SerializeField] private RectTransform arrow;
//    public Transform postemgun;
   
    public void TurnOffArow()
    {
        arrow.gameObject.SetActive(false);
       
    }
    public void TurnArrow(Transform posTarget)
    {
        if (arrow.gameObject.activeSelf == false)
        {
            arrow.gameObject.SetActive(true);
        }
    
        CalculationPos(posTarget);
        arrow.anchoredPosition = posArrowInScenes;
        //   arrow.transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
    public void CalculationPos(Transform posTarget)
    {
        Vector3 gamePos = GameCamera.WorldToViewportPoint(posTarget.position);
        Vector3 uiPos = UICamera.ViewportToWorldPoint(gamePos);
        tempcam.position = uiPos;
        Vector3 inUi = tempcam.anchoredPosition;
        if ((inUi.x < widthScenes / 2 && inUi.x > -widthScenes / 2)
            && (inUi.y < heightScenes / 2 && inUi.y > -heightScenes / 2))
        {
            TurnOffArow();
        }
        else
        {
            float x = Mathf.Clamp(inUi.x, -widthScenes / 2, widthScenes / 2);
            float y = Mathf.Clamp(inUi.y, -heightScenes / 2, heightScenes / 2);
            posArrowInScenes = new Vector3(x, y, 0);
        }
    }
}



public enum GameState
{
    Free,
    Playing,
    Pausing,
    Lose,
    Win,
    TutPauseGame,
}

public enum GameMode
{
    Home,
    Normal,
    BossWorld,
    CollectFuel,
    Endless,
}
