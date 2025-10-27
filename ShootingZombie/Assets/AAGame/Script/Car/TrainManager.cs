using AA_Game;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Yurowm.GameCore;

public class TrainManager : Item
{
    public string nameCar;

    //Public variable
    public float MaxSpeed = 5;
    public float healthBase;
    public float health;
    private float healthConst;
    [HideInInspector]
    public float healthShield;
    [HideInInspector]
    public float damageGiven;
    public Transform GunPosition;
    public Transform GunPositionCollectMode;
    public Transform[] botPos;
    public Slider SliderHealth;

    internal bool IsMoving = false;


    [Header("Boolean Manager")]
    internal bool CheckFloatedUP = true;
    internal bool CheckFloatedDow = false;
    internal bool StartGoingHome = false;
    public bool StartShootings = true;
    internal bool StartIdlingStation = true;
    internal bool Explostion = true;

    //Control Speed
    internal float SpeedFloating = 2.76f;
    internal float acceleration = 5f;
    internal float currentSpeed = 0f;
    internal float FloatingValue = 0f;
    internal float ForceFloating = 1f;
    internal float RotationAngle = 0f;

    [HideInInspector]
    public RotateAndDragOnMouseClick _checker;
    [HideInInspector]
    public CharacterAnim characterAnim;

    //state 
    [HideInInspector]
    public IState currentState;
    [HideInInspector]
    public float timeInstate;
    [HideInInspector]
    public CarState carState;

    BoosterManager booster;

    //
   // public HealthBarController healthBar;


    //for repairer
    public Transform[] posSpawnRepairer;
    public Transform[] posFix;

    //for bots and player
    public Transform[] posBotStart;
    public Transform[] posGetInToTheCar;
    public Transform posPlayerStart;
    public Transform playerPosGetInToTheCar;

    //for fx
    public ItemID effectStart;
    public Transform posFxStart;

    //
    public Transform carPos;
    public GameObject shield;
    public GameObject plancePos;
    public GameObject boom;


    //stun
    public bool isStun;

    public bool isSpawnRepairer;
    public int phaseCar;


    public GameObject[] fxPhaseRunning;
    public GameObject[] fxPhaseIdle;

    public WitchController witchController;
    public Transform posWitch;

    private void OnEnable()
    {
        GameEvent.OnSelectBooster.RemoveListener(OnSelectBooster);
        GameEvent.OnSelectBooster.AddListener(OnSelectBooster);
        GameEvent.OnPlayerLose.AddListener(() => ChangeState(new CardDieState(this)));
        GameEvent.OnPlayerWin.AddListener(() =>
        {
            if (GlobalData.gameMode == GameMode.Normal)
            {
                if (health > (healthBase / 2))
                {
                    GameManager.Instance.starAmount += 1;
                }
            }
            playerGun.GunFlashOff();
        });


        GameEvent.OnUpgradeGara.RemoveListener(SpawnRepairerOnGaraUpdate);
        GameEvent.OnUpgradeGara.AddListener(SpawnRepairerOnGaraUpdate);

        GameEvent.OnCarLevelUp.RemoveListener(OnCarUpgrade);
        GameEvent.OnCarLevelUp.AddListener(OnCarUpgrade);
    }

    private void Awake()
    {
        booster = BoosterManager.instance;
        characterAnim = GetComponent<CharacterAnim>();
        healthBase = User.Instance.Car.hp;
        healthConst = healthBase;
        SetUpHealth();
        GameEvent.OnSelectBooster.AddListener(CheckingBoosterHealth);
        GameEvent.OnUnlockTalent.AddListener(SetUpHealth);
        StartCoroutine(Healing());
        SetSkin(1);
    }

    private void Start()
    {
        GameEvent.OnCarUnlockSlot.AddListener(OnAddSlot);
        characterAnim.skin.CarSetSkin(User.Instance.Car.skin.ToString(), ItemID.phase_1.ToString());
    }

    public void OnCarUpgrade()
    {
        characterAnim.skin.CarSetSkin(User.Instance.Car.skin.ToString(), ItemID.phase_1.ToString());
    }

