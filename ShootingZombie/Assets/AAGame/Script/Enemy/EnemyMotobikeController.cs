using AA_Game;
using UnityEngine;
using Yurowm.GameCore;

public class EnemyMotobikeController : EnemyBase
{
    public Transform bulletPos;
    public GameObject posAimCar;
    public GameObject fxSmokeRun;

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

        if (target != null)
        {
            posAimCar.transform.position = target.carPos.position;
        }
    }

    public override void HandleEvent(string eventName)
    {
        if (eventName == "shoot")
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        BulletEnemy bullet = ContentPoolable.Emit(ItemID.bullet_enemy_1) as BulletEnemy;
        bullet.transform.position = bulletPos.position;
        Vector3 dir = -this.bulletPos.position + target.carPos.position;
        bullet.transform.right = dir;
        bullet.AddForce();

        Item flash = ContentPoolable.Emit(ItemID.gun_flash_4) as Item;
        flash.transform.localScale = new Vector3(-1, 1, 1);
        flash.transform.parent = GameManager.Instance.trainManager.transform;
        flash.transform.position = new Vector3(bulletPos.position.x, bulletPos.position.y, bulletPos.position.z);
    }

    public override void EnterFollowCar()
    {
        base.EnterFollowCar();
        fxSmokeRun.SetActive(true);
    }

    public override void EnterDie()
    {
        base.EnterDie();
        fxSmokeRun.SetActive(false);
        //coin
        //  AudioAssistant.PlaySound("enemydie2");
       // AudioManager.instance.Play("enemydie2");
        FxItem fxDie = ContentPoolable.Emit(this.fxDie) as FxItem;
        fxDie.transform.position = this.transform.position;
    }

    public override void EnterWin()
    {
        base.EnterWin();
        fxSmokeRun.SetActive(false);
    }
}
