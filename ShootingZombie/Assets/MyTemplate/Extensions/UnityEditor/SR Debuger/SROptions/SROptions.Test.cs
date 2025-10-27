//#define ENABLE_TEST_SROPTIONS
using System;
using System.ComponentModel;
using System.Diagnostics;
using SRDebugger;
using SRDebugger.Services;
using SRF;
using SRF.Service;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using MyGame;

public partial class SROptions
{
    //[Category("Cheat")]
    //public void Gold()
    //{
    //    User.Instance[ItemID.Gold] += 99999999;
    //}

    //public void Gem()
    //{
    //    User.Instance[ItemID.Gem] += 100000;
    //}

    [Category("Cheat")]
    public void WinGame()
    {

    }

    [Category("Cheat")]
    public void LoseGame()
    {

    }

    //[Category("Cheat Ads")]
    //public bool SkipAdsReward
    //{
    //    get
    //    {
    //        return AdsManager.CheatAds;
    //    }
    //    set
    //    {
    //        AdsManager.CheatAds = value;
    //        User.Instance[ItemID.RemoveAds] = 1;
    //        AdsManager.Instance.HideBanner();
    //    }
    //}

    //int level = 0;
    //[Category("Cheat Level")]
    //public int Level
    //{
    //    get
    //    {
    //        return level;
    //    }
    //    set
    //    {
    //        level = value;

    //        User.Instance[ItemID.PlayingLevel] = level;
    //        SceneManager.LoadScene(SceneName.GameScene.ToString());
    //    }
    //}
}
