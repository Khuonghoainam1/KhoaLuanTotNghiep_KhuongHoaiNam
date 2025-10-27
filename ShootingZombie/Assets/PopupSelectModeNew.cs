using DanielLochner.Assets.SimpleScrollSnap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class PopupSelectModeNew : MonoBehaviour
{
    public SimpleScrollSnap scrollSnap;

    public void GetModeCenter()
    {
        Debug.Log(scrollSnap.Panels[scrollSnap.CenteredPanel].gameObject.name);
    }
}
