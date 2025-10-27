using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;
//using static FirebaseManager;

[CreateAssetMenu(fileName = "User", menuName = "Toan/User", order = 0)]
public class User : ScriptableObject
{
    private static User instance;
    public static User Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<User>("Manager/User");
            }

            return instance;
        }
    }

    public List<ItemValue> defaultItems;
    private UserData userData;
    private const string UserDataPath = "UserDataPath";

    public void Init()
    {
        userData = DataPersistent.ReadDataExist<UserData>(UserDataPath);
        if (userData == null)
        {
            userData = new UserData();
            userData.Init();
            Save();
        }

        CheckDefaultItem();
    }

    void CheckDefaultItem()
    {
        bool needToSave = false;

        foreach (ItemValue defaultItem in defaultItems)
        {
            if (!userData.items.ContainsKey(defaultItem.item))
            {
                userData.items.Add(defaultItem.item, defaultItem.value);
                needToSave = false;
            }
        }

        if (needToSave)
        {
            Save();
        }
    }

    public void Save()
    {
        DataPersistent.SaveData<UserData>(UserDataPath, userData);
    }

    public void Clear()
    {
        DataPersistent.ClearData(UserDataPath);
    }

    public int this[ItemID id, AdLocation location = AdLocation.None]
    {
        get
        {
            if (!userData.items.ContainsKey(id))
            {
                return 0;
            }
            return userData.items[id];
        }
        set
        {
            if (!userData.items.ContainsKey(id))
            {
                userData.items.Add(id, value);
                Earn(id, value, location);
            }
            else
            {
                int change = value - userData.items[id];
                if (change > 0)
                {
                    Earn(id, change, location);
                }
                else
                {
                    Spend(id, -change, location);
                }

                userData.items[id] = value;
            }

            switch (id)
            {
                case ItemID.PlayingLevel:
                    {
                        //FirebaseManager.Instance.UserProperty(EventName.level_reach, value.ToString(), location.ToString());
                        break;
                    }
                case ItemID.days_playing:
                    {
                        //FirebaseManager.Instance.UserProperty(EventParameter.days_playing, value.ToString(), location.ToString());
                        break;
                    }
            }

            Save();

            GameEvent.OnItemChanged?.Invoke(id, value);
        }
    }

    void Earn(ItemID id, int value, AdLocation location)
    {
        if (id != ItemID.Gold || location == AdLocation.None)
        {
            return;
        }

        //FirebaseManager.Instance.earn_virtual_currency(id, value, location.ToString());
        //FirebaseManager.Instance.UserProperty(EventParameter.total_earn, value.ToString(), location.ToString());
    }

    void Spend(ItemID id, int value, AdLocation location)
    {
        if (id != ItemID.Gold || location == AdLocation.None)
        {
            return;
        }

        //FirebaseManager.Instance.spend_virtual_currency(id, value, location.ToString());
        //FirebaseManager.Instance.UserProperty(EventParameter.total_spent, value.ToString(), location.ToString());
    }

    //public bool EnableMusic
    //{
    //    get
    //    {
    //        return this[ItemID.Music] == 1;
    //    }

    //    set
    //    {
    //        this[ItemID.Music] = value ? 1 : 0;
    //        AudioAssistant.main.musicVolume = value ? 1 : 0;
    //        Save();
    //    }
    //}

    //public bool EnableVibrate
    //{
    //    get
    //    {
    //        return this[ItemID.Vibrate] == 1;
    //    }

    //    set
    //    {
    //        this[ItemID.Vibrate] = value ? 1 : 0;
    //        //VibrateManager.Instance.Init();
    //        Save();
    //    }
    //}


    //public bool EnableSound
    //{
    //    get
    //    {
    //        return this[ItemID.Sound] == 1;
    //    }

    //    set
    //    {
    //        this[ItemID.Sound] = value ? 1 : 0;
    //        AudioAssistant.main.soundVolume = value ? 1 : 0;
    //        Save();
    //    }
    //}

    public bool TutCompleted
    {
        get
        {
            return userData.tutCompleted;
        }

        set
        {
            userData.tutCompleted = value;
            Save();
        }
    }

    public int SessionCount
    {
        get
        {
            return userData.sessionCount;
        }

        set
        {
            userData.sessionCount = value;
            Save();
        }
    }
    public int IndexBtnTutData
    {
        get
        {
            return userData.indexBtnTut;
        }

        set
        {
            userData.indexBtnTut = value;
            Save();
        }
    }
    public bool IsCompletedTutID(TutID tutID)
    {
        return userData.completedTutID.Contains((int)tutID);
    }

    public void SetCompletedTutID(TutID tutID)
    {
        userData.completedTutID.Add((int)tutID);
        Save();
    }

    public void WinRate(bool iswin)
    {
        if (iswin)
        {
            this[ItemID.Win]++;
            this[ItemID.WinStreak]++;
        }
        else
        {
            this[ItemID.Lose]++;
            this[ItemID.WinStreak] = 0;
        }
    }

    public List<ItemID> ListUserGun()
    {
        return userData.listUserGun;
    }

    public List<ItemID> ListBotGun()
    {
        return userData.listBotGun;
    }

    public List<ItemID> ListBots()
    {
        return userData.bots;
    }

    public Player Player()
    {
        return userData.player;
    }



    #region CAR AND BOT

    public UserCar Car
    {
        get => userData.car;
        set
        {
            userData.car = value;
        }
    }

    public List<UserBot> UserPlayers1
    {
        get => userData.userPlayers;
        set
        {
            userData.userPlayers = value;
        }
    }

    public List<UserBot> UserBots1
    {
        get => userData.userBots1;
        set
        {
            userData.userBots1 = value;
        }
    }

    public List<UserBot> UserBots2
    {
        get => userData.userBots2;
        set
        {
            userData.userBots2 = value;
        }
    }

    public List<UserBot> UserBots3
    {
        get => userData.userBots3;
        set
        {
            userData.userBots3 = value;
        }
    }

    public UserBot UserPlayerUsing
    {
        get => userData.userPlayerUsing;
        set
        {
            userData.userPlayerUsing = value;
        }
    }

    public UserBot UserBot1Using
    {
        get => userData.userBot1Using;
        set
        {
            userData.userBot1Using = value;
        }
    }

    public UserBot UserBot2Using
    {
        get => userData.userBot2Using;
        set
        {
            userData.userBot2Using = value;
        }
    }

    public UserBot UserBot3Using
    {
        get => userData.userBot3Using;
        set
        {
            userData.userBot3Using = value;
        }
    }

    #endregion




    public ItemID GunUsing
    {
        get => userData.gunUsing;
        set
        {
            userData.gunUsing = value;
        }
    }

    public ItemID GunBotUsing
    {
        get => userData.gunBotUsing;
        set
        {
            userData.gunBotUsing = value;
        }
    }

    public GameMode GameMode
    {
        get => userData.gameMode;
        set
        {
            userData.gameMode = value;
        }
    }

    public List<StarLevel> ListStarLevel()
    {
        return userData.listStarLevel;
    }

    public List<ItemID> ListCharSkin()
    {
        return userData.listCharSkin;
    }

    //current skin of character
    public ItemID CurrentPlayerSkin
    {
        get => userData.currentPlayerSkin;
        set
        {
            userData.currentPlayerSkin = value;
        }
    }

    public ItemID CurrentBot1Skin
    {
        get => userData.currentBot1Skin;
        set
        {
            userData.currentBot1Skin = value;
        }
    }

    public ItemID CurrentBot2Skin
    {
        get => userData.currentBot2Skin;
        set
        {
            userData.currentBot2Skin = value;
        }
    }
    public ItemID CurrentBot3Skin
    {
        get => userData.currentBot3Skin;
        set
        {
            userData.currentBot3Skin = value;
        }
    }

    public List<Talent> UserTalents()
    {
        return userData.userTalents;
    }
    public int TotalSpin
    {
        get => userData.totalSpin;
        set => userData.totalSpin = value;
    }

    public List<ItemID> lstGunclaim()
    {
          return userData.gunclaim;
    }

    public bool IsSpinDailyAds
    {
        get => userData.isSpinDaily;
        set => userData.isSpinDaily = value;
    }
}

