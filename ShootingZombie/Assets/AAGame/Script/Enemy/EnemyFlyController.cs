using AA_Game;
using UnityEngine;
using Yurowm.GameCore;
using DG.Tweening;

public class EnemyFlyController : EnemyBase
{
    public Transform bulletPos;
    //public GameObject posAimCar;

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

    public override void EnterAttack()
    {
        base.EnterAttack();
        speed = target.MaxSpeed - SpeedGrowingUp;    
    }

    public override void UpdateAttack()
    {
        base.UpdateAttack();
        transform.position = Vector3.MoveTowards(transform.position, targetComeTo, (speed + SpeedGrowingUp) * Time.deltaTime);

        if (target != null)
        {
            //posAimCar.transform.position = target.carPos.position;
        }

        timeInState += Time.deltaTime;
        if (timeInState >= anim.GetAnimData(AnimID.attack_1).duration / anim.TimeScale)
        {
            ChangeState(new FlyRandomState(this));
        }
    }


    public override void EnterSpawn()
    {
        enemyState = EnemyState.Spawn;
        anim.PlayAnim(AnimID.run, false, 1, false);
        capsuleCollider2D.enabled = false;
        transform.DOMoveY(Random.Range(4, 8), 1f).OnComplete(() =>
        {
            if(enemyState != EnemyState.Win && enemyState != EnemyState.Die)
            {
                ChangeState(new FollowCar(this));
            }
        });
    }

    public override void UpdateSpawn()
    {
        targetComeTo = new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetComeTo, (speed + SpeedGrowingUp) * Time.deltaTime);
    }

    public override void HandleEvent(string eventName)
    {
        if (eventName == "attack_tracking")
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        BulletEnemyFly bullet = ContentPoolable.Emit(ItemID.bullet_enemy_fly) as BulletEnemyFly;
        bullet.transform.parent = null;
        bullet.transform.position = bulletPos.position;
        bullet.transform.parent = target.transform;
        bullet.Moving();

        Item flash = ContentPoolable.Emit(ItemID.gun_flash_4) as Item;
        flash.transform.localScale = new Vector3(-1, 1, 1);
        flash.transform.parent = GameManager.Instance.trainManager.transform;
        flash.transform.position = new Vector3(bulletPos.position.x, bulletPos.position.y, bulletPos.position.z);
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

    public override void EnterDie()
    {
        base.EnterDie();
        //coin
        ///AudioAssistant.PlaySound("enemydie1");
      //  AudioManager.instance.Play("enemydie1");
        FxItem fxDie = ContentPoolable.Emit(this.fxDie) as FxItem;
        fxDie.transform.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
        fxDie.transform.localScale = new Vector3(2, 2, 2);
    }
}
