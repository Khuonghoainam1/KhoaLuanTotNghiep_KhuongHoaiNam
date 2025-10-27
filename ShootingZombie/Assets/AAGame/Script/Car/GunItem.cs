using AA_Game;
using Spine.Unity;
using UnityEngine;
using Yurowm.GameCore;

public class GunItem : Item
{
    public Transform muzzleFlash;
    public ItemID bullet;
    Vector3 _randomSpreadDirection;
    public float speedShootBase;
    [HideInInspector]
    public float speedShoot;
    public Vector3 Spread;
    BoosterManager booster;
    public GameObject gunFlash;




    public float gunDamage;
    public float gunDamageBase;

    public Transform posAimTarget;

    public SkeletonAnimation anim;
    Spine.AnimationState animState;
    public Spine.Skeleton skeleton;

    private void Awake()
    {
        anim = GetComponent<SkeletonAnimation>();
        animState = anim.AnimationState;
        skeleton = anim.skeleton;
        skeleton.SetSkin(User.Instance.Car.gun.ToString());
        skeleton.SetSlotsToSetupPose();
    }


    private void Start()
    {
        booster = BoosterManager.instance;
        speedShoot = speedShootBase;
        GameEvent.OnSelectBooster.AddListener(CheckingSpeedShoot);
        GameEvent.OnUnlockTalent.AddListener(OnUnlockTalent);

        GameEvent.OnCarLevelUp.RemoveListener(OnCarLevelUp);
        GameEvent.OnCarLevelUp.AddListener(OnCarLevelUp);



        //==============TALENT DAMAGE===============//
        gunDamageBase = User.Instance.Car.damage + User.Instance.UserPlayerUsing.damage;
        gunDamage = gunDamageBase;
        gunDamage += GameManager.Instance.damageTalent;
        gunDamage += gunDamage * (GameManager.Instance.percentATKTalent / 100);
    }


    public void OnCarLevelUp()
    {
        skeleton.SetSkin(User.Instance.Car.gun.ToString());
        skeleton.SetSlotsToSetupPose();
    }


    public void OnUnlockTalent()
    {
        gunDamage = gunDamageBase;
        gunDamage += GameManager.Instance.damageTalent;
        gunDamage += gunDamage * (GameManager.Instance.percentATKTalent / 100);
    }

    public void GunFlashOn()
    {
        if (gunFlash.activeSelf == false)
        {
            gunFlash.SetActive(true);
            ShootAnim();
            GameEvent.OnShooting.Invoke();
        }
    }

    public void GunFlashOff()
    {
        if (gunFlash.activeSelf == true)
        {
            gunFlash.SetActive(false);
            IdleAnim();
            if(GlobalData.gameMode != GameMode.Home)
            {
                GameEvent.OnEndShooting.Invoke();
            }
        }
    }

    public void ShootAnim()
    {
        animState.SetAnimation(0,"shoot",true);
    }

    public void IdleAnim()
    {
        animState.SetAnimation(0, "idle", true);
    }

    public void CheckingSpeedShoot()
    {
        speedShoot = speedShootBase;
        if (booster.listBoost.Contains(NameBooster.PercentSpeed10))
        {
            speedShoot -= speedShootBase * 0.1f;
        }
        if (booster.listBoost.Contains(NameBooster.PercentSpeed15))
        {
            speedShoot -= speedShootBase * 0.15f;
        }
        if (booster.listBoost.Contains(NameBooster.PercentSpeed20))
        {
            speedShoot -= speedShootBase * 0.20f;
        }
    }

    public void Shoot()
    {
        if (GameManager.Instance.trainManager.isStun)
        {
            return;
        }

        GunFlashOn();

        if (booster.listBoost.Contains(NameBooster.DuoBullet) && booster.listBoost.Contains(NameBooster.ConeBullet))
        {
            Spread = new Vector3(0, 0, -10);
            DuoAndCone();
        }
        else if (booster.listBoost.Contains(NameBooster.DuoBullet))
        {
            DuoBullet();
        }
        else if (booster.listBoost.Contains(NameBooster.ConeBullet))
        {
            Spread = new Vector3(0, 0, -10);
            ConeBullet();
        }
        else
        {
            BulletNormal();
        }
        AudioManager.instance.Play("Shoot");

    }

    public void BulletNormal()
    {
        BulletPlayer bullet = ContentPoolable.Emit(this.bullet) as BulletPlayer;
        bullet.transform.position = new Vector3(muzzleFlash.position.x, muzzleFlash.position.y, muzzleFlash.position.z);
        bullet.transform.rotation = muzzleFlash.rotation;
        bullet.bulletDamageBase = this.gunDamage;
        bullet.AddForce();
    }

    public void DuoBullet()
    {
        BulletPlayer bullet = ContentPoolable.Emit(this.bullet) as BulletPlayer;
        bullet.transform.position = new Vector3(muzzleFlash.position.x, muzzleFlash.position.y - 0.2f, muzzleFlash.position.z);
        bullet.transform.rotation = muzzleFlash.rotation;
        bullet.bulletDamageBase = this.gunDamage;
        bullet.AddForce();

        BulletPlayer bullet2 = ContentPoolable.Emit(this.bullet) as BulletPlayer;
        bullet2.transform.position = new Vector3(muzzleFlash.position.x, muzzleFlash.position.y + 0.2f, muzzleFlash.position.z);
        bullet2.transform.rotation = muzzleFlash.rotation;
        bullet2.bulletDamageBase = this.gunDamage;
        bullet2.AddForce();
    }

    public void ConeBullet()
    {
        for (int i = 0; i < 3; i++)
        {
            SpawnProjectile(muzzleFlash.position, i, 3);
        }
    }


    public void DuoAndCone()
    {
        for (int i = 0; i < 6; i++)
        {
            SpawnProjectile(muzzleFlash.position, i, 6);
        }
    }


    public void SpawnProjectile(Vector3 spawnPosition, int projectileIndex, int totalProjectiles)
    {
        BulletPlayer bullet2 = ContentPoolable.Emit(this.bullet) as BulletPlayer;
        bullet2.transform.position = spawnPosition;
        bullet2.transform.rotation = muzzleFlash.rotation;
        bullet2.bulletDamageBase = this.gunDamage;

        if (bullet2 != null)
        {
                if (totalProjectiles > 1)
                {
                    _randomSpreadDirection.x = Remap(projectileIndex, 0, totalProjectiles - 1, -Spread.x, Spread.x);
                    _randomSpreadDirection.y = Remap(projectileIndex, 0, totalProjectiles - 1, -Spread.y, Spread.y);
                    _randomSpreadDirection.z = Remap(projectileIndex, 0, totalProjectiles - 1, -Spread.z, Spread.z);
                }
                else
                {
                    _randomSpreadDirection = Vector3.zero;
                }
            
            bullet2.transform.rotation = Quaternion.Euler(new Vector3(bullet2.transform.localEulerAngles.x, bullet2.transform.localEulerAngles.y, bullet2.transform.localEulerAngles.z - _randomSpreadDirection.z));
            bullet2.AddForce();
        }
    }

    public static float Remap(float x, float A, float B, float C, float D)
    {
        float remappedValue = C + (x - A) / (B - A) * (D - C);
        return remappedValue;
    }
}
