using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ItemID
{
    None,
    Music,
    Sound,
    Vibrate,
    LoadingSprite,
    PlayingLevel,
    Ads,
    Gold,
    IAP_Count,
    RemoveAds,
    days_playing,
    day_of_year,
    ItemIcon,
    Win,
    WinStreak,
    Lose,
    Gem,


    //gun piece
    Manh_NaoSung,
    Manh_BangSung,
    Manh_VoDan,
    Manh_LoXo,
    Manh_ConOc,
    Manh_Sung_End,
    LevelBossMode,

    //tut
    tutTalent,
    SkinFree,



    //Iap packs
    pack_remove_ads = 200,
    Iapp_End,

    gun_1 = 300,
    gun_2,
    gun_3,
    gun_4,
    gun_5,
    gun_6,
    player_Gun_end,

    bullet_1 = 400,
    bullet_2,
    bullet_3,
    bullet_4,
    bullet_5,
    bullet_6,
    bullet_7,
    bullet_8,
    bullet_9,
    bullet_10,
    bullet_bot_1,
    bullet_enemy_1,
    bullet_enemy_fly,
    ice_bullet,
    bullet_enemy_poison,
    bullett_mud_poison,
    bullet_end,

    rocket_1 = 450,
    rocket_end,

    map_1_1 = 500,
    map_1_2,
    map_1_3,
    map_1_4,
    map_1_GasStation,
    map_1_TramDungNghi,
    map_1_end,

    map_2_1,
    map_2_2,
    map_2_3,
    map_2_4,


    map_3,
    map_4,
    map_5,
    map_6,
    map_7,
    map_8,
    map_9,
    map_10,
    map_end,

    enemy_1 = 600,
    enemy_2,
    enemy_3,
    enemy_4,
    enemy_tank_1,
    enemy_tank_2,
    enemy_bomb_1,
    enemy_dog,

    enemy_motobike_1 = 620,
    enemy_poison_1,
    enemy_mud_1,
    enemy_mud_2,

    enemy_fly_1 = 650,
    enemy_fly_near_1,
    enemy_end,

    enemy_gold_1=675,


    car_1 = 700,
    car_2,
    car_3,
    car_4,
    car_5,
    car_6,
    car_end,
    //skin car
    phase_1=720,
    phase_2,
    phase_3,
    phase_4,

    //repairer
    repairer_blue = 750,
    repairer_red,
    repairer_green,
    repairer_end,


    enemy_hit_1 = 800,
    enemy_hit_bazoka,
    enemy_hit_2,
    enemy_hit_3,
    enemy_hit_bazoka_1,
    ice_hit,
    enemy_hit_end,

    //character
    bot_pistol_1 = 900,
    bot_bazoka_1,
    bot_ak_1,
    bot_end,

    player_1 = 950,
    player_end,


    coin_follow_1 = 1000,
    path,


    gun_flash_1 = 1050,
    bazoka_flash_1,
    gun_flash_2,
    gun_flash_3,
    gun_flash_4,
    gun_flash_5,
    gun_flash_end,

    car_hit_1 = 2000,
    car_hit_end,

    car_up_1 = 2100,
    car_up_end,

    smoke_1 = 2200,
    smoke_2,
    smoke_3,
    smoke_4,
    smoke_5,
    smoke_end,

    drone_1 = 2300,
    drone_2,
    drone_end,

    boss_1 = 2400,
    boss_2,
    boss_end,
    shield = 2500,
    plane1 = 2600,
    boom = 2700,

    gunButtonInventory,

    //gun
    GunCar = 2800,

    //PlayingLevelTotalUnlock = 3000,
    LevelToPlay,
    IsAutoPlay,


    //daily
    HeroWeapon,
    day7,
    day7Open,
    dayCoin,
    dayCoinOpen,
    PopupClaimItem,


    //fx
    BulletDestroy = 3300,
    FxGoldEnemyDie = 3301,

    //Map boss world
    map_boss_1 = 3400,
    map_boss_2,
    map_boss_3,
    map_boss_4,
    map_boss_5,
    map_boss_end,

    boss_world_1 = 3450,
    boss_world_2,
    boss_world_3,
    boss_world_4,
    boss_world_5,
    boss_world_end,



    levelCollectToPlay = 3500,
    levelHomeStation,
    thorAmount,
    thorUpgraded,


    //skin
    main =3600,
    luffy,
    tanjiro,
    zenitsu,
    sasuke,
    naruto,

    //bot pistal
    char_blue,
    char_captain,
    char_cyclops,
    char_huggy,
    char_police,
    char_superman,

    //bot ak
    char_deadpool,
    char_fireman,
    char_flash,
    char_ironman,
    char_red,
    char_spiderman,

    //bot bazoka
    char_minion,
    char_pikachu,
    char_pooh,
    char_shipper,
    char_wolverine,
    char_yellow,
    skin_end,

    //sung phu
    Magma_1 = 3700,
    Magma_2,
    Magma_3,
    Rifle_1,
    Rifle_2,
    Rifle_3,
    Shotgun_1,
    Shotgun_2,
    Shotgun_3,
    Rocket_1,
    Weapon_skin_end,

    TutPlay = 3800,
    TutUpgradeWeapon,
    TutUpgradeCar,
    TutUpgradeGara,
    TutBossWorld,
    TutCollectFuel,
    TutEndless,
    TutSelectBoss,
    //
    TutSlectWorkShop,
    TutGamePlay,
    TutBooster,
    TutBoosterVip,

    //free skin hero
    solanfreeSkin =3900,
    itemSkin = 3950,
    talentCurrent=4000,
    talentCurrentSpecial = 4025,
    levelTalentUI =4050,
    ticket=4100,
    expCarCurrent=4105,
    itemDotPha,
    WitchChar = 4200,

    //car skin

    IronSource_Consent = 999999,
    ShowedGDPR,


}