[System.Serializable]
public class UserData
{
    public Dictionary<ItemID, int> items;
    public bool tutCompleted;

    public int sessionCount = -1;

    public List<int> completedTutID;

    //inventory
    public List<ItemID> listUserGun;
    public ItemID gunUsing;

    public List<ItemID> listBotGun;
    public ItemID gunBotUsing;

    //bot
    public List<ItemID> bots;
    //player
    public Player player;
    //car
    public UserCar car;

    public GameMode gameMode;

    public List<StarLevel> listStarLevel;
    //skin
    public List<ItemID> listCharSkin;
    public ItemID currentPlayerSkin;
    public ItemID currentBot1Skin;
    public ItemID currentBot2Skin;
    public ItemID currentBot3Skin;

    //tutorial
    // public ItemID tutoriall;
    public int indexBtnTut;
    // public ItemID tutoriall;
    public int totalSpin = 0;

    //talents
    public List<Talent> userTalents;


    //popup collection
    public List<ItemID> gunclaim;

    public List<UserBot> userPlayers;
    public UserBot userPlayerUsing;
    public List<UserBot> userBots1;
    public UserBot userBot1Using;
    public List<UserBot> userBots2;
    public UserBot userBot2Using;
    public List<UserBot> userBots3;
    public UserBot userBot3Using;

