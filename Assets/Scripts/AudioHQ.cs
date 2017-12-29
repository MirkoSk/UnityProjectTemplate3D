using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Creates an AudioSource for every sound in the assets folder. This class is a singleton and executes in edit mode.
/// </summary>
[ExecuteInEditMode]
public class AudioHQ : SubscribedBehaviour {

    #region Variable Declarations
    [SerializeField] AudioMixerGroup routingGroup;
    AudioClip[] audioClips;
    List<AudioSource> audioSources = new List<AudioSource>();

    public static AudioHQ Instance;
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
            Debug.Log("There can only be one AudioHQ instantiated. Destroying this Instance...");
            DestroyImmediate(this);
        }
    }

    private void OnDisable() {
        Instance = null;
    }
    #endregion



    #region Custom Event Functions
    // Every child of SubscribedBehaviour can implement these
    #endregion



    #region Public Functions
    /// <summary>
    /// Returns an AudioSource by name. Returns null if "name" couldn't be found.
    /// </summary>
    public AudioSource GetAudioSource(string name) {
        // Need to create a new array, because the audioSources list gets cleared when entering Play Mode
        AudioSource[] sources = GetComponents<AudioSource>();
        foreach (AudioSource src in sources) {
            if (src.clip != null && src.clip.name == name) {
                return src;
            }
        }
        Debug.LogError("AudioSource for \"" + name + "\" couldn't be found.");
        return null;
    }

    /// <summary>
    /// Updates the available AudioSources on this GameObject
    /// </summary>
    public void UpdateAudioSources() {
        // Get all sounds in the project
        audioClips = Resources.FindObjectsOfTypeAll<AudioClip>();

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

    /// <summary>
    /// Removes all AudioSources from this GameObject
    /// </summary>
    public void RemoveAllAudioSources() {
        AudioSource[] sources = GetComponents<AudioSource>();
        foreach (AudioSource src in sources) {
            StartCoroutine(destroyAudioSource(src));
        }
        audioSources.Clear();
    }
    #endregion



    #region Private Functions
    /// <summary>
    /// Sets the specified AudioSource to an initial setup state
    /// </summary>
    private void InitializeAudioSource(AudioSource audioSrc, AudioClip clip) {
        audioSrc.outputAudioMixerGroup = routingGroup;
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
