using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Data Structure defining the elements of an audio track.
/// </summary>
[CreateAssetMenu]
public class AudioTrack : ScriptableObject
{
    public AudioClip clip;
    public AudioMixerGroup output;
    public bool mute = false;
    public bool bypassEffects = false;
    public bool bypassListenerEffects = false;
    public bool playOnAwake = false;
    public bool loop = false;
    [Range(0, 256)]
    public int priority = 128;
    [Range(0, 1)]
    public float volume = 1f;
    [Range(-3, 3)]
    public float pitch = 1f;
    [Range(-1, 1)]
    public float stereoPan = 0f;
}