using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Pixelplacement;
using SceneManagement;

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
        SceneManager.Instance.ExitGame();
    }

    public void LoadCredits()
    {
        SceneManager.Instance.LoadLevel(Constants.SCENE_CREDITS);
    }

    public void Play()
    {
        SceneManager.Instance.LoadLevel(Constants.SCENE_LEVEL01);
    }
    #endregion
}
