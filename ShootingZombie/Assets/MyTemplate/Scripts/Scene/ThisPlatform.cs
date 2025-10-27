using System;
using UnityEngine;
/// <summary>
/// Source: https://gist.github.com/ChrisNZL/1d769a0010cd5c9ec79bd6f5b44cf611
/// </summary>
public class ThisPlatform
{

    // This property allows you to use ThisPlatform.IsIphoneX to determine if you should do anything special in your code when checking for iPhone X.
    public static bool IsIphoneX
    {

        get
        {
#if UNITY_ANDROID
            float ratio = (float)Screen.height / (float)Screen.width;
            if (ratio > 1.8f && ratio < 2.1f)
            {
                return false;
            }
            else if (ratio >= 2.1f)
            {
                return true;
            }
#endif
#if UNITY_IOS

            // If testing without an iPhone X, add FORCE_IPHONEX to your Scripting Define Symbols.
            // Useful when using Xcode's Simulator, or on any other device that is not an iPhone X.
#if FORCE_IPHONEX
					return true;
#else
            // iOS.Device.generation doesn't reliably report iPhone X (sometimes it's "unknown"), but it's there in case it ever properly works.
            if (UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhoneX || UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhoneXS
        || UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhoneXR || UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhoneXSMax)
            {
                return true;
            }

            // As a last resort to see if the device is iPhone X, check the reported device model/identifier.
            string deviceModel = SystemInfo.deviceModel;
            if (deviceModel == IphoneX.Identifier_A || deviceModel == IphoneX.Identifier_B
                || deviceModel == IphoneX.Identifier_C || deviceModel == IphoneX.Identifier_D
                || deviceModel == IphoneX.Identifier_E || deviceModel == IphoneX.Identifier_F)
            {
                return true;
            }
            return false;
#endif
#else
            return false;
#endif

        }
    }

    public static bool IsTablet()
    {
        float rateScreen = ((float)Screen.width / Screen.height);
        return Math.Round(rateScreen, 3) == 0.75f;
    }
}

#if UNITY_IOS

public class IphoneX
{

    // If unable to use Unity's Screen.safeArea, or if you just don't want to, the following values should help with your calculations to keep elements away from the screen edges (assuming the phone is landscape here).
    public const float SideMarginPercentage = .055f;
    public const float BottomMarginPercentage = .0827f;
    public const float HomeButtonWidthPercentage = .256f;

    // Device identifiers from https://www.theiphonewiki.com/wiki/List_of_iPhones
    public const string Identifier_A = "iPhone10,3";
    public const string Identifier_B = "iPhone10,6";
    public const string Identifier_C = "iPhone11,8";
    public const string Identifier_D = "iPhone11,2";
    public const string Identifier_E = "iPhone11,4";
    public const string Identifier_F = "iPhone11,6";

}

#endif