using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class MenuAudio : MonoBehaviour {

    #region Variable Declarations

	#endregion
	
	
	
	#region Unity Event Functions
	
    #endregion



    #region Public Functions
    public void PlayConfirm() {
        AudioManager.Instance.PlayMenuConfirm();
    }
	#endregion
}
