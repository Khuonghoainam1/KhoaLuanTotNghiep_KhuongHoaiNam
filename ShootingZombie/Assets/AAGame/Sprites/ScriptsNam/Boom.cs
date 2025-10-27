using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AA_Game;
public class Boom : Item
{
    [SerializeField]
    private Rigidbody2D boomRB;
    private float boomSpeed;
    private float bulletCurrentDamage= 20;
    public ItemID hitEffect;
    private float bulletHealth;
    private void OnEnable()
    {
        
    }
    public void BoomKill()
    {
        bulletHealth -= 10;
        if (bulletHealth <= 0)
        {
            this.Kill();
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.CompareTag("Enemy"))
        //{

        //    EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();

        //    if (enemy.enemyState == EnemyState.Die || enemy.enemyState == EnemyState.Victory)
        //    {
        //        return;
        //    }
        //    enemy.damageGiven = this.bulletCurrentDamage;
        //    enemy.hitEffect = this.hitEffect;
        //    enemy.HitTrigger();
        //    BoomKill();
        //}
    }
}

