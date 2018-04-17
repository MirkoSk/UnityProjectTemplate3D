using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using SceneManagement;

/// <summary>
/// 
/// </summary>
public class PlayState : State {

    #region Variable Declarations
    [SerializeField] string sceneToLoad;
    #endregion



    #region Unity Event Functions
    private void OnEnable() {
        SceneManager.Instance.LoadLevel(sceneToLoad);
    }

    private void OnDisable() {

    }

    private void Update() {

    }
    #endregion



    #region Public Functions
    #endregion
}
