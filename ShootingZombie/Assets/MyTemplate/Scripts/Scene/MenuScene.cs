using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using Thanh.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuScene : BaseScene
{
    //Left btn
    public static MenuScene _main;
    public static MenuScene main
    {
        get
        {
            if (_main == null)
            {
                _main = Instance as MenuScene;
            }

            return _main;
        }
    }

    public Button btnSetting;
    public Button btnSpin;
    public Button btnRemoveAds;
   
    //Right btn
    public Button btnShop;
    public Button btnDaily;

    //start
    public Button btnStart;

    //text
    public TextMeshProUGUI txtLevel;
    protected override void Start()
    {
        base.Start();
        txtLevel.text ="LEVEL  "+ (User.Instance[ItemID.PlayingLevel]+1).ToString();
        //AudioAssistant.main.PlayMusic("MenuScene");
        RegisterButton();
        
    }

    void RegisterButton()
    {
        btnSetting.onClick.RemoveAllListeners();
        btnSetting.onClick.AddListener(OnSetting);

        btnSpin.onClick.RemoveAllListeners();
        btnSpin.onClick.AddListener(OnSpin);

        btnRemoveAds.onClick.RemoveAllListeners();
        btnRemoveAds.onClick.AddListener(OnRemoveAds);

        btnShop.onClick.RemoveAllListeners();
        btnShop.onClick.AddListener(OnShop);

        btnDaily.onClick.RemoveAllListeners();
        btnDaily.onClick.AddListener(OnDaily);

        btnStart.onClick.RemoveAllListeners();
        btnStart.onClick.AddListener(OnStart);
    }

    void OnSpin()
    {

    }

    void OnShop()
    {
    }

    void OnDaily()
    {
    }

    void OnSetting()
    {
        PopupManager.Instance.OpenPopup<PopupSetting>(PopupID.PopupSetting);
    }

    void OnRemoveAds()
    {

    }

    void OnStart()
    {
        Loader.Instance.LoadScene(SceneName.GameScene.ToString());
    }
}