    public bool isSpinDaily;

    public void Init()
    {
        items = new Dictionary<ItemID, int>();
        tutCompleted = false;
        sessionCount = -1;
        User.Instance[ItemID.Gold] += 2000;
        User.Instance[ItemID.itemDotPha] = 0;
        completedTutID = new List<int>();
        listUserGun = new List<ItemID>();
        User.Instance[ItemID.levelCollectToPlay] = 0;
        gameMode = GameMode.Home;

        StarLevel firstLevel = new StarLevel();
        firstLevel.level = 0;
        firstLevel.level = 0;
        listStarLevel = new List<StarLevel>();
        listStarLevel.Add(firstLevel);

        gunclaim = new List<ItemID>();

        User.Instance[ItemID.levelHomeStation] = 0;
        User.Instance[ItemID.thorAmount] = 0;
        User.Instance[ItemID.thorUpgraded] = 0;


        User.Instance[ItemID.Music] = 1;
        User.Instance[ItemID.Sound] = 1;

        //manh sung
        User.Instance[ItemID.Manh_NaoSung] = 50;
        User.Instance[ItemID.Manh_BangSung] = 2;
        User.Instance[ItemID.Manh_VoDan] = 3;
        User.Instance[ItemID.Manh_LoXo] = 4;
        User.Instance[ItemID.Manh_ConOc] = 5;

        //bot
        bots = new List<ItemID>();
        bots.Add(ItemID.bot_pistol_1);
        bots.Add(ItemID.bot_ak_1);
        bots.Add(ItemID.bot_bazoka_1);

        //player
        player = new Player();
        player.id = ItemID.player_1;
        player.level = 1;

        //car
        car = new UserCar(Resources.Load<CarLevelData>("Scriptable/CarLevelData").carLevelDatas[0]);

        //sung phu
        listBotGun = new List<ItemID>();
        listBotGun.Add(ItemID.Magma_1);
        gunBotUsing = ItemID.Magma_1;

        //TutHome
        indexBtnTut = 0;
        User.Instance[ItemID.TutPlay] = 0;

        User.Instance[ItemID.solanfreeSkin] = 0;
        User.Instance[ItemID.TutUpgradeWeapon] = 0;
        User.Instance[ItemID.TutUpgradeGara] = 0;
        User.Instance[ItemID.TutUpgradeCar] = 0;
        User.Instance[ItemID.TutBossWorld] = 0;
        User.Instance[ItemID.TutCollectFuel] = 0;
        User.Instance[ItemID.TutEndless] = 0;
        User.Instance[ItemID.TutSelectBoss] = 0;
        User.Instance[ItemID.TutSlectWorkShop] = 0;
        User.Instance[ItemID.tutTalent] = 0;

        //TutGamePlay
        User.Instance[ItemID.TutGamePlay] = 0;
        User.Instance[ItemID.TutBooster] = 0;
        User.Instance[ItemID.TutBoosterVip] = 0;

        //talent
        User.Instance[ItemID.talentCurrent] = 2;
        User.Instance[ItemID.talentCurrentSpecial] = 1;
        userTalents = new List<Talent>();
        Talent talent = new Talent();
        talent.level = 1;
        talent.talentType = TalentType.Damage;
        userTalents.Add(talent);
        User.Instance[ItemID.ticket] = 1;


        //BOT USING
        userPlayers = new List<UserBot>();
        userBots1 = new List<UserBot>();
        userBots2 = new List<UserBot>();
        userBots3 = new List<UserBot>();

        //userPlayerUsing = new UserBot();
        //userPlayerUsing.type = TypeBot.Pistol;
        //userPlayerUsing.level = 1;
        //userPlayerUsing.damage = 1;
        //userPlayerUsing.name = "Iron Man";
        //userPlayerUsing.skin = "Char/skin_ironman";
        //userPlayerUsing.speedShoot = 5;
        //userPlayerUsing.shootRange = 25;
        //userPlayers.Add(userPlayerUsing);
        //userPlayerUsing = userPlayers[0];


        foreach (UserBot userBot in Resources.Load<HeroesLevelData>("Scriptable/HeroesLevelData").listPlayers)
        {
            UserBot bot = new UserBot(userBot);
            userPlayers.Add(bot);
        }
        userPlayers[0].isUnlock = true;
        userPlayers[0].isUsing = true;
        userPlayerUsing = userPlayers[0];

        foreach (UserBot userBot in Resources.Load<HeroesLevelData>("Scriptable/HeroesLevelData").listBot1)
        {
            UserBot bot = new UserBot(userBot);
            userBots1.Add(bot);
        }
        foreach (UserBot userBot in Resources.Load<HeroesLevelData>("Scriptable/HeroesLevelData").listBot2)
        {
            UserBot bot = new UserBot(userBot);
            userBots2.Add(bot);
        }
        foreach (UserBot userBot in Resources.Load<HeroesLevelData>("Scriptable/HeroesLevelData").listBot3)
        {
            UserBot bot = new UserBot(userBot);
            userBots3.Add(bot);
        }

        User.Instance[ItemID.expCarCurrent] = 0;
        User.Instance[ItemID.Music] = 1;
        User.Instance[ItemID.Sound] = 1;
        User.Instance[ItemID.LevelBossMode] = 1;
    }