    public void SetSkin(int skinNum)
    {
        if (skinNum == 1)
        {
            characterAnim.skin.CarSetSkin(User.Instance.Car.skin.ToString(), ItemID.phase_1.ToString());
        }
        else if (skinNum == 2)
        {
            characterAnim.skin.CarSetSkin(User.Instance.Car.skin.ToString(), ItemID.phase_2.ToString());
        }
        else if (skinNum == 3)
        {
            characterAnim.skin.CarSetSkin(User.Instance.Car.skin.ToString(), ItemID.phase_3.ToString());
        }
        else if (skinNum == 4)
        {
            characterAnim.skin.CarSetSkin(User.Instance.Car.skin.ToString(), ItemID.phase_4.ToString());
        }
    }

    public void SetUpHealth()
    {
        healthBase = healthConst + GameManager.Instance.hpTalent;
        health = healthBase;
       // healthBar.gameObject.SetActive(false);
        //SliderHealth.gameObject.SetActive(false);
        GameScene.main.popupPlaying.sliderHpBar.gameObject.SetActive(false);

    }

    public void ChangeState(IState newState)
    {
        timeInstate = 0;
        if (currentState != null)
        {
            currentState.ExitState();
        }
        currentState = newState;
        currentState.EnterState();
        gameObject.name = "Car---" + carState.ToString();
    }


    public DroneController droneController;
    public WitchController _witchController;
    public void OnSelectBooster()
    {
        Item fxUp = ContentPoolable.Emit(ItemID.car_up_1) as Item;
        fxUp.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        fxUp.transform.parent = this.carPos;

        //spawn drone if have booster
        if (booster.listBoost.Contains(NameBooster.Drone) && droneController == null)
        {
            Item drone = ContentPoolable.Emit(ItemID.drone_1) as Item;
            droneController = drone.GetComponent<DroneController>();
            booster.listBoost.Remove(NameBooster.Drone);
        }
        else if (booster.listBoost.Contains(NameBooster.Drone) && droneController != null)
        {
            droneController.liveTime = 0;
        }

        //spawn shield if have booster
        if (booster.listBoost.Contains(NameBooster.Shield))
        {
            shield.SetActive(true);
            healthShield = healthBase / 2;
            booster.listBoost.Remove(NameBooster.Shield);
        }

        //spawn boom if have booster
        if (booster.listBoost.Contains(NameBooster.Plane))
        {
            StartCoroutine(SpawnPlane());
            booster.listBoost.Remove(NameBooster.Plane);
        }


        if (booster.listBoost.Contains(NameBooster.Strange) && _witchController == null)
        {
            SpawnWitch();
            //Item drone = ContentPoolable.Emit(ItemID.WitchChar) as Item;
            //_witchController = drone.GetComponent<WitchController>();
            booster.listBoost.Remove(NameBooster.Strange);
        }
        else if (booster.listBoost.Contains(NameBooster.Strange) && _witchController != null)
        {
            _witchController.liveTime = 0;
        }

    }
    IEnumerator SpawnPlane()
    {
        yield return new WaitForSeconds(1f);
        Item plan = ContentPoolable.Emit(ItemID.plane1) as Item;
        plan.transform.parent = this.transform;
        if (GlobalData.gameMode == GameMode.CollectFuel)
        {
            plancePos.transform.localPosition = new Vector3(plancePos.transform.localPosition.x - 40f, plancePos.transform.localPosition.y, plancePos.transform.localPosition.z); ;
        }
        plan.transform.position = plancePos.transform.position;
    }


    public void CarStart()
    {
        SpawnBot();
        SpawnPlayer();
        carState = CarState.Free;
        characterAnim.PlayAnim(AnimID.idle_stop, true, 1, false);
        
        if(GlobalData.gameMode == GameMode.CollectFuel)
        {
            SpawnGun(true);
        }
        else
        {
            SpawnGun();
        }
        playerGun.GunFlashOff();


        if (GlobalData.gameMode == GameMode.Home)
        {
            //spawn preparer
            if (User.Instance[ItemID.levelHomeStation] >= 1)
            {
                isSpawnRepairer = true;
                for (int i = 0; i < ItemType.Repairer.Count; i++)
                {
                    RepairerControl repairer = ContentPoolable.Emit(ItemType.Repairer[i]) as RepairerControl;
                    repairer.transform.position = this.posFix[i].position;
                    repairer.posFix = this.posFix[i];
                    repairer.ChangeState(new FixingState(repairer));
                }
            }
        }
    }

