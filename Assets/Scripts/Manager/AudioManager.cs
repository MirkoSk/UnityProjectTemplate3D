﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Offers functions for playing 2D audio. This class is a singleton and won't be destroyed on load
/// </summary>
public class AudioManager : SubscribedBehaviour {

    #region Variable Declarations
    // Visible in Inspector
    [SerializeField] AudioMixer masterMixer;
    public AudioMixer MasterMixer { get { return masterMixer; } }

    public static AudioManager Instance;
    #endregion



    #region Unity Event Functions
    void Awake() {
        //Check if instance already exists
        if (Instance == null)

            //if not, set instance to this
            Instance = this;

        //If instance already exists and it's not this:
        else if (Instance != this) {

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of an AudioManager.
            Debug.Log("There can only be one AudioManager instantiated. Destroying this Instance...");
            Destroy(this);
        }
    }
    #endregion



    #region Custom Event Functions
    // Every child of SubscribedBehaviour can implement these
    #endregion



    #region Public Functions
    public void PlayAudio(string name) {
        AudioHQ.Instance.GetAudioSource(name).Play();
    }

    public void PlayAudio(string name, float fadeInTime) {
        AudioHQ.Instance.GetAudioSource(name).Play(fadeInTime);
    }

    public void StopAudio(string name) {
        AudioHQ.Instance.GetAudioSource(name).Stop();
    }

    public void StopAudio(string name, float fadeOutTime) {
        AudioHQ.Instance.GetAudioSource(name).Stop(fadeOutTime);
    }

    public void SetMusicVolume(float volume) {
        masterMixer.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume) {
        masterMixer.SetFloat("SFXVolume", volume);
    }
    #endregion



    #region Private Functions

    #endregion



    #region Coroutines

    #endregion
}