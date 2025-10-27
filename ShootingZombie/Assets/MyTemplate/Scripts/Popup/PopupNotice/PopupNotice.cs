using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Thanh.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupNotice : Popup
{
    public TextMeshProUGUI description;
    public TextMeshProUGUI txtTitle;
    public Button btnAds;
    int reward = 1000;
    public void SetData(string title, string _description)
    {
        txtTitle.text = title;
        description.text = _description;

        btnAds.onClick.RemoveAllListeners();
        btnAds.onClick.AddListener(OnAds);
    }

    void OnAds()
    {
        //BuyManager.Instance.Buy(new List<ItemValueFloat> { new ItemValueFloat(ItemID.Ads, 0) }, null, isSuccess =>
        //{
        //    if (isSuccess)
        //    {
        //        User.Instance[ItemID.Gold] += reward;
        //    }
        //    else
        //    {
        //        //PopupManager.Instance.OpenPopup<PopupNotice>(PopupID.PopupNotiOOP, pop => pop.SetData());
        //    }

        //}, AdLocation.PopupNotice);
    }
}