    /// <summary>
    /// call when gara up to level 1
    /// </summary>
    public void SpawnRepairerOnGaraUpdate()
    {
        if (User.Instance[ItemID.levelHomeStation] >= 1 && isSpawnRepairer == false)
        {
            isSpawnRepairer = true;
            for (int i = 0; i < ItemType.Repairer.Count; i++)
            {
                RepairerControl repairer = ContentPoolable.Emit(ItemType.Repairer[i]) as RepairerControl;
                repairer.transform.position = this.posSpawnRepairer[i].position;
                repairer.posFix = this.posFix[i];
                repairer.ChangeState(new MovingToCarState(repairer));
            }
        }
    }

    public void StartRun()
    {
        carState = CarState.Start;
        characterAnim.PlayAnim(AnimID.start, true, 1, false);

        Item fxStart = ContentPoolable.Emit(this.effectStart) as Item;
        fxStart.transform.position = this.posFxStart.position;
        fxStart.transform.parent = this.posFxStart;

        //start finding target
        StartCoroutine(FindingTargetNearestAuto());
    }

    public void Running()
    {
        carState = CarState.Running;
        SetData();
        characterAnim.PlayAnim(AnimID.idle_run, true, 1, false);

        //neu che do auto
        if (GameManager.Instance.isAutoPlay)
        {

        }
    }

    public void MovingToStation()
    {
        carState = CarState.MovingToStation;
        IsMoving = true;
        this.MaxSpeed = MaxSpeed * 1.5f;
        characterAnim.PlayAnim(AnimID.idle_run, true, 1, false);
        playerGun.GunFlashOff();
    }

    public void MovingHome()
    {
        MovingToStation();
    }


    /// <summary>
    /// On state get hit
    /// </summary>
    public void GetHit()
    {
        float def = (100 - User.Instance.Car.def)/100;
        carState = CarState.Hit;
        if (characterAnim.GetAnimID != AnimID.hit)
        {
            characterAnim.PlayAnim(AnimID.hit, true, 1.5f, false);
        }
        if (healthShield > 0)
        {
            healthShield -= damageGiven * def;
        }
        else
        {
            shield.SetActive(false);
            health -= damageGiven * def;
        }
        if (health <= 0)
        {
            health = 0;
            GameScene.main.popupPlaying.sliderHpBar.value = 0;
            GameScene.main.popupPlaying.txtHealth.text = ((int)health).ToString();
        }
        GameScene.main.popupPlaying.sliderHpBar.value = health / healthBase ;
        GameScene.main.popupPlaying.txtHealth.text = ((int)health).ToString();
        

        if (health <= 0)
        {
            GameEvent.OnPlayerLose.Invoke();
        }
    }

    /// <summary>
    /// Minus health but dont change state
    /// </summary>
    public void GetHitBullet()
    {
        float def = (100 - User.Instance.Car.def) / 100;
        if (healthShield > 0)
        {
            healthShield -= damageGiven * def;
        }
        else
        {
            shield.SetActive(false);
            health -= damageGiven * def;
        }
        if (health <= 0)
        {
            health = 0;
            GameScene.main.popupPlaying.sliderHpBar.value = 0;
            GameScene.main.popupPlaying.txtHealth.text = ((int)health).ToString();
        }
        GameScene.main.popupPlaying.txtHealth.text = ((int)health).ToString();
        GameScene.main.popupPlaying.sliderHpBar.value = health/healthBase;
        if (health <= 0)
        {
            GameEvent.OnPlayerLose.Invoke();
        }
    }


    public void Revive()
    {
        health = healthBase;
        StartShootings = true;

        if (GlobalData.gameMode == GameMode.Normal || GlobalData.gameMode == GameMode.Endless)
        {
            SetData();
            ChangeState(new CarRunningState(this));
            //neu che do auto
            if (GameManager.Instance.isAutoPlay)
            {
                GameEvent.OnShootingAuto.Invoke();
            }
            Debug.Log("Revive");
        }
        else if (GlobalData.gameMode == GameMode.BossWorld)
        {
            SetData();
            ChangeState(new FightingBossState(this));
        }

    }

    public void ReviveBossWorld()
    {
        health = healthBase;
        StartShootings = true;
        SetData();
        ChangeState(new FightingBossState(this));
    }