    [OnDeserialized]
    void OnDeserialized(StreamingContext sc)
    {
        if (completedTutID == null)
        {
            completedTutID = new List<int>();
        }
    }
}

[System.Serializable]
public class ItemValue
{
    [HorizontalGroup("Group 1")]
    [HideLabel, LabelWidth(50)]
    public ItemID item;

    [HorizontalGroup("Group 1")]
    [HideLabel, LabelWidth(50)]
    public int value;

    public ItemValue(ItemID _item, int _value)
    {
        item = _item;
        value = _value;

    }

    public ItemValue Clone()
    {
        return new ItemValue(item, value);
    }
}

[System.Serializable]
public class ItemValueFloat
{
    [HideLabel]
    public ItemID item;

    [HideLabel]
    public float value;

    public ItemValueFloat(ItemID _item, float _value)
    {
        item = _item;
        value = _value;
    }

    public ItemValueFloat Clone()
    {
        return new ItemValueFloat(item, value);
    }
}

[System.Serializable]
public class Player
{
    public ItemID id;
    public int level;
}

[System.Serializable]
public class Car
{
    public ItemID id;
    public int level;
    public CarName carName;
}

public enum CarName
{
    CarGreen,
}

[System.Serializable]
public class StarLevel
{
    public int level;
    public int starAmount;
}



