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

        EditorGUILayout.Vector3Field("Position", transform.position);
        EditorGUILayout.Vector3Field("Rotation", transform.rotation.eulerAngles);
        EditorGUILayout.Vector3Field("Scale", transform.localScale);

        if (GUILayout.Button("Reset Transform")) {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
    }

}
