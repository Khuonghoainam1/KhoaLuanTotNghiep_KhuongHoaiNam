using UnityEngine;

public class BossNormalController : EnemyBase
{
    public override void OnEnable()
    {
        base.OnEnable();
        ChangeState(new FollowCar(this));


        //level tut
        //if(User.Instance[ItemID.TutPlay] < 4)
        //{
        //    healthBase = healthBase / 2;
        //    health = healthBase;
        //    //healthBar.gameObject.SetActive(false);
        //   // healthBar.red.transform.localScale = new Vector3(health / healthBase, healthBar.red.transform.localScale.y, healthBar.red.transform.localScale.z);
        //}
    }

    public override void EnterFollowCar()
    {
        base.EnterFollowCar();
        anim.PlayAnim(AnimID.run, true, 1f, false);
    }
    public override void UpdateFollowCar()
    {
        base.UpdateFollowCar();
        float dis = Vector3.Distance(transform.position, targetComeTo);
        if (dis <= 10 && User.Instance[ItemID.TutPlay] == 3)
        {
            Time.timeScale = 0.2f;
            targetComeTo = target.transform.position;
            GameEvent.OnSetTrueTutVip.Invoke(NameBooster.Shield);
        }
    }
    public override void UpdateAttack()
    {
        targetComeTo = new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetComeTo, (speed + SpeedGrowingUp) / 2 * Time.deltaTime);
        timeInState += Time.deltaTime;
        if (timeInState >= anim.GetAnimData(AnimID.attack_1).duration / anim.TimeScale)
        {
            ChangeState(new FollowCar(this));
        }
    }

    public override void HandleEvent(string eventName)
    {
        if (eventName == "hit" || eventName == "attack_tracking")
        {
            target.damageGiven = damage;
            if (target.carState == CarState.Hit)
            {
                target.GetHitBullet();
            }
            else
            {
                target.ChangeState(new CarHitState(target));
            }
        }
    }
}
