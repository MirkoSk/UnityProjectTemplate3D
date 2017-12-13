﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Custom Behaviour for our game. Every object, that needs to know about the current game state 
/// needs to inherit from this (instead of MonoBehaviour) and implement it's methods
/// </summary>
abstract public class SubscribedBehaviour : MonoBehaviour {

    // Subscribing to all custom game events
    private void OnEnable() {
        GameEvents.Exploration += OnExploration;
    }

    // Unsubscribing from all custom game events
    private void OnDisable() {
        GameEvents.Exploration -= OnExploration;
    }



    /* Virtual functions for every custom game event
     * If overriden, these functions will be called in the inheriting classes when events are triggered
     */
    virtual protected void OnExploration() { }
}