using AA_Game;
using UnityEngine;
using Yurowm.GameCore;

public class BulletEnemy : Item
{
    [SerializeField]
    private Rigidbody2D bulletRB;

    public float bulletSpeed = 40;
    public float bulletDamageBase = 5;
    [HideInInspector]
    public float bulletCurrentDamage = 5;
    public ItemID hitEffect;

    private void OnEnable()
    {
        bulletCurrentDamage = bulletDamageBase;
    }

    public void AddForce()
    {
        bulletRB.AddForce(transform.right * bulletSpeed, ForceMode2D.Impulse);
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
            flash.transform.position = new Vector3(this.transform.position.x, this.transform.position.y+Random.Range(-1.1f,1.1f), this.transform.position.z);

            this.Kill();
        }
    }
}
