using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class SetGameState : MonoBehaviour 
{

    #region Variable Declarations
    [Tooltip("The GameState this scene shall have.")]
    [SerializeField] GameState gameState;
	#endregion
	
	
	
	#region Unity Event Functions
	private void Start () 
	{
        print("Setting gamestate");
        GameManager.Instance.ChangeGameState(gameState);
	}
    #endregion
}
