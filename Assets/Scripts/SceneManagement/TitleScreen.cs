using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class TitleScreen : MonoBehaviour {

    #region Variable Declarations
    [SerializeField] float titleScreenDuration = 3f;
	#endregion
	
	
	
	#region Unity Event Functions
	private void Start () {
		
	}
	
	private void Update () {
        if (Time.timeSinceLevelLoad >= titleScreenDuration)
        {
            GameManager.Instance.ChangeGameState(GameState.MainMenu);
        }
    }
	#endregion
}
