using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 
/// </summary>
[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor {

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        AudioManager manager = (AudioManager)target;

        if (GUILayout.Button("Update AudioSources")) {
            manager.UpdateAudioSources();
        }
    }

}
