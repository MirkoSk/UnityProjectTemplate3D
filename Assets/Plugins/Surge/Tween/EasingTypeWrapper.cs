using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public enum EasingType
{
    None,
    EaseLinear,
    EaseIn,
    EaseInStrong,
    EaseOut,
    EaseOutStrong,
    EaseInOut,
    EaseInOutStrong,
    EaseInBack,
    EaseOutBack,
    EaseInOutBack,
    EaseSpring,
    EaseBounce,
    EaseWobble
}

/// <summary>
/// 
/// </summary>
public class EasingTypeWrapper
{

    
    public static AnimationCurve GetCurve(EasingType easingType)
    {
        switch (easingType)
        {
            case EasingType.EaseLinear:
                return Tween.EaseLinear;
            case EasingType.EaseIn:
                return Tween.EaseIn;
            case EasingType.EaseInBack:
                return Tween.EaseInBack;
            case EasingType.EaseInOut:
                return Tween.EaseInOut;
            case EasingType.EaseInOutBack:
                return Tween.EaseInOutBack;
            case EasingType.EaseInOutStrong:
                return Tween.EaseInOutStrong;
            case EasingType.EaseInStrong:
                return Tween.EaseInStrong;
            case EasingType.EaseOut:
                return Tween.EaseOut;
            case EasingType.EaseOutBack:
                return Tween.EaseOutBack;
            case EasingType.EaseOutStrong:
                return Tween.EaseOutStrong;
            case EasingType.EaseSpring:
                return Tween.EaseSpring;
            case EasingType.EaseWobble:
                return Tween.EaseWobble;
            default:
                return null;
        }
    }
}
