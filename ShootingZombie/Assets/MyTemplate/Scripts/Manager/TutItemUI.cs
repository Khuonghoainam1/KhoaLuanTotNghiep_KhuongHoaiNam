/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class TutItemUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private IDTutUI idTutUI;
    [SerializeField] private RectTransform point;

    private RectTransform rect;
    private Transform parent;
    private int index;

    // Gia tri Goc
    private Vector2 anchorMin;
    private Vector2 anchorMax;
    private float left;
    private float right;
    private float top;
    private float bottom;

    public void SetupItem(IDTutUI id)
    {
        idTutUI = id;
        point = GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        GameEvent.OnShowTutorialUI.AddListener(OnShowTutUI);
        GameEvent.OnClickTutorialUI.AddListener(OnclickTutorial);
        parent = transform.parent;
        index = transform.GetSiblingIndex();

        rect = GetComponent<RectTransform>();
        anchorMax = rect.anchorMax;
        anchorMin = rect.anchorMin;
        left = rect.GetLeft();
        right = rect.GetRight();
        top = rect.GetTop();
        bottom = rect.GetBottom();
    }

    private void OnShowTutUI(IDTutUI idTut)
    {
        if (idTut != idTutUI) return;
        transform.SetParent(TutorialManager.Instance.blockRaycat.transform);
        TutorialManager.Instance.ShowHand(point.position, true);
    }

    private void OnclickTutorial(IDTutUI idTut)
    {
        if (idTut != idTutUI) return;
        TutorialManager.Instance.KillHandObj();
        transform.SetParent(parent);
        transform.SetSiblingIndex(index);
        rect.anchorMax = anchorMax;
        rect.anchorMin = anchorMin;
        rect.SetLeft(left);
        rect.SetRight(right);
        rect.SetTop(top);
        rect.SetBottom(bottom);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameEvent.OnClickTutorialUI?.Invoke(idTutUI);
    }
}

public enum IDTutUI
{
    ClickWeaponMenu,
    ClickTabFuse,
    ClickFuse,
    ClickWeaponGame,
    Click_Tab1,
    Click_Tab2,
    Click_Tab3,
}*/