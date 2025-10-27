using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectTransformUtil
{
    public static void SetLeft(this RectTransform rect, float left)
    {
        rect.offsetMin = new Vector2(left, rect.offsetMin.y);
    }

    public static float GetLeft(this RectTransform rect)
    {
        return rect.offsetMin.x;
    }

    public static void SetBottom(this RectTransform rect, float val)
    {
        rect.offsetMin = new Vector2(rect.offsetMin.x, val);
    }

    public static float GetBottom(this RectTransform rect)
    {
        return rect.offsetMin.y;
    }

    public static void SetRight(this RectTransform rect, float val)
    {
        rect.offsetMax = new Vector2(val, rect.offsetMax.y);
    }

    public static float GetRight(this RectTransform rect)
    {
        return rect.offsetMax.x;
    }

    public static void SetTop(this RectTransform rect, float val)
    {
        rect.offsetMax = new Vector2(rect.offsetMax.x, val);
    }

    public static float GetTop(this RectTransform rect)
    {
        return rect.offsetMax.y;
    }
}