using UnityEngine;

public class BulletBot : BulletBase
{
    public override void OnEnable()
    {
        base.OnEnable();
        //kiem tra crit
        float crit = 0;
        if(typeBot == TypeBot.Pistol)
        {
            crit = User.Instance.UserBot1Using.crit;
        }
        else if (typeBot == TypeBot.Riffle)
        {
            crit = User.Instance.UserBot2Using.crit;
        }
        else if (typeBot == TypeBot.Bazoka)
        {
            crit = User.Instance.UserBot3Using.crit;
        }

        if (Random.Range(1, 101) < crit)
        {
            //crit
            // hitEffect = hitEffectCrit;
            bulletCurrentDamage = 2 * bulletCurrentDamage;
        }
    }

    public void SetDamage(TypeBot typeBot)
    {
        switch (typeBot)
        {
            case TypeBot.Pistol:
                bulletDamageBase = User.Instance.UserBot1Using.damage;
                bulletCurrentDamage = bulletDamageBase;
                break;
            case TypeBot.Riffle:
                bulletDamageBase = User.Instance.UserBot2Using.damage;
                bulletCurrentDamage = bulletDamageBase;
                break;
            case TypeBot.Bazoka:
                bulletDamageBase = User.Instance.UserBot3Using.damage;
                bulletCurrentDamage = bulletDamageBase;
                break;
        }
    }
}
