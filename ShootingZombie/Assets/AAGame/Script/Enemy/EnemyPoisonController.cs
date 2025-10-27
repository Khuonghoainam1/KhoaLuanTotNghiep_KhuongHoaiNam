using UnityEngine;
using Yurowm.GameCore;

public class EnemyPoisonController : EnemyBase
{
    public Transform bulletPos;

    public override void OnEnable()
    {
        base.OnEnable();
        ChangeState(new SpawnState(this));
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
        if (eventName == "attack")
        {
            ThrowPoison();
        }
    }

    public void ThrowPoison()
    {
        BulletEnemyFly bullet = ContentPoolable.Emit(ItemID.bullet_enemy_poison) as BulletEnemyFly;
        bullet.transform.parent = null;
        bullet.transform.position = bulletPos.position;
        bullet.transform.parent = target.transform;
        bullet.Moving();
    }
}
