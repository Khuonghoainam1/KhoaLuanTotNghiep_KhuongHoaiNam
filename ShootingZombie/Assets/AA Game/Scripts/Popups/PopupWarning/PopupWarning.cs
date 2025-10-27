using System.Collections;
using Thanh.Core;
using UnityEngine;

public class PopupWarning : Popup
{
    private void OnValidate()
    {
        popupID = PopupID.PopupWarning;
    }
    public override void OnShow()
    {
        base.OnShow();
        StartCoroutine(WaitClose());
    }
    IEnumerator WaitClose()
    {
        yield return new WaitForSeconds(1.5f);
        Close();
    }
}