    public void IdleRunning() //chua can dung
    {

    }
    public void Stop()
    {
        carState = CarState.Stop;
        characterAnim.PlayAnim(AnimID.stop, true, 1, false);
        playerGun.GunFlashOff();
        GameManager.Instance.isWaitingForUpgrade = true;
    }
    public void Die()
    {
        carState = CarState.Die;
        characterAnim.PlayAnim(AnimID.stop, true, 1, false);
      //  healthBar.gameObject.SetActive(false);
        playerGun.GunFlashOff();
    }
    public void Upgrading()
    {
        carState = CarState.Upgrading;
        characterAnim.PlayAnim(AnimID.stop, true, 1, false);
        //spawn preparer
        //for (int i = 0; i < ItemType.Repairer.Count; i++)
        //{
        //    RepairerControl repairer = ContentPoolable.Emit(ItemType.Repairer[i]) as RepairerControl;
        //    repairer.transform.position = this.posSpawnRepairer[i].position;
        //    repairer.posFix = this.posFix[i];
        //}
    }
    public void SetData()
    {
        IsMoving = true;
        _checker = GameScene.main.checker;
      /*  healthBar.gameObject.SetActive(true);
        healthBar.red.transform.localScale = new Vector3(health / healthBase, healthBar.red.transform.localScale.y, healthBar.red.transform.localScale.z);
        healthBar.value.text = ((int)health).ToString() + " / " + ((int)healthBase);*/

      //  SliderHealth.gameObject.SetActive(true);
       // SliderHealth.value = health / healthBase;
        GameScene.main.popupPlaying.sliderHpBar.gameObject.SetActive(true);
        GameScene.main.popupPlaying.sliderHpBar.value = health/healthBase;
        GameScene.main.popupPlaying.txtHealth.text = ((int)health).ToString();
    }

    public GunItem playerGun;

    public void SpawnGun(bool isCollectMode = false)
    {
        playerGun = ContentPoolable.Emit(ItemID.GunCar) as GunItem;

        if (isCollectMode)
        {
            playerGun.transform.parent = GunPositionCollectMode;
            playerGun.transform.localPosition = new Vector3(-2, -0.2f, 0);
            playerGun.transform.localScale = new Vector3(-1,1,1);
        }
        else
        {
            playerGun.transform.parent = GunPosition;
            playerGun.transform.localPosition = new Vector3(0, -1.7f, 0);
            playerGun.transform.localScale = Vector3.one;
        }

        playerGun.transform.localRotation = Quaternion.identity;
        playerGun.GunFlashOff();
    }
    public void SpawnBot(bool isCollectMode = false)
    {
        for (int i = 0; i < User.Instance.Car.slot; i++)
        {
            EnemyPool bot = ContentPoolable.Emit(User.Instance.ListBots()[i]) as EnemyPool;
            bot.transform.parent = botPos[i];
            bot.transform.position = posBotStart[i].position;
            bot.transform.localScale = Vector3.one;
            bot.transform.localRotation = Quaternion.identity;
            bot.GetComponent<CharacterBase>().posMoveToCar = posGetInToTheCar[i];
            bot.GetComponent<CharacterBase>().posIntherCar = botPos[i];

            if (GlobalData.gameMode != GameMode.Home)
            {
                bot.transform.position = botPos[i].position;
                if (isCollectMode && i == 0)
                {
                    bot.transform.position = botPos[3].position;
                }
            }
        }
    }

