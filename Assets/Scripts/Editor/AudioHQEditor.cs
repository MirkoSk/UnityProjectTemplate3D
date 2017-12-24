using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 
/// </summary>
[CustomEditor(typeof(AudioHQ))]
public class AudioHQEditor : Editor {

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        AudioHQ hq = (AudioHQ)target;

        if (GUILayout.Button("Update AudioSources")) {
            hq.UpdateAudioSources();
            Repaint();
        }

        if (GUILayout.Button("Remove all AudioSources")) {
            hq.RemoveAllAudioSources();
            Repaint();
        }
    }
}
