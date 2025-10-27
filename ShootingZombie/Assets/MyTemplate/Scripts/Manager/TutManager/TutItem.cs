/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutItem : MonoBehaviour, IPointerClickHandler
{
    public TutUI tutUI;
    public RectTransform point;

    void OnEnable()
    {
        GameEvent.OnShowTutUI.AddListener(OnShowTutUI);
    }

    void OnShowTutUI(TutUI tutID, bool showHand)
    {
        if (tutUI != tutID)
        {
            return;
        }

        TutorialManager.Instance.ShowHand(showHand, point.position);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        GameEvent.OnClickTutUI?.Invoke(tutUI);
    }
}*/