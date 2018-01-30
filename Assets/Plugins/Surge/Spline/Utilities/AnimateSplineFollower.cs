using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

/// <summary>
/// Animates an object on a spline at level start.
/// </summary>
[RequireComponent(typeof(Spline))]
public class AnimateSplineFollower : MonoBehaviour {

    #region Variable Declarations
    [SerializeField] Transform objectToAnimate;
    [Range(0, 1)]
    [SerializeField] float startPercentage = 0f;
    [Range(0, 1)]
    [SerializeField] float endPercentage = 1f;
    [SerializeField] bool faceDirection = false;

    [Space]
    [Tooltip("Duration for a full cycle from startPercentange to endPercentage in seconds")]
    [SerializeField] float duration = 5f;
    [Tooltip("Delay in seconds from level start")]
    [SerializeField] float delay = 0f;

    [Space]
    [SerializeField] Tween.LoopType loopType = Tween.LoopType.None;
    [SerializeField] EasingType easingType = EasingType.EaseLinear;
    [Tooltip("Custom easing is used when easingType is set to \"None\"")]
    [SerializeField] AnimationCurve customEasing = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    Spline mySpline;
    #endregion



    #region Unity Event Functions
    private void Start () {
        mySpline = GetComponent<Spline>();

        // Start the Tween with custom easing, if easingType == None
        if (easingType == EasingType.None)
            Tween.Spline(mySpline, objectToAnimate, startPercentage, endPercentage, faceDirection, duration, delay, customEasing, loopType);
        else
            Tween.Spline(mySpline, objectToAnimate, startPercentage, endPercentage, faceDirection, duration, delay, EasingTypeWrapper.GetCurve(easingType), loopType);
    }
	#endregion
}
