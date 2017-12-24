using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Manages all 2D audio playing in the game. This class is a singleton and executes in edit mode
/// </summary>
[ExecuteInEditMode]
public class AudioManager : SubscribedBehaviour {

    #region Variable Declarations
    // Visible in Inspector
    [SerializeField] AudioMixer masterMixer;
    [SerializeField] AudioMixer nonSpatialMixer;
    [SerializeField] AudioClip[] audioClips;

    List<AudioSource> audioSources;

    public static AudioManager Instance;
    #endregion



    #region Unity Event Functions
    void OnEnable() {
        //Check if instance already exists
        if (Instance == null)

            //if not, set instance to this
            Instance = this;

        //If instance already exists and it's not this:
        else if (Instance != this) {

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of an AudioManager.
            Debug.LogError("There can only be one AudioManager instantiated. Destroying 2nd AudioManager...");
            DestroyImmediate(this);
        }
        audioSources = new List<AudioSource>();
    }

    private void OnDisable() {
        Instance = null;
    }
    #endregion



    #region Custom Event Functions
    // Every child of SubscribedBehaviour can implement these
    #endregion



    #region Public Functions
    public void PlayAudio(string name) {
        GetAudioSource(name).Play();
    }

    public void PlayAudio(string name, bool looped) {
        AudioSource audioSrc = GetAudioSource(name);
        audioSrc.loop = looped;
        audioSrc.Play();
    }

    public void StopAudio(string name) {
        GetAudioSource(name).Stop();
    }

    /// <summary>
    /// Updates the available AudioSources on this GameObject
    /// Should only be called from the AudioManagerEditor class
    /// </summary>
    public void UpdateAudioSources() {
        // All audioClips removed? -> Delete all audioSources and return
        if (audioClips.Length == 0 && audioSources.Count != 0) {
            foreach (AudioSource src in audioSources) {
                StartCoroutine(destroyAudioSource(src));
            }
            audioSources.Clear();
            return;
        }

        // Adding new AudioSources
        // Special case: No AudioSources yet
        if (audioSources.Count == 0) {
            foreach (AudioClip clip in audioClips) {
                gameObject.AddComponent<AudioSource>();
            }
        }
        // Go through all audioClips in the array and add an AudioSource for every clip there isn't already one AudioSource for
        else {
            for (int clip = 0; clip < audioClips.Length; clip++) {
                for (int source = 0; source < audioSources.Count; source++) {
                    if (audioClips[clip] != null && audioSources[source].clip.name == audioClips[clip].name) {
                        break;
                    }
                    if (source == audioSources.Count - 1) {
                        gameObject.AddComponent<AudioSource>();
                    }
                }
            }
        }
        // Populate the list
        GetComponents(audioSources);

        // Deleting unneeded AudioSources
        // Go through all AudioSources and delete every source that don't have a corresponding clip anymore
        for (int source = 0; source < audioSources.Count; source++) {
            for (int clip = 0; clip < audioClips.Length; clip++) {
                if (audioSources[source].clip != null && audioSources[source].clip.name == audioClips[clip].name) {
                    break;
                }
                if (audioSources[source].clip != null && clip == audioClips.Length - 1) {
                    StartCoroutine(destroyAudioSource(audioSources[source]));
                    audioSources.RemoveAt(source);
                }
            }
        }

        // Setup the new AudioSources
        for (int i = 0; i < audioClips.Length; i++) {
            if (audioSources[i].clip == null && audioClips[i] != null) {
                InitializeAudioSource(audioSources[i], audioClips[i]);
            }
        }
    }
    #endregion



    #region Private Functions
    /// <summary>
    /// Returns an AudioSource by name. Returns null if "name" couldn't be found.
    /// </summary>
    private AudioSource GetAudioSource(string name) {
        foreach (AudioSource src in audioSources) {
            if (src.clip.name == name) {
                return src;
            }
        }
        Debug.LogError("AudioSource for \"" + name + "\" couldn't be found.");
        return null;
    }

    /// <summary>
    /// Sets the specified AudioSource to an initial setup state
    /// </summary>
    private void InitializeAudioSource(AudioSource audioSrc, AudioClip clip) {
        audioSrc.outputAudioMixerGroup = nonSpatialMixer.FindMatchingGroups("Unassigned")[0];
        audioSrc.volume = 1f;
        audioSrc.loop = false;
        audioSrc.playOnAwake = false;
        audioSrc.clip = clip;
    }
    #endregion



    #region Coroutines
    /// <summary>
    /// Helper Coroutine, because DestroyImmediate can't be called on OnValidate()
    /// </summary>
    IEnumerator destroyAudioSource(AudioSource src) {
        yield return null;
        DestroyImmediate(src);
    }
    #endregion
}
