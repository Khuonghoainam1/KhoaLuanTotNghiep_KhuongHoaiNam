using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class SwichToggle : MonoBehaviour
{
    [SerializeField] RectTransform uiHandleRectTransform;
    [SerializeField] Color backgroundActiveColor;
    [SerializeField] Color handleActiveColor;
    public toggleName nameToggle;
    Image backgroundImage, handleImage;

    Color backgroundDefaultColor, handleDefaultColor;

    Toggle toggle;

    Vector2 handlePosition;

    void Awake()
    {
        toggle = GetComponent<Toggle>();
        if (nameToggle == toggleName.Mussic)
        {
            //toggle.isOn = User.Instance.EnableMusic;
        }
        else if (nameToggle == toggleName.Sound)
        {
            //toggle.isOn = User.Instance.EnableSound;
        }

        

        handlePosition = uiHandleRectTransform.anchoredPosition;
       
        backgroundImage = uiHandleRectTransform.parent.GetComponent<Image>();
        handleImage = uiHandleRectTransform.GetComponent<Image>();

        backgroundDefaultColor = backgroundImage.color;
        handleDefaultColor = handleImage.color;

        toggle.onValueChanged.AddListener(OnSwitch);

        if (toggle.isOn)
            OnSwitch(true);
        // AudioAssistant.PlaySound("BtnClick");
             AudioManager.instance.Play("BtnClick");

    }

    void OnSwitch(bool on)
    {
        uiHandleRectTransform.DOAnchorPos(on ? handlePosition * -1 : handlePosition, .4f).SetEase(Ease.InOutBack);

        backgroundImage.DOColor(on ? backgroundActiveColor : backgroundDefaultColor, .6f);
        handleImage.DOColor(on ? handleActiveColor : handleDefaultColor, .4f);
        if (nameToggle == toggleName.Mussic)
        {
            User.Instance[ItemID.Music] = 1;
            //  AudioAssistant.PlaySound("BtnClick");
            AudioManager.instance.Play("BtnClick");
            //User.Instance.EnableMusic = on;
            //toggle.isOn = User.Instance.EnableMusic;
        
        }
        else if(nameToggle == toggleName.Sound)
        {
           // User.Instance[ItemID.Sound] = 1;
            //  AudioAssistant.PlaySound("BtnClick");
            AudioManager.instance.Play("BtnClick");
            //User.Instance.EnableSound = on;
            //toggle.isOn = User.Instance.EnableSound;
            
        }
        //AudioManager.instance.MuteAdio(on);
    }


    void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnSwitch);
    }
}
public enum toggleName
{
    Mussic,
    Sound,
}