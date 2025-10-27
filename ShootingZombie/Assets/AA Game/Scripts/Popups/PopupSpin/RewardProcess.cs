using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardProcess : MonoBehaviour
{
    [SerializeField] private GameObject tick;
    [SerializeField] public Image bg;
    [SerializeField] private Sprite spriteBgOff;
    [SerializeField] private Sprite spriteBgOn;
    [SerializeField] private SkeletonGraphic BgrColorl;
    [SerializeField] private GameObject lightning;
    /*    [SerializeField] private Color colorOff;
        [SerializeField] private Color colorOn;*/
    public void SetStatus(bool isOn)
    {
        tick.gameObject.SetActive(isOn);
        lightning.gameObject.SetActive(isOn);
        // bg.color = isOn ? colorOn : colorOff;
        if (isOn == true)
        {
            BgrColorl.AnimationState.SetAnimation(0, "0_yellow", true);
            BgrColorl.Skeleton.SetSlotsToSetupPose();
        }
        else
        {
            BgrColorl.AnimationState.SetAnimation(0, "0_blue", true);
            BgrColorl.Skeleton.SetSlotsToSetupPose();
        }
    }
}