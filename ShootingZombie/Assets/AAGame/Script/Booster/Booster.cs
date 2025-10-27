using UnityEngine;

[CreateAssetMenu(fileName = "Booster", menuName = "Booster/Booster", order = 1)]
public class Booster : ScriptableObject
{
    public NameBooster booster;
    public TypeBooster type;
    public Sprite icon;
    public Sprite bg;
    public string nameBooster;
    public string typeBooster;

}

public enum NameBooster
{
    PercentDamage10,
    PercentDamage15,

    PercentSpeed10,
    PercentSpeed15,
    PercentSpeed20,

    PercentHealth20,
    PercentHealth50,

    DuoBullet,
    ConeBullet,
    SeriBullet,
    BulletBounc,


    //=============VIP=============//
    Drone,
    Shield,
    Plane,

    //==========BOOSTER UNCHANGED==========//
    Strength,
    Wealthier,
    Luckier,
    Healing,

    Strange,
}

public enum TypeBooster
{
    Basic,
    VIP,
    UnChange,
}