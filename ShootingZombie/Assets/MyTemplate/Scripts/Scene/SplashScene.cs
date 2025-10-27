using MyGame;
using System;
using System.Collections;
using System.Collections.Generic;
using Thanh.Core;
using UnityEngine;

public class SplashScene : BaseScene
{
    public Theme _theme;

    private new IEnumerator Start()
    {
#if UNITY_EDITOR
        Application.runInBackground = true;
#endif
        theme = _theme;
        currentSceneName = sceneName;

        Loader.Instance.FakeLoading();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        User.Instance.Init();
        yield return null;

        AudioAssistant.main.Init();
        yield return null;

        User.Instance.SessionCount++;
        yield return null;
#if ShowBanner
        bool closedGDPR = true;
        if (User.Instance[ItemID.ShowedGDPR] == 0)
        {
            closedGDPR = false;
            PopupManager.Instance.OpenPopup<PopupGDPR>(PopupID.PopupGDPR, (pop) => pop.onClose.AddListener(() => closedGDPR = true));
        }

        while (closedGDPR == false) yield return null;

        User.Instance[ItemID.ShowedGDPR] = 1;
#endif

        //AdsManager.Instance.Init();
        yield return null;

        TimeSchedule.Instance.Init();
        yield return null;

        DailyRewardManager.Instance.Init();
        yield return null;

#if UNITY_IOS
        AdsManager.Instance.RequestAuthorizationTracking();
#endif

        //if (/*User.Instance.SessionCount > 0 && */AdsManager.IsSpamAds())
        //{
        //    yield return WaitForSecondsCache.Get(FirebaseManager.Instance.remote.WaitAOA());
        //}

        //AdsManager.Instance.StopAOA();

        if (User.Instance[ItemID.day_of_year] == 0)
        {
            User.Instance[ItemID.day_of_year] = DateTime.Now.DayOfYear;
            User.Instance[ItemID.days_playing] = 0;
        }
        else if (DateTime.Now.DayOfYear != User.Instance[ItemID.day_of_year])
        {
            User.Instance[ItemID.day_of_year] = DateTime.Now.DayOfYear;
            User.Instance[ItemID.days_playing]++;
        }

        //if (User.Instance.SessionCount <= 0)
        //{
        Loader.Instance.LoadScene(SceneName.GameScene.ToString());
        //}
        //else
        //{
        //    Loader.Instance.LoadScene(SceneName.HomeScene.ToString());
        //}

        yield break;
    }
}