    public void SpawnWitch()
    {
        EnemyPool witch = ContentPoolable.Emit(ItemID.WitchChar) as EnemyPool;
        witch.transform.position = posWitch.position;
        witch.transform.parent = posWitch;
        witch.gameObject.transform.localScale = Vector3.one;
        if (GlobalData.gameMode == GameMode.CollectFuel)
        {
            witch.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        _witchController = witch.gameObject.GetComponent<WitchController>();
    }

    public void OnAddSlot()
    {
        EnemyPool bot = ContentPoolable.Emit(User.Instance.ListBots()[User.Instance.Car.slot-1]) as EnemyPool;
        bot.transform.parent = botPos[User.Instance.Car.slot-1];
        bot.transform.position = posBotStart[User.Instance.Car.slot-1].position;
        bot.transform.localScale = Vector3.one;
        bot.transform.localRotation = Quaternion.identity;
        bot.GetComponent<CharacterBase>().posMoveToCar = posGetInToTheCar[User.Instance.Car.slot-1];
        bot.GetComponent<CharacterBase>().posIntherCar = botPos[User.Instance.Car.slot-1];
    }

    public void SpawnPlayer(bool isCollectMode =false)
    {
        EnemyPool bot = ContentPoolable.Emit(User.Instance.Player().id) as EnemyPool;
        bot.transform.parent = botPos[3];
        bot.transform.position = posPlayerStart.position;
        bot.transform.localScale = Vector3.one;
        bot.transform.localRotation = Quaternion.identity;
        bot.GetComponent<CharacterBase>().posMoveToCar = playerPosGetInToTheCar;
        bot.GetComponent<CharacterBase>().posIntherCar = botPos[3];

        if (GlobalData.gameMode != GameMode.Home)
        {
            bot.transform.position = botPos[3].position;
            if (isCollectMode)
            {
                bot.transform.position = botPos[0].position;
            }
        }
    }
    private void Update()
    {
        currentState.UpdateState();

        //skin
        if(health <=0 && phaseCar != 4)
        {
            phaseCar = 4;
            SetSkin(phaseCar);
            SetFXPhase(phaseCar);
            return;
        }
        if(health >= 2*healthBase / 3  && phaseCar != 1)
        {
            phaseCar = 1;
            SetSkin(phaseCar);
            foreach (GameObject fx in fxPhaseRunning)
            {
                fx.SetActive(false);
            }
            foreach (GameObject fx in fxPhaseIdle)
            {
                fx.SetActive(false);
            }
            return;
        }
        if(health < 2*healthBase / 3 && phaseCar != 2 && health >= healthBase / 3)
        {
            phaseCar = 2;
            SetSkin(phaseCar);
            SetFXPhase(phaseCar);
            return;
        }
        if(health>0 && health <healthBase/3 && phaseCar != 3)
        {
            phaseCar = 3;
            SetSkin(phaseCar);
            SetFXPhase(phaseCar);
            return;
        }


    }
    private void OnDestroy()
    {
        currentState.ExitState();
    }


    public void SetFXPhase(int phase)
    {
        foreach(GameObject fx in fxPhaseRunning)
        {
            fx.SetActive(false);
        }
        foreach (GameObject fx in fxPhaseIdle)
        {
            fx.SetActive(false);
        }

        if (GlobalData.gameMode == GameMode.BossWorld)
        {
            fxPhaseIdle[phase - 2].SetActive(true);
        }
        else
        {
            fxPhaseRunning[phase - 2].SetActive(true);
        }
    }


    public void CarMoving()
    {
        currentSpeed += acceleration * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, 0f, MaxSpeed);

        if (GlobalData.gameMode == GameMode.CollectFuel)
        {
            transform.Translate(Vector2.right * currentSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.left * currentSpeed * Time.deltaTime);
        }

        transform.Translate(Vector2.up * FloatingValue * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, 0, RotationAngle);

        if (GameManager.Instance.upgradePlace == null)
        {
            return;
        }

        if (GameManager.Instance.isGoingToStation == true)
        {
            float distance = Vector3.Distance(this.transform.position, GameManager.Instance.upgradePlace.transform.position);
            if (distance <= 5)
            {
                GameManager.Instance.isGoingToStation = false;
                ChangeState(new CarStopState(this));
            }
        }

        if (GameManager.Instance.gameState == GameState.Win)
        {
            float distance = Vector3.Distance(this.transform.position, GameManager.Instance.upgradePlace.transform.position);
            if (distance <= 5)
            {
                if (GlobalData.gameMode == GameMode.CollectFuel)
                {
                    PopupManager.Instance.OpenPopup<PopUpGameWin>(PopupID.PopupGameWin);

                }
                else
                {
                    PopupManager.Instance.OpenPopup<PopUpGameWin>(PopupID.PopupGameWin);

                }

                ChangeState(new CardUgradingState(this));
            }
        }
    }
    public void RotationGun()
    {
        //if (_checker != null)
        //{
        //    GunPosition.rotation = _checker.RotationGun;
        //}
    }

