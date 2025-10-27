using AA_Game;
using UnityEngine;
using Yurowm.GameCore;

public abstract class CharacterBase : MonoBehaviour
{
    public TypeCharacter typeCharacter;
    public CharacterAnim anim;
    public float timeInstate;
    public CharacterState characterState;
    protected IState currentState;
    public EnemyBase target;
    public GameObject path;
    public Transform rocketPos;
    public float botDetectRange;
    public Transform posMoveToCar;
    public Transform posIntherCar;
    public GameObject posAimZombie;
    public BossWorldBase targetBoss;
    public float randomPosToShoot;

    public abstract void ChangeState(IState newState);

    public virtual void Start()
    {
        if (GlobalData.gameMode == GameMode.CollectFuel)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1,transform.localScale.y,transform.localScale.z);
        }
    }

    private void Update()
    {
        currentState.UpdateState();
    }
    private void OnDestroy()
    {
        currentState.ExitState();
    }

    public abstract void Spawn();
    public abstract void Free();
    public abstract void CharStart();
    public abstract void MovingToCar();
    public abstract void CharJumpIntoTheCarState();
    public abstract void OnGetInTheCar();
    public abstract void Shoot();
    public abstract void FindingTarget();
    public abstract void Hit();
    public abstract void Die();
    public abstract void Victory();
    public abstract void Reload();
    public abstract void OnWin();
    public abstract void OnLose();

    public virtual void EnterStun()
    {
        anim.TimeScale = 0;
        characterState = CharacterState.Stun;
        Item iceHit = ContentPoolable.Emit(ItemID.ice_hit) as Item;
        iceHit.transform.position = this.transform.position;
    }
    public virtual void UpdateStun()
    {
        timeInstate += Time.deltaTime;
        if(timeInstate >= 3f)
        {
            ChangeState(new CharFindingTargetState(this));
        }
    }
    public virtual void ExitStun()
    {
        anim.TimeScale = 1;
    }


    //========MODE BOSS=========//
    public virtual void EnterShootingBoss()
    {
        characterState = CharacterState.ShootingBoss;
        anim.PlayAnim(AnimID.shoot, true, 1, false);
    }

    public virtual void UpdateShootingBoss()
    {
        if (targetBoss == null || targetBoss.bossState == BossState.Die)
        {
            ChangeState(new CharFindingTargetState(this));
        }

        //aim car
        if (targetBoss != null && posAimZombie != null)
        {
            if (target.typeEnemy == TypeEnemy.Boss_World)
            {
                posAimZombie.transform.position = new Vector3(targetBoss.transform.position.x, targetBoss.transform.position.y + randomPosToShoot, targetBoss.transform.position.z);
            }
            else
            {
                posAimZombie.transform.position = targetBoss.transform.position;
            }
        }
    }

    public virtual void ExitShootingBoss()
    {

    }
}

public enum TypeCharacter
{
    Piston,
    AK,
    Bazoka,
    PlayerMain,
}

public enum CharacterState
{
    Spawn,
    Start,
    MovingToCar,
    JumpIntoTheCar,
    Free,
    FindingTarget,
    Shoot,
    Reload,
    Hit,
    Die,
    Victory,
    ShootingBoss,
    Stun,
}
