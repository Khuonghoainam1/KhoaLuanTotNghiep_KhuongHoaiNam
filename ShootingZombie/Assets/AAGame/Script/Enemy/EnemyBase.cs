using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using Yurowm.GameCore;

public abstract class EnemyBase : MonoBehaviour
{
    //==========ENEMY STATS===========//
    public TypeEnemy typeEnemy;
    public float healthBase;
    [HideInInspector]
    public float health;
    public float damage;
    public float healthConst;
    [HideInInspector]
    public float speed;
    public float speedBase;

    public float SpeedGrowingUp = 0f;
    public float RandomSpeed = 0f;

    public float distanceAttack;
    [HideInInspector]
    public float damageGiven;
    [HideInInspector]
    public TrainManager target;

    public CharacterAnim anim;
    public EnemyPool pool;
    public HealthBarController healthBar;
    public Transform hitPos;
    [HideInInspector]
    public float timeInState;
    [HideInInspector]
    public float randomDistanceAttack;
    public EnemyState enemyState;
    protected IState currentState;
    public CapsuleCollider2D capsuleCollider2D;
    public Vector3 targetComeTo;

    //for effect
    public ItemID hitEffect;
    public ItemID fxDie;
    public FxItem hitFx;

    //for layer
    public MeshRenderer meshRenderer;

    protected string attackSoundFx = "enemyatk";
    protected string spawnSoundFX = "enemyspawn";

    public EnemyStatsData enemyStatsNormal;
    public EnemyStats enemyStatsEndless;
    public List<string> skinsEnemy;


    public virtual void OnEnable()
    {
        if (hitFx != null)
        {
            hitFx.Kill();
        }
        speed = speedBase;
        RandomSpeed = Random.Range(0, 2f);
        SpeedGrowingUp = 0;

        //health by level setup
        if (GlobalData.gameMode == GameMode.Endless)
        {
            healthBase = enemyStatsEndless.hp;
            damage = enemyStatsEndless.atk;
        }
        else
        {
            if (GlobalData.instance.levelToPlay <= 29)
            {
                healthBase = enemyStatsNormal.enemyStatsNormal[GlobalData.instance.levelToPlay].hp;
                damage = enemyStatsNormal.enemyStatsNormal[GlobalData.instance.levelToPlay].atk;
            }
            else
            {
                healthBase = enemyStatsNormal.enemyStatsNormal[29].hp;
                damage = enemyStatsNormal.enemyStatsNormal[29].atk;
            }
        }


        if (healthConst == 0)
        {
            healthConst = healthBase;
        }
        health = healthBase;
        healthBar.red.transform.localScale = new Vector3(health / healthBase, healthBar.red.transform.localScale.y, healthBar.red.transform.localScale.z);
        healthBar.gameObject.SetActive(false);
        anim.OnHandleEvent.RemoveListener(HandleEvent);
        anim.OnHandleEvent.AddListener(HandleEvent);
        GameEvent.OnPlayerLose.AddListener(OnWin);
        GameEvent.OnReviveGame.RemoveListener(OnRevive);
        GameEvent.OnReviveGame.AddListener(OnRevive);
        GameEvent.OnPlayerWin.AddListener(OnLose);
        foreach (Transform obj in hitPos)
        {
            if (obj != null)
            {
                obj.GetComponent<FxItem>().Kill();
            }
        }

        if (skinsEnemy.Count > 0)
        {
            string skin = skinsEnemy.GetRandom();
            anim.skin.SetSkin(skin);
        }
    }

    public virtual void OnDisable()
    {
        GameEvent.OnPlayerLose.RemoveListener(OnWin);
        GameEvent.OnPlayerWin.RemoveListener(OnLose);
    }

    public virtual void Start()
    {
        healthConst = healthBase;
    }

    public virtual void Update()
    {
        currentState.UpdateState();
        meshRenderer.sortingOrder = (int)(-20 - transform.position.y);
    }

    private void OnDestroy()
    {
        currentState.ExitState();
    }

    public virtual void OnRevive()    //boss world override this function
    {
        ChangeState(new FollowCar(this));
    }

    public virtual void OnWin()
    {
        ChangeState(new EnemyVictoryState(this));
    }

    public virtual void OnLose()
    {
        ChangeState(new EnemyLoseState(this));
    }

    public virtual void ChangeState(IState newState)
    {
        timeInState = 0;
        if (currentState != null)
        {
            currentState.ExitState();
        }
        currentState = newState;
        currentState.EnterState();
    }

    public virtual void HandleEvent(string eventName)
    {
        if (eventName == "attack" || eventName == "attack_tracking")
        {
            target.damageGiven = damage;
            if (target.carState == CarState.Hit)
            {
                target.GetHitBullet();
            }
            else
            {
                if (GlobalData.gameMode == GameMode.Normal || GlobalData.gameMode == GameMode.Endless)
                {
                    target.ChangeState(new CarHitState(target));
                }
                else
                {
                    target.GetHitBullet();
                }
            }
        }
    }

