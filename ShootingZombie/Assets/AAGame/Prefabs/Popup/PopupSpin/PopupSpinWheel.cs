using EasyUI.PickerWheelUI;
using System.Collections;
using System.Collections.Generic;
using Thanh.Core;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupSpinWheel : Popup
{
    [SerializeField] private Button StartSpin;
    [SerializeField] private Button EndSpin;
    [SerializeField] private TMP_Text CancelSpin;
    [SerializeField] private PickerWheel PickerSpin;

    private void OnEnable()
    {
        StartSpin.onClick.AddListener(() =>
        {
      
            PickerSpin.OnSpinStart(() =>
            {
                Debug.Log("SpinStart");
                StartSpin.gameObject.SetActive(false);
            });
            PickerSpin.OnSpinEnd(WheelPiece =>
            {
                Debug.Log("End Spin" + WheelPiece.Label + " , amout " + WheelPiece.Amount  );
                StartSpin.gameObject.SetActive(true);
            });
            PickerSpin.Spin();

        });

    }
}
