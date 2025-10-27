using System.Collections;
using UnityEngine;
using DG.Tweening;

public class BossSkibidiToiletController : EnemyBase
{
    private int numberAttackNormal = 0;
    public GameObject[] lazer;
    public float mauNoiTai;

    public override void OnEnable()
    {
        base.OnEnable();
        healthBase += mauNoiTai;
        ChangeState(new FreeState(this));
    }

    public override void Start()
    {
        base.Start();
        transform.position = new Vector3(15, -10, 0);
    }

    public override void HandleEvent(string eventName)
    {
        if (eventName == "hit")
        {
            target.damageGiven = damage;
            //if (target.carState == CarState.Hit)
            //{
            //    target.GetHitBullet();
            //}
            //else
            //{
            //    target.GetHit();
            //}

            target.GetHitBullet();
        }
    }

    public override void OnRevive()    //boss world override this function
    {
        ChangeState(new FreeState(this));
    }


    public override void EnterAttack()
    {
        enemyState = EnemyState.Attack;
        if (health > healthBase / 2)
        {
            ChangeState(new Attack100HealthState(this));
        }
        else
        {
            ChangeState(new Attack50HealthState(this));
        }
    }

    /// <summary>
    /// Clear base code
    /// </summary>
    public override void UpdateAttack()
    {
    }


    public override void EnterAttack100()
    {
        base.EnterAttack100();
        numberAttackNormal += 1;
    }

    public override void EnterAttack50()
    {
        base.EnterAttack50();
        if (numberAttackNormal >= 2)
        {
            numberAttackNormal = 0;
            transform.DOJump(new Vector3(20, -10, 0), 5, 1, 1).OnComplete(() =>
            {
                anim.PlayAnim(AnimID.attack_2, true, 1, false);
                lazer[1].SetActive(true);
                lazer[0].SetActive(true);
            });
        }
        else
        {
            ChangeState(new Attack100HealthState(this));
        }
    }

    public override void UpdateAttack50()
    {
        base.UpdateAttack50();
        if (timeInState >= 3f)
        {
            ChangeState(new FreeState(this));
        }
    }

    public override void ExitAttack50()
    {
        base.ExitAttack50();
        lazer[0].SetActive(false);
        lazer[1].SetActive(false);
        if (transform.position == new Vector3(20, -10, 0))
        {
            transform.DOJump(new Vector3(15, -10, 0), 5, 1, 1);
        }
    }
}
