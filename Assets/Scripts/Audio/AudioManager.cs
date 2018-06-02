using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Offers functions for playing 2D audio. This class is a singleton and won't be destroyed on load
/// </summary>
public class AudioManager : Utility.Singleton<AudioManager> {

    #region Variable Declarations
    // Visible in Inspector
    [Space]
    [SerializeField] AudioMixer masterMixer;
    public AudioMixer MasterMixer { get { return masterMixer; } }
    [SerializeField] AudioTrack[] audioTracks;

    Dictionary<string, AudioSource> audioSources = new Dictionary<string, AudioSource>();
    #endregion



    #region Unity Event Functions
    void Awake() {
        RegisterSingleton(this);
    }

    private void Start()
    {
        SpawnAudioSources();
        FillDictionary();
    }
    #endregion



    #region Custom Event Functions

    #endregion



    #region Public Functions
    public void PlayMenuConfirm() {
        AudioSource src;
        audioSources.TryGetValue("MenuConfirm", out src);
        src.Play();
    }

    public void SetMusicVolume(float volume)
    {
        masterMixer.SetFloat(Constants.MIXER_MUSIC_VOLUME, volume);
    }

    public void SetSFXVolume(float volume)
    {
        masterMixer.SetFloat(Constants.MIXER_SFX_VOLUME, volume);
    }
    #endregion



    #region Private Functions
    void SpawnAudioSources()
    {
        foreach (AudioTrack track in audioTracks)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = track.clip;
            source.outputAudioMixerGroup = track.output;
            source.playOnAwake = track.playOnAwake;
            source.loop = track.loop;
            source.volume = track.volume;
        }
    }

    void FillDictionary()
    {
        AudioSource[] sources = gameObject.GetComponents<AudioSource>();
        foreach (AudioSource source in sources)
        {
            audioSources.Add(source.clip.name, source);
        }
    }
    #endregion



    #region Coroutines

    #endregion
}
