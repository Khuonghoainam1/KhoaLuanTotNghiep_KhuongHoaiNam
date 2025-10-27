using AA_Game;
using UnityEngine;
using Yurowm.GameCore;

public class RocketController : BulletBase
{
    public Vector3 offset;
    public float force;

    public override void AddForce()
    {
        bulletCurrentDamage = User.Instance.UserBot3Using.damage;
        bulletRB.AddForce(transform.right * force, ForceMode2D.Impulse);
    }

    public override void Update()
    {
        transform.rotation = VectorToRotationSlerp(transform.rotation, bulletRB.velocity, 20);

        if (GlobalData.gameMode == GameMode.BossWorld)
        {
            if (this.transform.position.y < -12f)
            {
                Item hitBazoka = ContentPoolable.Emit(ItemID.enemy_hit_bazoka) as Item;
                hitBazoka.transform.position = this.transform.position;
                this.Kill();
                //am thanh bazoka
                 AudioManager.instance.Play("bazoka2");
            }
        }
        else
        {
            if (this.transform.position.y < -6.5f)
            {
                Item hitBazoka = ContentPoolable.Emit(ItemID.enemy_hit_bazoka) as Item;
                hitBazoka.transform.position = this.transform.position;
                this.Kill();
                AudioManager.instance.Play("bazoka2");
                //am thanh bazoka
            }
        }

    }

    public void SetDamage(TypeBot typeBot)
    {
        switch (typeBot)
        {
            case TypeBot.Pistol:
                bulletDamageBase = User.Instance.UserBot1Using.damage;
                bulletCurrentDamage = bulletDamageBase;
                break;
            case TypeBot.Riffle:
                bulletDamageBase = User.Instance.UserBot2Using.damage;
                bulletCurrentDamage = bulletDamageBase;
                break;
            case TypeBot.Bazoka:
                bulletDamageBase = User.Instance.UserBot3Using.damage;
                bulletCurrentDamage = bulletDamageBase;
                break;
        }
    }

    public static Quaternion VectorToRotationSlerp(Quaternion srcRotation, Vector3 targetPos, float slerpSpeed)
    {
        float angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        Quaternion rotationTarget = Quaternion.AngleAxis(angle, Vector3.forward);
        return Quaternion.Slerp(srcRotation, rotationTarget, Time.deltaTime * slerpSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.CompareTag("Enemy"))
        //{
        //    if (SceneManager.GetActiveScene().name == SceneName.GameScene.ToString())
        //    {
        //        EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();

        //        if (enemy.enemyState == EnemyState.Die || enemy.enemyState == EnemyState.Victory)
        //        {
        //            return;
        //        }

        //        enemy.damageGiven = this.damage;
        //        enemy.hitEffect = this.hitEffect;
        //        enemy.HitTrigger();
        //        this.Kill();
        //    }
        //    else if (SceneManager.GetActiveScene().name == SceneName.BossWorldScene.ToString())
        //    {
        //        BossWorldBase boss = collision.gameObject.GetComponent<BossWorldBase>();
        //        if (boss.bossState == BossState.Die || boss.bossState == BossState.Win)
        //        {
        //            return;
        //        }
        //        boss.Hit(this.damage);
        //        FxItem hitEff = ContentPoolable.Emit(hitEffect) as FxItem;
        //        hitEff.transform.position = this.transform.position;
        //        hitEff.transform.parent = boss.transform;
        //        this.Kill();
        //    }
        //}
    }
}
