using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Pixelplacement;

/// <summary>
/// 
/// </summary>
public class MainMenu : MonoBehaviour
{

    #region Variable Declarations
    [SerializeField] GameObject playButton;
    [SerializeField] DisplayObject mainMenu;

    int howToPlay;
    //EventSystem eventSystem;
	#endregion
	
	
	
	#region Unity Event Functions
	private void Start() {
        //eventSystem = GameObject.FindObjectOfType<EventSystem>();

        mainMenu.SetActive(true);
	}
    #endregion



    #region Public Functions
    public void ExitGame() {
        GameManager.Instance.ExitGame();
    }

    public void LoadCredits()
    {
        GameManager.Instance.ChangeGameState(GameState.Credits);
    }

    public void Play()
    {
        GameManager.Instance.ChangeGameState(GameState.Play);
    }
    #endregion
}
