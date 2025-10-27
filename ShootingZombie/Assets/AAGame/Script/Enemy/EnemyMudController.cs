using System.Collections;
using UnityEngine;
using Yurowm.GameCore;

public class EnemyMudController : EnemyBase
{
    public Transform bulletPos;

    public override void OnEnable()
    {
        base.OnEnable();
        ChangeState(new FollowCar(this));
    }

    public override void EnterAttack()
    {
        base.EnterAttack();
        speed = speedBase / 2;
    }


    public override void UpdateAttack()
    {
        base.UpdateAttack();
        transform.position = Vector3.MoveTowards(transform.position, targetComeTo, (speed + SpeedGrowingUp) * Time.deltaTime);
    }

    public override void HandleEvent(string eventName)
    {
        if (eventName == "attack_tracking")
        {
            ThrowPoison();
        }
    }

    public void ThrowPoison()
    {
        BulletEnemyFly bullet = ContentPoolable.Emit(ItemID.bullett_mud_poison) as BulletEnemyFly;
        bullet.transform.parent = null;
        bullet.gameObject.SetActive(false);
        bullet.transform.position = bulletPos.position;
        bullet.gameObject.SetActive(true);
        bullet.transform.parent = target.transform;
        bullet.Moving();
    }
}