public static class ItemType
{
    public static List<ItemID> IapPacks = ((ItemID[])Enum.GetValues(typeof(ItemID))).Where(x => (int)x >= (int)ItemID.pack_remove_ads && (int)x < (int)ItemID.Iapp_End).ToList();

    //==========MAPS=========//
    public static List<ItemID> Map_1 = ((ItemID[])Enum.GetValues(typeof(ItemID))).Where(x => (int)x >= (int)ItemID.map_1_1 && (int)x < (int)ItemID.map_1_end).ToList();

    //==========MAPS BOSS WORLD=========//
    public static List<ItemID> Map_Boss = ((ItemID[])Enum.GetValues(typeof(ItemID))).Where(x => (int)x >= (int)ItemID.map_boss_1 && (int)x < (int)ItemID.map_boss_end).ToList();

    //==========CHARACTER=========//
    public static List<ItemID> Bots = ((ItemID[])Enum.GetValues(typeof(ItemID))).Where(x => (int)x >= (int)ItemID.bot_pistol_1 && (int)x < (int)ItemID.bot_end).ToList();

    //==========REPAIRER=========//
    public static List<ItemID> Repairer = ((ItemID[])Enum.GetValues(typeof(ItemID))).Where(x => (int)x >= (int)ItemID.repairer_blue && (int)x < (int)ItemID.repairer_end).ToList();

    //==========ENEMIES=========//
    public static List<ItemID> Enemies = ((ItemID[])Enum.GetValues(typeof(ItemID))).Where(x => (int)x >= (int)ItemID.enemy_1 && (int)x < (int)ItemID.enemy_end).ToList();

    //==========ENEMIES BASIC=========//
    public static List<ItemID> EnemiesBasic = ((ItemID[])Enum.GetValues(typeof(ItemID))).Where(x => (int)x >= (int)ItemID.enemy_1 && (int)x < (int)ItemID.enemy_motobike_1).ToList();

    //==========BOSS NORMAL=========//
    public static List<ItemID> BossNormal = ((ItemID[])Enum.GetValues(typeof(ItemID))).Where(x => (int)x >= (int)ItemID.boss_1 && (int)x < (int)ItemID.boss_end).ToList();

    //==========MANHSUNG=========//
    public static List<ItemID> ManhSung = ((ItemID[])Enum.GetValues(typeof(ItemID))).Where(x => (int)x >= (int)ItemID.Manh_NaoSung && (int)x < (int)ItemID.Manh_Sung_End).ToList();

    //==========CAR=========//
    public static List<ItemID> Cars = ((ItemID[])Enum.GetValues(typeof(ItemID))).Where(x => (int)x >= (int)ItemID.car_1 && (int)x < (int)ItemID.car_end).ToList();

    //==========CHAR SKIN=========//
    public static List<ItemID> Skins = ((ItemID[])Enum.GetValues(typeof(ItemID))).Where(x => (int)x >= (int)ItemID.main && (int)x < (int)ItemID.skin_end).ToList();

    //==========CHAR SKIN=========//
    public static List<ItemID> SkinsMain = ((ItemID[])Enum.GetValues(typeof(ItemID))).Where(x => (int)x >= (int)ItemID.main && (int)x < (int)ItemID.char_blue).ToList();

    //==========CHAR SKIN=========//
    public static List<ItemID> SkinsBot = ((ItemID[])Enum.GetValues(typeof(ItemID))).Where(x => (int)x >= (int)ItemID.char_blue && (int)x < (int)ItemID.skin_end).ToList();

    //==========CHAR SKIN WEAPON=========//
    public static List<ItemID> SkinWeapon = ((ItemID[])Enum.GetValues(typeof(ItemID))).Where(x => (int)x >= (int)ItemID.Magma_1 && (int)x < (int)ItemID.Weapon_skin_end).ToList();



    public static bool IsIapPack(this ItemID id)
    {
        return IapPacks.Contains(id);
    }

    public static string ToRGBHex(this Color c)
    {
        return string.Format("#{0:X2}{1:X2}{2:X2}", ToByte(c.r), ToByte(c.g), ToByte(c.b));
    }
    public static bool IsSkin(this ItemID id)
    {
        return Skins.Contains(id);
    }
    public static bool IsWeapon(this ItemID id)
    {
        return SkinWeapon.Contains(id);
    }
    private static byte ToByte(float f)
    {
        f = Mathf.Clamp01(f);
        return (byte)(f * 255);
    }
}