    public virtual void TakeDamage(float damage)
    {
        if (health > 0 && health <= healthBase)
        {
            healthBar.gameObject.SetActive(true);
        }
        health -= damage;
        healthBar.red.transform.localScale = new Vector3(health / healthBase, healthBar.red.transform.localScale.y, healthBar.red.transform.localScale.z);

        if (health <= 0)
        {
            healthBar.gameObject.SetActive(false);
            capsuleCollider2D.enabled = false;
            health = 10000;//lock double die
            if (enemyState != EnemyState.Die)
            {

                ChangeState(new DieState(this));
            }
        }
    }


    public void Lose()
    {
        ChangeState(new DieState(this));
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            BulletBase bullet = collision.gameObject.GetComponent<BulletBase>();
            bullet.BulletHit();
            this.hitEffect = bullet.hitEffect;

            if (bullet.isBombPlane)
            {
                if(typeEnemy != TypeEnemy.Boss_Normal && typeEnemy != TypeEnemy.Boss_World)
                {
                    TakeDamage(100000);
                }
                else
                {
                    TakeDamage(healthBase/10);
                }
                //choi am thanh bom plane
                AudioManager.instance.Play("boomPlane");
            }
            else
            {
                TakeDamage(bullet.bulletCurrentDamage);
            }

            hitFx = ContentPoolable.Emit(hitEffect) as FxItem;

            if (typeEnemy == TypeEnemy.Boss_World)
            {
                hitFx.transform.position = bullet.transform.position;
                hitFx.transform.parent = this.hitPos;
            }
            else
            {
                hitFx.transform.position = this.hitPos.position;
                hitFx.transform.parent = this.hitPos;
            }

            if (bullet.isBazoka)
            {
                //choi am thanh bazoka
                AudioManager.instance.Play("bazoka2");
            }
        }
    }

    //==========SPAWN===========//
    public virtual void EnterSpawn()
    {
        enemyState = EnemyState.Spawn;
        anim.PlayAnim(AnimID.spawn, false, 1, false);
        capsuleCollider2D.enabled = false;
       // AudioAssistant.PlaySound(spawnSoundFX);
    }

    public virtual void UpdateSpawn()   //boss world override this function
    {
        timeInState += Time.deltaTime;
        if (timeInState >= anim.GetAnimData(AnimID.spawn, 1).duration)
        {
            ChangeState(new FollowCar(this));
        }
    }

    public virtual void ExitSpawn()
    {
        capsuleCollider2D.enabled = true;
    }

    //==========ATTACK===========//
    public virtual void EnterAttack()       //boss world override this function
    {
        enemyState = EnemyState.Attack;
        anim.PlayAnim(AnimID.attack_1, true, 1, false);
        // AudioAssistant.PlaySound(attackSoundFx);
     //   AudioManager.instance.Play(attackSoundFx);
    }

    public virtual void UpdateAttack()      //boss world override this function
    {
        targetComeTo = new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z);
        float dis = Vector3.Distance(transform.position, targetComeTo);
        if (dis >= distanceAttack + 5f)
        {
            ChangeState(new FollowCar(this));
        }
    }

    public virtual void ExitAttack()
    {

    }


    //==========FLY TO HIT CAR===========//
    public virtual void EnterFlyHitCar()
    {
        enemyState = EnemyState.FlyToHitCar;
        anim.PlayAnim(AnimID.run, true, 1, false);
        speed += 12f;
    }

    public virtual void UpdateFlyHitCar()
    {
        Vector3 posHit = new Vector3(target.transform.position.x, target.transform.position.y + 4f, target.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, posHit, (speed + SpeedGrowingUp) * Time.deltaTime);
        float dis = Vector3.Distance(transform.position, posHit);
        if (dis <= 5f && transform.position.y < 8f)
        {
            ChangeState(new AttackState(this));
        }
    }

    public virtual void ExitFlyHitCar()
    {
        speed -= 12f;
    }

    //==========DIE===========//
    public virtual void EnterDie()
    {
        enemyState = EnemyState.Die;
        anim.PlayAnim(AnimID.die, false, 1, false);

        capsuleCollider2D.enabled = false;
        healthBar.gameObject.SetActive(false);

        //coin
        //AudioAssistant.PlaySound("enemydie3");
        //AudioManager.instance.Play("enemydie3");
        CoinFollower coin = ContentPoolable.Emit(ItemID.coin_follow_1) as CoinFollower;
        coin.transform.position = this.transform.position;

        pool.WaitToKill(1);
        GameEvent.OnEnemyDie.Invoke(this);

        if (GlobalData.gameMode == GameMode.CollectFuel)
        {
            GoldFxEnemyDie();
        }
    }

    public virtual void GoldFxEnemyDie()
    {
        FxItem fxcoin = ContentPoolable.Emit(ItemID.FxGoldEnemyDie) as FxItem;
        fxcoin.transform.position = this.transform.position;
    }


    public virtual void UpdateDie()
    {
    }

    public virtual void ExitDie()
    {
    }

    //==========WIN===========//
    public virtual void EnterWin()
    {
        enemyState = EnemyState.Win;
        anim.PlayAnim(AnimID.victory, true, 1, false);
    }

    public virtual void UpdateWin()
    {

    }

    public virtual void ExitWin()
    {

    }

    //==========FOLLOW CAR===========//
    public virtual void EnterFollowCar()
    {
        enemyState = EnemyState.FollowCar;
        speed = speedBase + Random.Range(0, 2f);
        anim.PlayAnim(AnimID.run, true, 1.5f, false);
        capsuleCollider2D.enabled = true;
    }

    public virtual void UpdateFollowCar()
    {
        targetComeTo = new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z);
        float dis = Vector3.Distance(transform.position, targetComeTo);
        if (dis <= 10)
        {
            targetComeTo = target.transform.position;
        }

        if (SpeedGrowingUp < 3 + RandomSpeed)
        {
            SpeedGrowingUp += Time.deltaTime / 2;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetComeTo, (speed + SpeedGrowingUp) * Time.deltaTime);

        if (dis <= distanceAttack && transform.position.y < 8f)
        {
            ChangeState(new AttackState(this));
        }
    }

    public virtual void ExitFollowCar()
    {

    }

    //==========FLY RANDOM===========//
    private int randomPosOffset;
    private float timeInRandomMove;

    public virtual void EnterFlyRandom()
    {
        enemyState = EnemyState.FlyRandom;
        randomPosOffset = Random.Range(0, 5);
        timeInRandomMove = Random.Range(3f, 5f);
        this.transform.DOLocalMoveY(Random.Range(2f, 9f), 2f);
        anim.PlayAnim(AnimID.run, true, 1, false);
    }

    public virtual void UpdateFlyRandom()
    {
        targetComeTo = new Vector3(target.transform.position.x, target.transform.position.y + 4 + randomPosOffset, target.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetComeTo, (speed + SpeedGrowingUp) * Time.deltaTime);

        timeInState += Time.deltaTime;
        if (timeInState >= timeInRandomMove)
        {
            ChangeState(new FollowCar(this));
        }
    }

    public virtual void ExitFlyRandom()
    {

    }

    //==========ATTACK100===========//
    public virtual void EnterAttack100()
    {
        enemyState = EnemyState.Attack100;
        anim.PlayAnim(AnimID.attack_1, true, 1, false);
    }

    public virtual void UpdateAttack100()
    {
        timeInState += Time.deltaTime;
        if (timeInState >= (anim.GetAnimData(AnimID.attack_1).duration / anim.TimeScale))
        {
            ChangeState(new FreeState(this));
        }
    }

    public virtual void ExitAttack100()
    {

    }

    //==========ATTACK50===========//
    public virtual void EnterAttack50()
    {
        enemyState = EnemyState.Attack50;
    }

    public virtual void UpdateAttack50()
    {
        timeInState += Time.deltaTime;
    }

    public virtual void ExitAttack50()
    {

    }

    //==========FREE===========//
    public virtual void EnterFree()
    {
        enemyState = EnemyState.Free;
        anim.PlayAnim(AnimID.idle, true, 1, false);
        capsuleCollider2D.enabled = true;
    }

    public virtual void UpdateFree()
    {
        timeInState += Time.deltaTime;
        if (timeInState >= 2f && GameManager.Instance.gameState == GameState.Playing)
        {
            ChangeState(new AttackState(this));
        }
    }

    public virtual void ExitFree()
    {

    }



    //==========LEAVE THE CAR===========//
    public virtual void EnterLeaveTheCar()
    {
        enemyState = EnemyState.LeaveTheCar;
        anim.PlayAnim(AnimID.run, true, 1.5f, false);
        capsuleCollider2D.enabled = true;
        if (transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        speed = speedBase * 0.8f;
    }

    public virtual void UpdateLeaveTheCar()   //boss world override this function
    {
        Vector3 pos = new Vector3(transform.position.x + 5f, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);
        float dis = Vector3.Distance(this.transform.position, target.transform.position);
        if (dis < 10f)
        {
            speed = target.MaxSpeed;
        }
    }

    public virtual void ExitLeaveTheCar()
    {
    }
}

public enum TypeEnemy
{
    //===NORMAL MODE===/
    Enemy_Basic,
    Enemy_Tank,
    Enemy_Fly,
    Enemy_Motobike,
    Boss_Normal,

    //===BOSS WORLD MODE===//
    Boss_World,
}

public enum EnemyState
{
    //=========COMMON==========//
    Spawn,
    Attack,
    Die,
    Win,

    //=======NORMAL MODE=======//
    FollowCar,
    FlyRandom,
    FlyToHitCar,

    //========BOSS WORLD MODE========//
    Attack100,
    Attack50,
    Free,

    //========BOSS WORLD MODE========//
    LeaveTheCar

}