    public void StartShooting()
    {
        if (carState == CarState.Running)
        {
            playerGun.GunFlashOn();
        }

        if (StartShootings)
        {
            //player anim
            StartCoroutine(Shooting());
            StartShootings = false;
        }
    }
    IEnumerator Shooting()
    {
        while (true)
        {
            if (!IsMoving || (GameManager.Instance.gameState != GameState.Playing && GameManager.Instance.gameState != GameState.Pausing))
            {
                playerGun.GunFlashOff();
                yield break;
            }
            yield return WaitForSecondsCache.Get(playerGun.speedShoot);
            if (playerGun != null)
            {
                if (carState == CarState.Running || carState == CarState.Hit || carState == CarState.FightingBoss)
                {
                    playerGun.Shoot();
                    ///
                }
            }
        }
    }
    public void CheckingBoosterHealth()
    {
        if (booster.listBoost.Contains(NameBooster.PercentHealth20))
        {
            health += healthBase * 0.2f;
            booster.listBoost.Remove(NameBooster.PercentHealth20);
            if (health > healthBase)
            {
                health = healthBase;
            }
        }
        if (booster.listBoost.Contains(NameBooster.PercentHealth50))
        {
            health += healthBase * 0.5f;
            booster.listBoost.Remove(NameBooster.PercentHealth50);
            if (health > healthBase)
            {
                health = healthBase;
            }
        }
        //healthBar.value.text = ((int)health).ToString() + " / " + ((int)healthBase);

    }


    public EnemyBase targetTemp;
    public EnemyBase target;
    /// <summary>
    /// FOR AUTO MODE
    /// </summary>
    IEnumerator FindingTargetNearestAuto()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (target == null || target.enemyState == EnemyState.Die)
            {
                target = null;
                float dis = 500;

                foreach (EnemyBase enemy in GameManager.Instance.listEnemy)
                {
                    if (enemy != null)
                    {
                        float distance = Vector3.Distance(this.transform.position, enemy.transform.position);
                        if (distance < dis)
                        {
                            dis = distance;
                            targetTemp = enemy;
                        }
                    }
                }

                if (targetTemp != null)
                {
                    float distanceNearest = Vector3.Distance(this.transform.position, targetTemp.transform.position);
                    if (distanceNearest <= 30f)
                    {
                        target = targetTemp;
                    }
                    else
                    {
                        target = null;
                    }
                }
            }
        }
    }


    /// <summary>
    /// FOR USING BOOSTER HEALING
    /// </summary>
    IEnumerator Healing()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);

            if (carState != CarState.Die && health < healthBase)
            {
                if (booster.listBoost.Contains(NameBooster.Healing))
                {
                    health += healthBase * 0.01f;
                    if (health > healthBase)
                    {
                        health = healthBase;
                    }
                }

                health += GameManager.Instance.healingTalent;
                if (health > healthBase)
                {
                    health = healthBase;
                }


                /* healthBar.red.transform.localScale = new Vector3(health / healthBase, healthBar.red.transform.localScale.y, healthBar.red.transform.localScale.z);
                 healthBar.value.text = ((int)health).ToString() + " / " + ((int)healthBase);*/

                // SliderHealth.value = health / healthBase;
                if (health <= 0)
                {
                    health = 0;
                    GameScene.main.popupPlaying.sliderHpBar.value = 0;
                    GameScene.main.popupPlaying.txtHealth.text = ((int)health).ToString();
                }
                GameScene.main.popupPlaying.sliderHpBar.value = health / healthBase;

            }
        }
    }



    #region BOSS WORLD MODE

    public void ReadyFightingBoss()
    {
        carState = CarState.ReadyFightingBoss;
        characterAnim.PlayAnim(AnimID.idle_stop, true, 1, false);
        SpawnGun();
        playerGun.GunFlashOff();
        SetData();
    }

    public void EnterFightingBoss()
    {
        carState = CarState.FightingBoss;
        characterAnim.PlayAnim(AnimID.idle_stop, true, 1, false);
    }

    #endregion


    #region COLLECT

    public void EnterReadyFollowEnemy()
    {
        carState = CarState.ReadyFollowEnemy;
        characterAnim.PlayAnim(AnimID.idle_stop, true, 1, false);
        if (GlobalData.gameMode == GameMode.CollectFuel)
        {
            SpawnGun(true);
        }
        else
        {
            SpawnGun();
        }
        playerGun.GunFlashOff();
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        SetData();
        //acceleration = -acceleration;
        //MaxSpeed = -MaxSpeed;
        ChangeState(new CarStartRunState(this));
    }
    #endregion
}

public enum CarState
{
    Free,
    Start,
    Running,
    MovingToStation,
    Hit,
    Stop,
    Die,
    Upgrading,
    MovingHome,
    ReadyFightingBoss,
    FightingBoss,
    ReadyFollowEnemy,
}
