using System.Collections.Generic;
using UnityEngine;
using Yurowm.GameCore;

public class EnemyBasicController : EnemyBase
{
    

    public bool isDogEnemy;

    public override void OnEnable()
    {
        base.OnEnable();
        ChangeState(new SpawnState(this));

        
    }

    public override void EnterAttack()
    {
        if (typeEnemy == TypeEnemy.Enemy_Basic)
        {
            base.EnterAttack();
        }
        else if(typeEnemy == TypeEnemy.Enemy_Tank)
        {
            if (Random.Range(0, 2) == 1)
            {
                enemyState = EnemyState.Attack;
                anim.PlayAnim(AnimID.attack_1, true, 1, false);
            }
            else
            {
                enemyState = EnemyState.Attack;
                anim.PlayAnim(AnimID.attack_2, true, 1, false);
            }
        }
    }

    public override void UpdateFollowCar()
    {
        base.UpdateFollowCar();
        //float dis = Vector3.Distance(transform.position, targetComeTo);
        //if (dis <= 35 && User.Instance[ItemID.TutPlay] == 1)
        //{
        //    Time.timeScale = 0.2f;
        //    User.Instance[ItemID.TutBoosterVip] = 1;
        //    targetComeTo = target.transform.position;
        //    GameEvent.OnSetTrueTutVip.Invoke(NameBooster.Drone);
        //}
    }

    public override void GoldFxEnemyDie()
    {
        FxItem fxcoin = ContentPoolable.Emit(ItemID.FxGoldEnemyDie) as FxItem;
        if (typeEnemy == TypeEnemy.Enemy_Tank)
        {
            fxcoin.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 2f, this.transform.position.z);
        }
        else
        {
            fxcoin.transform.position = this.transform.position;
        }
    }

    public override void UpdateAttack()
    {
        if (!isDogEnemy && typeEnemy != TypeEnemy.Enemy_Tank)
        {
            base.UpdateAttack();
        }
        else
        {
            timeInState += Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, (speed/2) * Time.deltaTime);
            if (timeInState >= anim.GetAnimData(AnimID.attack_2, 1).duration)
            {
                ChangeState(new FollowCar(this));
            }
        }
    }
}
