using UnityEngine;
using Yurowm.GameCore;

public class EnemyBombController : EnemyBase
{
    public override void OnEnable()
    {
        base.OnEnable();
        ChangeState(new SpawnState(this));
    }

    public override void UpdateFollowCar()
    {
        base.UpdateFollowCar();
        float dis = Vector3.Distance(transform.position, targetComeTo);


        if (dis <= 35 && User.Instance[ItemID.TutPlay] == 2)
        {
            Time.timeScale = 0.2f;
            User.Instance[ItemID.TutBoosterVip] = 3;
            targetComeTo = target.transform.position;
            GameEvent.OnSetTrueTutVip.Invoke(NameBooster.Plane);
        }
    }

    public override void HandleEvent(string eventName)
    {
        if (eventName == "attack")
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

            //fx bom no
            //choi am thanh enemy bomb
            AudioManager.instance.Play("ombom");
            FxItem hitEff = ContentPoolable.Emit(ItemID.enemy_hit_bazoka_1) as FxItem;
            hitEff.transform.position = new Vector3(target.transform.position.x-1f,target.transform.position.y+1f,target.transform.position.z);
            //Kill chinh minh
            ChangeState(new DieState(this));
        }
    }


    public override void EnterAttack()
    {
        base.EnterAttack();
        speed = speedBase * 0.6f;
    }


    public override void UpdateAttack()
    {
        base.UpdateAttack();
        transform.position = Vector3.MoveTowards(transform.position, targetComeTo, (speed + SpeedGrowingUp) * Time.deltaTime);
    }


    public override void EnterDie()
    {
        enemyState = EnemyState.Die;
        anim.PlayAnim(AnimID.die, false, 1, false);

        capsuleCollider2D.enabled = false;
        healthBar.gameObject.SetActive(false);

       // //coin
       // AudioAssistant.PlaySound("enemydie3");
        //AudioManager.instance.Play("enemydie3");
        CoinFollower coin = ContentPoolable.Emit(ItemID.coin_follow_1) as CoinFollower;
        coin.transform.position = this.transform.position;

        GameEvent.OnEnemyDie.Invoke(this);

        if (GlobalData.gameMode == GameMode.CollectFuel)
        {
            GoldFxEnemyDie();
        }

        pool.Kill();
    }
}
