using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemFX : Singleton<ItemFX>
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasScaler canvasScaler;
    public void OnSetupCamera(Camera camera)
    {
        canvas.worldCamera = camera;
        canvas.planeDistance = 2;
        canvas.sortingLayerName = "UI";
        canvas.sortingOrder = 150;
        if (camera.aspect >= 2.1f)
        {
            canvasScaler.matchWidthOrHeight = 1;
        }
        else
        {
            canvasScaler.matchWidthOrHeight = 0;
        }
    }
}
