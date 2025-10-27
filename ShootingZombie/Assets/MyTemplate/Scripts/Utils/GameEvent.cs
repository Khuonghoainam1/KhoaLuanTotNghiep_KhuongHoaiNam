using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GameEvent : Singleton<GameEvent>
{
    public static Event<ItemID, int> OnItemChanged = new Event<ItemID, int>();
    public static Event<ItemID> ChangeUsingSkin = new Event<ItemID>();
    public static Event<int> OnPopupChanged = new Event<int>();
    public static Event<TutUI, bool> OnShowTutUI = new Event<TutUI, bool>();
    public static Event<TutUI> OnClickTutUI = new Event<TutUI>();

    // 
    public static UnityEvent RefreshListMap = new UnityEvent();
    public static UnityEvent OnSelectBooster = new UnityEvent();
    public static UnityEvent<EnemyBase> OnEnemyDie = new UnityEvent<EnemyBase>();
    public static UnityEvent OnCarRunning = new UnityEvent();
    public static UnityEvent OnStopCarRunning = new UnityEvent();
    public static UnityEvent OnPlayerWin = new UnityEvent();
    public static UnityEvent OnPlayerLose = new UnityEvent();
    public static UnityEvent OnStartGame = new UnityEvent();
    public static UnityEvent OnReviveGame = new UnityEvent();
    public static UnityEvent<GunInventory> OnViewGun = new UnityEvent<GunInventory>();
    public static UnityEvent OnUnLockGun = new UnityEvent();

    public static UnityEvent OnShootingAuto = new UnityEvent();
    public static UnityEvent OffShootingAuto = new UnityEvent();

    public static UnityEvent StartFightingWithBoss = new UnityEvent();

    public static UnityEvent OnShowRevive = new UnityEvent();
    public static UnityEvent OnRevive = new UnityEvent();
    public static UnityEvent OnCloseRevive = new UnityEvent();
    public static UnityEvent OnPlayerStun = new UnityEvent();
    public static UnityEvent<string> OnUnlockNewSkin = new UnityEvent<string>();
    public static UnityEvent<HeroSelect> OnSelectHero = new UnityEvent<HeroSelect>();
    public static UnityEvent<string> OnSelectSkinOld = new UnityEvent<string>();
    public static UnityEvent<string> OnEquiepGun = new UnityEvent<string>();

    public static UnityEvent OnUpgradeGara = new UnityEvent();

    public static UnityEvent<NameBooster> OnSetTrueTutVip = new UnityEvent<NameBooster>();
    public static UnityEvent CollectAll = new UnityEvent();
    public static UnityEvent RessetBooster = new UnityEvent();
    public static UnityEvent OnUnlockTalent = new UnityEvent();
    public static UnityEvent OnMoveToPlay = new UnityEvent();
    public static UnityEvent OnCarLevelUp = new UnityEvent();
    public static UnityEvent OnCarUnlockSlot = new UnityEvent();


    public static UnityEvent<UserBot> OnSelectSkin = new UnityEvent<UserBot>();
    public static UnityEvent<UserBot> OnEquipSkin = new UnityEvent<UserBot>();
    public static UnityEvent<UserBot> OnLevelUpChar = new UnityEvent<UserBot>();
    public static UnityEvent OnShooting = new UnityEvent();
    public static UnityEvent OnEndShooting = new UnityEvent();
    public static UnityEvent OnSetupMap = new UnityEvent();

    public static void ClearDelegates()
    {
        Type eventType = typeof(UnityEventBase);
        typeof(GameEvent).GetFields()
            .Where(x => x.IsStatic && eventType.IsAssignableFrom(x.FieldType))
            .ForEach(x => (x.GetValue(null) as UnityEventBase).RemoveAllListeners());
    }
}

public class Event<T> : UnityEvent<T> { }
public class Event<T1, T2> : UnityEvent<T1, T2> { }
public class Event<T1, T2, T3> : UnityEvent<T1, T2, T3> { }
public class Event<T1, T2, T3, T4> : UnityEvent<T1, T2, T3, T4> { }