//==========USER INVENTORY============//
[System.Serializable]
public class UserCar
{
    public int level;
    public ItemID skin;
    public ItemID gun;
    public int slot;
    public float exp;
    public float gold;
    public float damage;
    public float hp;
    public float crit;
    public float def;
    public float expNeedToNewSkin;
    public ItemID newSkin;

    public UserCar(UserCar userCar)
    {
        level = userCar.level;
        skin = userCar.skin;
        gun = userCar.gun;
        slot = userCar.slot;
        exp = userCar.exp;
        gold = userCar.gold;
        damage = userCar.damage;
        hp = userCar.hp;
        crit = userCar.crit;
        def = userCar.def;
        expNeedToNewSkin = userCar.expNeedToNewSkin;
        newSkin = userCar.newSkin;
    }
}

[System.Serializable]
public class UserBot
{
    public ItemID id;
    public TypeBot type;
    public string name;
    public SkinChar skin;
    public bool isUnlock;
    public bool isUsing;
    public float price;
    public PriceType priceType;
    public int level;
    public float damage;
    public float crit;
    public float speedShoot;
    public float shootRange;
    public int mucDotPha;
    public int itemCanDeDotPha;
    public int goldNeedToUp;
    public int levelMax1;
    public int levelMax2;
    public int levelMax3;
    public int star;
    public float damageMax;
    public float critMax;
    public float damageDotPha1;
    public float critDotPha1;
    public float damageDotPha2;
    public float critDotPha2;
    public int goldDotPha;
    public int adsDotPha;
    public int adsDotPhaDaxem;

    public int adsUnlock;

    public UserBot(UserBot userBot)
    {
        id = userBot.id;
        type = userBot.type;
        name = userBot.name;
        skin = userBot.skin;
        isUnlock = userBot.isUnlock;
        isUsing = userBot.isUsing;
        price = userBot.price;
        priceType = userBot.priceType;
        level = userBot.level;
        damage = userBot.damage;
        crit = userBot.crit;
        speedShoot = userBot.speedShoot;
        shootRange = userBot.shootRange;
        mucDotPha = userBot.mucDotPha;
        itemCanDeDotPha = userBot.itemCanDeDotPha;
        goldNeedToUp = userBot.goldNeedToUp;
        levelMax1 = userBot.levelMax1;
        levelMax2 = userBot.levelMax2;
        levelMax3 = userBot.levelMax3;
        star = userBot.star;
        damageMax = userBot.damageMax;
        critMax = userBot.critMax;
        damageDotPha1 = userBot.damageDotPha1;
        critDotPha1 = userBot.critDotPha1;
        damageDotPha2 = userBot.damageDotPha2;
        critDotPha2 = userBot.critDotPha2;
        goldDotPha = userBot.goldDotPha;
        adsDotPha = userBot.adsDotPha;
        adsDotPhaDaxem = userBot.adsDotPhaDaxem;
        adsUnlock = userBot.adsUnlock;
    }
}

public enum TypeBot
{
    Player,
    Pistol,
    Riffle,
    Bazoka,
}

public enum PriceType
{
    Coin,
    Gem,
    Ads,
}
