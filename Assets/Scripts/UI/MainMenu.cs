using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 
/// </summary>
public class MainMenu : MonoBehaviour
{

    #region Variable Declarations
    [SerializeField] GameObject playButton;

    int howToPlay;
    //EventSystem eventSystem;
	#endregion
	
	
	
	#region Unity Event Functions
	private void Start() {
        //eventSystem = GameObject.FindObjectOfType<EventSystem>();
	}
    #endregion



    #region Public Functions
    public void ExitGame() {
        GameManager.Instance.ExitGame();
    }

    public void LoadCredits()
    {
        GameManager.Instance.LoadScene(Constants.SCENE_CREDITS);
    }

    public void Play()
    {
        GameManager.Instance.LoadNextScene();
    }
    #endregion
}
