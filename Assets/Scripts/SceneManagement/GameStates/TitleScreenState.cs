using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

/// <summary>
/// 
/// </summary>
public class TitleScreenState : State {

    #region Variable Declarations

    #endregion



    #region Unity Event Functions
    private void OnEnable() {
        GameManager.Instance.LoadLevel(Constants.SCENE_TITLE);
    }

    private void OnDisable() {

    }

    private void Update() {

    }
    #endregion
}
