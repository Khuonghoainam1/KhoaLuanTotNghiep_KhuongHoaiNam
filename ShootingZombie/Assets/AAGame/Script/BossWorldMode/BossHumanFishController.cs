using System.Collections;
using UnityEngine;
using Yurowm.GameCore;

public class BossHumanFishController : EnemyBase
{
    private float timeSpawnEnemyRunner;
    private int indexBoss;
    private float healthBaseSmallBoss;
    private float damageSmallBoss;
    public float mauNoiTai;

    public override void OnEnable()
    {
        base.OnEnable();
        healthBase += mauNoiTai;
        ChangeState(new FreeState(this));
        healthBaseSmallBoss = healthBase / 3;
        damageSmallBoss = damage / 3;
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
            target.GetHitBullet();
        }
    }

    public override void OnRevive()    //boss world override this function
    {
        ChangeState(new FreeState(this));
    }

    public override void EnterDie()
    {
        enemyState = EnemyState.Die;
        anim.PlayAnim(AnimID.die, false, 1, false);

        capsuleCollider2D.enabled = false;
        healthBar.gameObject.SetActive(false);

        //coin
        CoinFollower coin = ContentPoolable.Emit(ItemID.coin_follow_1) as CoinFollower;
        coin.transform.position = this.transform.position;

        if (indexBoss < 3)
        {
            StartCoroutine(SmallBoss());
        }
        else
        {
            pool.WaitToKill(1);
            GameEvent.OnEnemyDie.Invoke(this);
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
        timeSpawnEnemyRunner += Time.deltaTime;
        if (health < healthBase / 2)
        {
            if (timeSpawnEnemyRunner >= 10)
            {
                timeSpawnEnemyRunner = 0;
                StartCoroutine(SpawnRunnerEnemy());
            }
        }
    }

    public override void EnterAttack50()
    {
        base.EnterAttack50();
        anim.PlayAnim(AnimID.idle, true, 1, false);
    }

    IEnumerator SpawnRunnerEnemy()
    {
        int enemyAmount = Random.Range(3, 6);
        for (int i = 0; i < enemyAmount; i++)
        {
            yield return new WaitForSeconds(0.2f);
            if (enemyState == EnemyState.Die || enemyState == EnemyState.Win)
            {
                yield break;
            }
            EnemyPool enemy = ContentPoolable.Emit(ItemID.enemy_1) as EnemyPool;
            Vector3 pos = new Vector3(this.transform.position.x + Random.Range(2, 4), this.transform.position.y + Random.Range(-3f, 3f), 0);
            enemy.transform.position = pos;
            enemy.GetComponent<EnemyBase>().target = this.target;
            enemy.GetComponent<EnemyBase>().speedBase = 4f;
            GameManager.Instance.enemiesCurrentAmount += 1;
            GameManager.Instance.totalEnemyInLevel += 1;
            GameManager.Instance.listEnemy.Add(enemy.GetComponent<EnemyBase>());
        }
    }

    IEnumerator SmallBoss()
    {
        yield return new WaitForSeconds(0.98f);
        indexBoss += 1;
        healthBase = healthBaseSmallBoss;
        health = healthBase;
        damage = damageSmallBoss;
        healthBar.gameObject.SetActive(true);
        healthBar.red.transform.localScale = new Vector3(health / healthBase, healthBar.red.transform.localScale.y, healthBar.red.transform.localScale.z);
        transform.localScale = transform.localScale / 1.3f;
        ChangeState(new FreeState(this));
    }
}
