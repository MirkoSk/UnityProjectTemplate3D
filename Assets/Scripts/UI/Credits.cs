using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Credits : MonoBehaviour {
	
	#region Variable Declarations
	#endregion
	
	
	
	#region Unity Event Functions	
	private void Update() {
        if (Input.GetButtonDown(Constants.INPUT_ESCAPE) || Input.GetButtonDown(Constants.INPUT_CANCEL)) {
            GameManager.Instance.LoadScene(Constants.SCENE_MAIN_MENU);
        }
	}
    #endregion



    #region Private Functions
    #endregion
}
