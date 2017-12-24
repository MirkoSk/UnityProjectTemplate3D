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


}
