using AA_Game;
using UnityEngine;


public class BulletBase : Item
{
    [SerializeField]
    protected Rigidbody2D bulletRB;
    public float bulletSpeed = 40;
    public float bulletDamageBase = 5;
    public float bulletCurrentDamage = 5;
    public float bulletHealthBase;
    protected float bulletHealth = 10;  //default is 10
    public ItemID hitEffect;
    public bool isBulletOfPlayer;
    public TypeBot typeBot;

    public bool isBombPlane;
    public bool isBazoka;


    public virtual void OnEnable()
    {
        bulletCurrentDamage = bulletDamageBase;
    }


    public virtual void Update()
    {
        if (transform.position.x > GameManager.Instance.cam.transform.position.x + 20)
        {
            Kill();
        }
    }

    public virtual void AddForce()
    {
        bulletRB.AddForce(transform.right * bulletSpeed, ForceMode2D.Impulse);
    }

    public virtual void BulletHit()
    {
        bulletHealth -= 10;
        if (bulletHealth <= 0)
        {
            this.Kill();
        }
    }
}
