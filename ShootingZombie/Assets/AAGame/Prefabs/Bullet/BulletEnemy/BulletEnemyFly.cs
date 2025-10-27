using AA_Game;
using DG.Tweening;
using UnityEngine;
using Yurowm.GameCore;

public class BulletEnemyFly : Item
{
    public float bulletDamageBase = 5;
    [HideInInspector]
    public float bulletCurrentDamage = 5;
    public ItemID hitEffect;
    public float health;

    public string BulletName;


    private void OnEnable()
    {
        health = 15;
        GameEvent.OnPlayerLose.AddListener(() => this.Kill());
    }

    private void Update()
    {
        if (BulletName == "Bullet_Enemy_Poison")
        {
            this.transform.Rotate(0, 0, 10);
        }
    }

    public void Moving()
    {
        if (BulletName == "Bullet_Enemy_Fly")
        {
            this.transform.DOLocalMove(Vector3.zero, 5f);
        }
        else if(BulletName == "Bullet_Enemy_Poison")
        {
            this.transform.DOLocalJump(Vector3.zero,10f,1,1.5f);
        }
        else if(BulletName == "Bullet_Enemy_Mud")
        {
            this.transform.DOLocalMove(new Vector3(0,2,0), 1.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TrainManager car = collision.gameObject.GetComponent<TrainManager>();

            if (car.carState == CarState.Die)
            {
                return;
            }

            car.damageGiven = this.bulletCurrentDamage;
            //car.hitEffect = this.hitEffect;
            car.GetHitBullet();

            Item flash = ContentPoolable.Emit(hitEffect) as Item;
            flash.transform.parent = GameManager.Instance.trainManager.transform;
            flash.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + Random.Range(-1.1f, 1.1f), this.transform.position.z);
            this.transform.DOComplete();
            this.Kill();
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (collision.gameObject.GetComponent<BulletBase>().isBulletOfPlayer)
            {
                collision.gameObject.GetComponent<BulletBase>().Kill();

                health -= 5;
                if (health <= 0)
                {
                    Item flash = ContentPoolable.Emit(ItemID.BulletDestroy) as Item;
                    flash.transform.position = this.transform.position;
                    this.transform.DOComplete();
                    this.Kill();
                }
            }
        }
    }
}
