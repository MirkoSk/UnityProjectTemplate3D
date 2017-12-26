using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods {

    #region Transform
    /// <summary>
    /// Looks for components of type T with specified Tag. Returns the first component of type T found.
    /// </summary>
    public static T FindComponentInChildrenWithTag<T>(this Transform parent, string tag) where T : Component {
        Transform[] children = parent.GetComponentsInChildren<Transform>();
        for (int i = 0; i < children.Length; i++) {
            if (children[i].tag == tag) {
                return children[i].GetComponent<T>();
            }
        }
        return null;
    }

    /// <summary>
    /// Looks for components of type T with specified Tag. Returns all components of type T found.
    /// </summary>
    public static T[] FindComponentsInChildrenWithTag<T>(this Transform parent, string tag) where T : Component {
        Transform[] children = parent.GetComponentsInChildren<Transform>();
        List<T> list = new List<T>();
        for (int i = 0; i < children.Length; i++) {
            if (children[i].tag.Contains(tag)) {
                list.Add(children[i].GetComponent<T>());
            }
        }
        T[] returnArray = new T[list.Count];
        list.CopyTo(returnArray);
        return returnArray;
    }
    #endregion



    #region AudioSource
    /// <summary>
    /// Plays the clip with a specified Fade-In time.
    /// </summary>
    /// <param name="fadeInTime">Length of the Fade-In in seconds</param>
    public static void Play(this AudioSource source, float fadeInTime) {
        float originalVolume = source.volume;
        source.volume = 0f;
        source.Play();
        LeanTween.value(source.gameObject, (float f) => { source.volume = f; }, 0f, originalVolume, fadeInTime)
                 .setEase(LeanTweenType.easeInOutQuad);
    }

    /// <summary>
    /// Stops playing the clip with a specified Fade-Out time.
    /// </summary>
    /// <param name="fadeOutTime">Length of the Fade-Out in seconds</param>
    public static void Stop(this AudioSource source, float fadeOutTime) {
        float originalVolume = source.volume;
        LeanTween.value(source.gameObject, (float f) => { source.volume = f; }, originalVolume, 0f, fadeOutTime)
                 .setEase(LeanTweenType.easeInOutQuad)
                 .setOnComplete(() => {
                     source.Stop();
                     source.volume = originalVolume;
                 });
    }
    #endregion



    #region Camera
    /// <summary>
    /// Fades in the screen from an image. You need to add an Image to a Canvas and tag it with "FadeOutImage" for this to work.
    /// </summary>
    /// <param name="fadeInTime">Time in seconds the camera shall take to fade in</param>
    public static void FadeIn(this Camera camera, float fadeInTime) {
        RectTransform fadeOutImage = GameObject.FindGameObjectWithTag(Constants.TAG_FADE_OUT_IMAGE).GetComponent<RectTransform>();

        // Abort, if no object was found
        if (fadeOutImage == null) {
            Debug.LogError("Camera Fade-Out aborting. No GameObject with tag " + Constants.TAG_FADE_OUT_IMAGE + "found");
            return;
        }

        LeanTween.alpha(fadeOutImage, 0f, fadeInTime).setEase(LeanTweenType.easeInOutQuad);
        fadeOutImage.GetComponent<UnityEngine.UI.Image>().enabled = false;
    }

    /// <summary>
    /// Fades the screen smoothly to an image. You need to add an Image to a Canvas and tag it with "FadeOutImage" for this to work.
    /// </summary>
    /// <param name="fadeOutTime">Time in seconds the camera shall take to fade out</param>
    public static void FadeOut(this Camera camera, float fadeOutTime) {
        RectTransform fadeOutImage = GameObject.FindGameObjectWithTag(Constants.TAG_FADE_OUT_IMAGE).GetComponent<RectTransform>();

        // Abort, if no object was found
        if (fadeOutImage == null) {
            Debug.LogError("Camera Fade-Out aborting. No GameObject with tag " + Constants.TAG_FADE_OUT_IMAGE + "found");
            return;
        }

        fadeOutImage.GetComponent<UnityEngine.UI.Image>().enabled = true;
        LeanTween.alpha(fadeOutImage, 1f, fadeOutTime).setEase(LeanTweenType.easeInOutQuad);
    }

    public static void FadeOut(this Camera camera, GameObject fadeOutPlane, float fadeOutTime, System.Action<object> onComplete, object onCompleteParam) {

    }
    #endregion
}
