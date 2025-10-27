using System.Collections.Generic;
using UnityEngine;

public class BulletPlayer : BulletBase
{
    public override void OnEnable()
    {
        base.OnEnable();
        bulletHealth = 10;
        bounceColliders = new Collider2D[5];

        //kiem tra crit
        if(Random.Range(1,101) < User.Instance.Car.crit + User.Instance.UserPlayerUsing.crit)
        {
            //crit
            // hitEffect = hitEffectCrit;
            bulletCurrentDamage = 2 * bulletCurrentDamage;
        }
    }


    float bounceRange = 100f;
    Collider2D[] bounceColliders;
    Collider2D nearestCollider;
    Collider2D bounceCollider;
    List<Collider2D> bouncedColliders = new List<Collider2D>();
    Vector3 Direction;
    public LayerMask bounceTargetLayer;

    public void BounceToTarget()
    {
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, bounceRange, bounceColliders, bounceTargetLayer);
        float minDistance = 1000000f;
        Vector3 distanceVec = Vector3.zero;
        float distance;
        nearestCollider = null;

        for (int i = 0; i < count; i++)
        {
            bounceCollider = bounceColliders[i];
            if (bouncedColliders.Contains(bounceCollider))
            {
                continue;
            }

            distanceVec = bounceCollider.gameObject.transform.position - transform.position;
            distance = distanceVec.magnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestCollider = bounceCollider;
            }
        }

        if (nearestCollider != null)
        {
            Direction = distanceVec.normalized;
            bulletRB.velocity = Vector2.zero;
            this.transform.right = Direction;
            bulletRB.AddForce(transform.right * bulletSpeed, ForceMode2D.Impulse);
        }
        else
        {
            this.Kill();
        }
    }

    public override void AddForce()
    {
        base.AddForce();
        BoosterManager booster = BoosterManager.instance;
        if (booster.listBoost.Contains(NameBooster.PercentDamage10))
        {
            bulletCurrentDamage += bulletDamageBase * 0.1f;
        }

        if (booster.listBoost.Contains(NameBooster.PercentDamage15))
        {
            bulletCurrentDamage += bulletDamageBase * 0.15f;
        }

        if (booster.listBoost.Contains(NameBooster.Strength))
        {
            bulletCurrentDamage += bulletDamageBase * 0.1f;
        }

        if (booster.listBoost.Contains(NameBooster.BulletBounc))
        {
            bulletHealth = bulletHealthBase;
        }

        if (GlobalData.gameMode == GameMode.CollectFuel)
        {
            transform.parent = GameManager.Instance.trainManager.transform;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (BoosterManager.instance.listBoost.Contains(NameBooster.BulletBounc))
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                bouncedColliders.Add(collision);
                BounceToTarget();
            }
        }
    }
}
