using System.Collections;
using System.Collections.Generic;
using Thanh.Core;
using UnityEngine;

public class PopupTryHero : Popup
{
    public override void Close()
    {
        base.Close();
        GameManager.Instance.isSelectTryHero = true;
    }
}
