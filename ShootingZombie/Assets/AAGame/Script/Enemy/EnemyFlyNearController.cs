using DG.Tweening;
using UnityEngine;
using Yurowm.GameCore;

public class EnemyFlyNearController : EnemyBase
{
    public override void OnEnable()
    {
        base.OnEnable();
        attackSoundFx = "enemyatkfly";
        if (GlobalData.gameMode == GameMode.CollectFuel)
        {
            ChangeState(new LeaveTheCarState(this));
        }
        else
        {
            ChangeState(new SpawnState(this));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public override void EnterSpawn()
    {
        enemyState = EnemyState.Spawn;
        anim.PlayAnim(AnimID.run, false, 1, false);
        capsuleCollider2D.enabled = false;
        transform.DOMoveY(Random.Range(4, 8), 1f).OnComplete(() =>
        {
            if (enemyState != EnemyState.Win && enemyState != EnemyState.Die)
            {
                ChangeState(new FollowCar(this));
            }
        });
    }

    /// <summary>
    /// 
    /// </summary>
    public override void EnterAttack()
    {
        base.EnterAttack();
        speed = target.MaxSpeed - SpeedGrowingUp;
    }

    /// <summary>
    /// 
    /// </summary>
    public override void UpdateAttack()
    {
        base.UpdateAttack();
        transform.position = Vector3.MoveTowards(transform.position, targetComeTo, (speed + SpeedGrowingUp) * Time.deltaTime);

        timeInState += Time.deltaTime;
        if (timeInState >= anim.GetAnimData(AnimID.attack_1).duration / anim.TimeScale)
        {
            ChangeState(new FlyRandomState(this));
        }
    }

    public override void ExitAttack()
    {
        speed -= 5f;
    }

    /// <summary>
    /// 
    /// </summary>
    public override void UpdateSpawn()
    {
        targetComeTo = new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetComeTo, (speed + SpeedGrowingUp) * Time.deltaTime);
    }

    public override void UpdateLeaveTheCar()
    {
        Vector3 pos = new Vector3(transform.position.x + 5f, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, pos, speed * Time.deltaTime);
        float dis = Vector3.Distance(this.transform.position, target.transform.position);
        if (dis < 20f)
        {
            speed = target.MaxSpeed;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public override void EnterDie()
    {
        base.EnterDie();
       // AudioAssistant.PlaySound("enemydie1");
        FxItem fxDie = ContentPoolable.Emit(this.fxDie) as FxItem;
        fxDie.transform.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
        fxDie.transform.localScale = new Vector3(2, 2, 2);
    }

    public override void UpdateFollowCar()
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
            ChangeState(new FlyToHitCar(this));
        }
    }
}
