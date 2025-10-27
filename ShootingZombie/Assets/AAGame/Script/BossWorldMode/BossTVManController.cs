using UnityEngine;
using Yurowm.GameCore;

public class BossTVManController : EnemyBase
{
    private float timeHoiSongAm;
    private bool isPowerUp;
    public Transform posSongAm;
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
            if (enemyState == EnemyState.Attack100)
            {
                target.damageGiven = damage;
                target.GetHitBullet();
            }
            else if (enemyState == EnemyState.Attack50)
            {
                //spawn song am
                SpawnSongAm();
            }
        }
    }

    public override void OnRevive() 
    {
        ChangeState(new FreeState(this));
    }

    public override void EnterFree()
    {
        base.EnterFree();
        if (isPowerUp == false)
        {
            if (health < healthBase / 3)
            {
                isPowerUp = true;
                health += 0.2f * healthBase;
                healthBar.red.transform.localScale = new Vector3(health / healthBase, healthBar.red.transform.localScale.y, healthBar.red.transform.localScale.z);
                damage += 0.2f * damage;

                //fx power up
            }
        }
    }

    public override void EnterAttack()
    {
        enemyState = EnemyState.Attack;
        ChangeState(new Attack100HealthState(this));
    }

    /// <summary>
    /// Clear base code
    /// </summary>
    public override void Update()
    {
        base.Update();
        timeHoiSongAm += Time.deltaTime;
        if (health < healthBase / 2)
        {
            if (timeHoiSongAm >= 10 && enemyState==EnemyState.Free)
            {
                timeHoiSongAm = 0;
                ChangeState(new Attack50HealthState(this));
            }
        }
    }

    public override void EnterAttack50()
    {
        enemyState = EnemyState.Attack50;
        anim.PlayAnim(AnimID.attack_2, true, 1, false);
    }

    public override void UpdateAttack50()
    {
        timeInState += Time.deltaTime;
        if(timeInState >= (anim.GetAnimData(AnimID.attack_1).duration / anim.TimeScale))
        {
            ChangeState(new FreeState(this));
        }
    }

    public void SpawnSongAm()
    {
        IceBulletController songAm = ContentPoolable.Emit(ItemID.ice_bullet) as IceBulletController;
        songAm.transform.position = this.posSongAm.transform.position;
        songAm.AddForce();
    }
}
