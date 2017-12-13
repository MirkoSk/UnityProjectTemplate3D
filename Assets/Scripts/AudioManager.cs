using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages all audio playing in the game. This class is a singleton and won't be destroyed when loading a new scene.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AudioManager : SubscribedBehaviour {

    #region Variable Declarations
    // Visible in Inspector
    //[SerializeField] AudioClip track01;

    public static AudioManager Instance;

    // Private Variables
    private AudioSource audioSrc;
    #endregion



    #region Unity Event Functions
    //Awake is always called before any Start functions
    void Awake() {
        //Check if instance already exists
        if (Instance == null)

            //if not, set instance to this
            Instance = this;

        //If instance already exists and it's not this:
        else if (Instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of an AudioManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        audioSrc = GetComponent<AudioSource>();
	}
	
	private void Update() {
		
	}
    #endregion



    #region Custom Event Functions
    // Every child of SubscribedBehaviour can implement these
    #endregion



    #region Private Functions
    /// <summary>
    /// Plays an audio clip with defined parameters
    /// </summary>
    /// <param name="clip">AudioClip to be played</param>
    /// <param name="looped">Shall the playback be looped?</param>
    private void PlayAudio(AudioClip clip, bool looped) {
        audioSrc.loop = looped;
        audioSrc.clip = clip;
        audioSrc.Play();
    }

    private void StopAudio() {
        iTween.AudioTo(gameObject, iTween.Hash("volume", 0f, "time", 2f, "easetype", iTween.EaseType.easeInOutQuad, "oncomplete", "ResetAudio"));
    }

    private void ResetAudio() {
        audioSrc.clip = null;
        audioSrc.volume = 1f;
        audioSrc.loop = false;
        audioSrc.clip = null;
    }
    #endregion
}
