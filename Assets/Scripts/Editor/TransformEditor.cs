using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 
/// </summary>
[CustomEditor(typeof(Transform))]
public class TransformEditor : Editor {

    public override void OnInspectorGUI() {

        Transform transform = (Transform)target;

        transform.localPosition = EditorGUILayout.Vector3Field("Position", transform.localPosition);
        transform.localRotation = Quaternion.Euler(EditorGUILayout.Vector3Field("Rotation", transform.localRotation.eulerAngles));
        transform.localScale = EditorGUILayout.Vector3Field("Scale", transform.localScale);

        if (GUILayout.Button("Reset Transform")) {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
    }

}
