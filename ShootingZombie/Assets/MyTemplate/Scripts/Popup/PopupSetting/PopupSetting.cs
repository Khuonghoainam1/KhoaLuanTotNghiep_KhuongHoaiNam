using System.Collections;
using System.Collections.Generic;
using Thanh.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;
#if FACEBOOK
using Facebook.Unity;
#endif

public class PopupSetting : Popup
{
    public Toggle btnVibrate;
    /*    public Toggle btnMusic;
        public Toggle btnSound;*/
    //public Image vibrateBG;
    //public Image musicBG;
    //public Image soundBG;

    //public Sprite toggleOff;
    //public Sprite toggleOn;

    //public Sprite vibrateOff;
    //public Sprite vibrateOn;

    //public Sprite soundOff;
    //public Sprite soundOn;

    //public Sprite musicOff;
    //public Sprite musicOn;

    public TextMeshProUGUI txtVersion;
    public Button btnRestoreIAP;

    public TextMeshProUGUI txtName;
    public Button Resume;
    public Button Home;

    public Toggle toggleMusic;
    public Toggle toggleSound;
    public void Awake()
    {
        if (User.Instance[ItemID.Music] == 1)
        {
            toggleMusic.isOn = true;
            AudioManager.instance.MuteAdio(false, "Music");
        }
        else
        {
            toggleMusic.isOn = false;
            AudioManager.instance.MuteAdio(true, "Music");



        }
        if (User.Instance[ItemID.Sound] == 1)
        {
            toggleSound.isOn = true;
            AudioManager.instance.MuteAdio(false, "Sound");
        }
        else
        {
            toggleSound.isOn = false;
            AudioManager.instance.MuteAdio(true, "Sound");
        }
    }
    public override void OnShow()
    {
        base.OnShow();

        
    
    }
    public void OnSwichMusic()
    {
        if (toggleMusic.isOn)
        {
            AudioManager.instance.MuteAdio(false, "Music");
            User.Instance[ItemID.Music] = 1;
        }
        else
        {
            AudioManager.instance.MuteAdio(true, "Music");
            User.Instance[ItemID.Music] = 0;
        }
    }
    public void OnSwichSound()
    {
        if (toggleSound.isOn)
        {
            AudioManager.instance.MuteAdio(false, "Sound");
            User.Instance[ItemID.Sound] = 1;
        }
        else
        {
            AudioManager.instance.MuteAdio(true, "Sound");
            User.Instance[ItemID.Sound] = 0;
        }
        // AudioAssistant.PlaySound("BtnClick");
    }
    /*  btnVibrate.onValueChanged.RemoveAllListeners();
      btnVibrate.onValueChanged.AddListener((isOn) =>
      {
         
          User.Instance.EnableVibrate = isOn;
          btnVibrate.image.sprite = isOn ? vibrateOn : vibrateOff;
          // vibrateBG.sprite = isOn ? vibrateOn : vibrateOff;
      });
      btnVibrate.isOn = User.Instance.EnableVibrate;
      btnVibrate.image.sprite = User.Instance.EnableVibrate ? vibrateOn : vibrateOff;*/
    //vibrateBG.sprite = User.Instance.EnableVibrate ? vibrateOn : vibrateOff;


    // musicBG.sprite = User.Instance.EnableMusic ? musicOn : musicOff;

    /*  btnMusic.onValueChanged.RemoveAllListeners();
      btnMusic.onValueChanged.AddListener((isOn) =>
      {
          AudioAssistant.PlaySound("BtnClick");
          User.Instance.EnableSound = isOn;
         // SoundManagerCustom.Instance.MuteAllSounds(!isOn);
          btnMusic.image.sprite = isOn ? soundOn : soundOff;
      });*/


    /*
            btnSound.onValueChanged.RemoveAllListeners();
            btnSound.onValueChanged.AddListener((isOn) =>
            {
                AudioAssistant.PlaySound("BtnClick");
                User.Instance.EnableSound = isOn;
                btnSound.image.sprite = isOn ? musicOn : musicOff;
                //soundBG.sprite = isOn ? soundOn : soundOff;
            });*/
    /*    btnSound.isOn = User.Instance.EnableSound;
        btnSound.image.sprite = User.Instance.EnableSound ? musicOn : musicOff;*/

    // soundBG.sprite = User.Instance.EnableSound ? soundOn : soundOff;

    /*#if UNITY_IOS
            btnRestoreIAP.gameObject.SetActive(User.Instance[ItemID.IAP_Count] > 0);
            btnRestoreIAP.onClick.RemoveAllListeners();
            btnRestoreIAP.onClick.AddListener(OnRestoreIAP);
    #else
            btnRestoreIAP.gameObject.SetActive(false);
    #endif
        }

        void OnRestoreIAP()
        {
            //InAppManager.Instance.RestorePurchases((isSuccess) =>
            //{
            //    if (isSuccess)
            //    {
            //        GameManager.Instance.btnRemoveAds.gameObject.SetActive(false);
            //    }

            //    PopupManager.Instance.OpenPopup<PopupNotice>(PopupID.PopupNotice, (pop) => pop.SetData(
            //        isSuccess ? "Restore success!" : "Restore fail!",
            //        isSuccess ? "All items have been restored" : "Try again"));
            //});
        }*/
    public override void Close()
    {
        base.Close();
    }
}
