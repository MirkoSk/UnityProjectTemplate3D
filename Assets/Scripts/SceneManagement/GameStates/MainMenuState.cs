using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

/// <summary>
/// 
/// </summary>
public class MainMenuState : State {

    #region Variable Declarations

    #endregion



    #region Unity Event Functions
    private void OnEnable() {
        GameManager.Instance.LoadLevel(Constants.SCENE_MAIN_MENU);
    }

    private void OnDisable() {
        
    }

    private void Update() {
        
    }
    #endregion
}
