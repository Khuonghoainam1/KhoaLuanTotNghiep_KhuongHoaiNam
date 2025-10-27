using System;
using System.Collections;
using System.Collections.Generic;
using Thanh.Core;
using UnityEngine;

public class Theme_UI : MonoBehaviour
{
    public List<ThemeItems> themeObjectsList;

    public void OnEnable()
    {
        if (BaseScene.currentSceneName == SceneName.SplashScene && BaseScene.Instance is SplashScene)
        {
            BaseScene.theme = (BaseScene.Instance as SplashScene)._theme;
        }

        if(themeObjectsList.Count <= 0)
        {
            return;
        }

        foreach(var item in themeObjectsList)
        {
            if (item != null)
            {
                foreach (var themeItem in item.listObjects)
                {
                    if (themeItem != null)
                    {
                        themeItem.SetActive(item.theme == BaseScene.theme);
                    }
                }
            }
        }
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}

[Serializable]
public class ThemeItems
{
    public Theme theme;
    public List<GameObject> listObjects;
}

public enum Theme
{
    Normal,
    Noel,
}