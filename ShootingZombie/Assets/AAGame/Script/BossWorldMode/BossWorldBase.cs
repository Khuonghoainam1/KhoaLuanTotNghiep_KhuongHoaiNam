using UnityEngine;
using DG.Tweening;

public class BossWorldBase : MonoBehaviour
{
    public float healthBase;
    [HideInInspector]
    public float health;
    public float damageBase;
    [HideInInspector]
    public float damage;
    [HideInInspector]
    public float timeInState;
    [HideInInspector]
    public CharacterAnim anim;
    [HideInInspector]
    public EnemyPool pool;
    [HideInInspector]
    public HealthBarController healthBar;
    public ItemID hitFx;
    public float damageGiven;
    public TrainManager target;

    public BossState bossState;

    protected IState currentState;

    public virtual void OnEnable()
    {
        anim = GetComponent<CharacterAnim>();
        pool = GetComponent<EnemyPool>();
        healthBar = GetComponentInChildren<HealthBarController>();
        GameEvent.OnPlayerWin.AddListener(OnLose);
        GameEvent.OnPlayerLose.AddListener(OnWin);
        anim.OnHandleEvent.RemoveListener(HandleEvent);
        anim.OnHandleEvent.AddListener(HandleEvent);
        GameEvent.OnReviveGame.AddListener(PlayerRevive);
        health = healthBase;
        damage = damageBase;
        ChangeState(new BossSpawnState(this));
    }

    public virtual void Start()
    {
        this.gameObject.name = "BOSS----" + bossState.ToString();
        transform.position = new Vector3(13, -8, 0);
    }

    private void OnDisable()
    {
        GameEvent.OnPlayerWin.RemoveListener(OnLose);
        GameEvent.OnPlayerLose.RemoveListener(OnWin);
        GameEvent.OnReviveGame.RemoveListener(PlayerRevive);
    }

    public void OnWin()
    {
        ChangeState(new BossWinState(this));
    }

    public void OnLose()
    {

    }

    public void PlayerRevive()
    {
        ChangeState(new BossFreeState(this));
    }

    public void HandleEvent(string eventName)
    {
        if (eventName == "attack" || eventName == "hit")
        {
            target.damageGiven = damage;
            target.GetHitBullet();
            //if (target.carState == CarState.Hit)
            //{
            //    target.GetHitBullet();
            //}
            //else
            //{
            //    target.ChangeState(new CarHitState(target));
            //}
        }
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
        this.gameObject.name = "BOSS----" + bossState.ToString();
    }

    private void Update()
    {
        currentState.UpdateState();
    }

    private void OnDestroy()
    {
        currentState.ExitState();
    }

    public void Hit(float damageGiven)
    {
        if (health > 0)
        {
            health -= damageGiven;

            if (health <= 0)
            {
                health = 0;
                ChangeState(new BossDieState(this));
            }
            healthBar.red.transform.localScale = new Vector3(health / healthBase, healthBar.red.transform.localScale.y, healthBar.red.transform.localScale.z);
        }
    }

    //==========SPAWN STATE===========//

    public virtual void EnterSpawn()
    {
        bossState = BossState.Spawn;
        anim.PlayAnim(AnimID.idle, true, 1, false);
        this.gameObject.transform.position = new Vector3(13, -8, 0);
    }

    public virtual void UpdateSpawn()
    {
        timeInState += Time.deltaTime;
        if (timeInState >= anim.GetAnimData(AnimID.idle).duration / anim.TimeScale)
        {
            ChangeState(new BossFreeState(this));
        }
    }

    public virtual void ExitSpawn()
    {
        GameEvent.StartFightingWithBoss.Invoke();
    }

    //==========FREE STATE===========//

    public virtual void EnterFree()
    {
        bossState = BossState.Free;
        anim.PlayAnim(AnimID.idle, true, 1, false);
    }

    public virtual void UpdateFree()
    {
        timeInState += Time.deltaTime;
        if (timeInState >= 2f)
        {
            ChangeState(new BossAttackState(this));
        }
    }

    public virtual void ExitFree()
    {

    }

    //==========ATTACK STATE===========//

    public virtual void EnterAttack()
    {
        bossState = BossState.Attack;
        if (health > healthBase / 2)
        {
            ChangeState(new BossAttack100HealthState(this));
        }
        else
        {
            ChangeState(new BossAttack50HealthState(this));
        }
    }

    public virtual void UpdateAttack()
    {

    }

    public virtual void ExitAttack()
    {

    }


    //==========JUMP TO PLAYER STATE===========//

    public virtual void EnterJumpToPlayer()
    {
        bossState = BossState.JumpToPlayer;
        anim.PlayAnim(AnimID.jump, true, 1, false);
        transform.DOLocalJump(new Vector3(13, -8, 0), 5, 1, 1).OnComplete(() =>
        {
            ChangeState(new BossAttack100HealthState(this));
        });
    }

    public virtual void UpdateJumpToPlayer()
    {

    }

    public virtual void ExitJumpToPlayer()
    {

    }


    //==========JUMP BACK STATE===========//

    public virtual void EnterJumpBack()
    {
        bossState = BossState.JumpBack;
        anim.PlayAnim(AnimID.jump, true, 1, false);
        transform.DOLocalJump(new Vector3(20, -8, 0), 5, 1, 1).OnComplete(() =>
        {

        });
    }

    public virtual void UpdateJumpBack()
    {

    }

    public virtual void ExitJumpBack()
    {

    }


    //==========ATTACK 100 HP STATE===========//

    public virtual void EnterAttack100()
    {
        bossState = BossState.Attack100HP;
        anim.PlayAnim(AnimID.attack_1, true, 1, false);
    }

    public virtual void UpdateAttack100()
    {
        timeInState += Time.deltaTime;
        if (timeInState >= (anim.GetAnimData(AnimID.attack_1).duration / anim.TimeScale))
        {
            ChangeState(new BossFreeState(this));
        }
    }

    public virtual void ExitAttack100()
    {

    }


    //==========ATTACK 50 HP STATE===========//

    public virtual void EnterAttack50()
    {
        bossState = BossState.Attack50HP;
    }

    public virtual void UpdateAttack50()
    {
        timeInState += Time.deltaTime;
    }

    public virtual void ExitAttack50()
    {

    }



    //==========GET HIT STATE===========//

    public virtual void EnterGetHit()
    {
        bossState = BossState.GetHit;
    }

    public virtual void UpdateGetHit()
    {

    }

    public virtual void ExitGetHit()
    {

    }


    //==========DIE STATE===========//

    public virtual void EnterDie()
    {
        bossState = BossState.Die;
        anim.PlayAnim(AnimID.die, false, 1, false);
        healthBar.gameObject.SetActive(false);
        //BossWorldManager.instance.OnEnemyDie();
    }

    public virtual void UpdateDie()
    {

    }

    public virtual void ExitDie()
    {

    }


    //==========WIN STATE===========//

    public virtual void EnterWin()
    {
        bossState = BossState.Win;
        anim.PlayAnim(AnimID.victory, true, 1, false);
    }

    public virtual void UpdateWin()
    {

    }

    public virtual void ExitWin()
    {

    }
}

public enum BossState
{
    Spawn,
    Free,
    Attack,
    Attack100HP,
    Attack50HP,
    GetHit,
    Die,
    Win,
    JumpToPlayer,
    JumpBack,
}
