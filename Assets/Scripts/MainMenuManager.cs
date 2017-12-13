using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 
/// </summary>
public class MainMenuManager : SubscribedBehaviour {

    #region Variable Declarations
    public static MainMenuManager Instance;
    #endregion



    #region Unity Event Functions
    private void Awake() {

        //Check if instance already exists
        if (Instance == null) {
            //if not, set instance to this
            Instance = this;
        }

        //If instance already exists and it's not this:
        else if (Instance != this) {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a DebugMode.
            Destroy(gameObject);
        }
    }
    #endregion



    #region Public Functions
    /// <summary>
    /// Calls the LoadNextLevel function from the GameManager
    /// </summary>
    public void Play() {
        GameManager.Instance.LoadNextLevel();
    }

    public void LoadOptions() {
        Debug.LogError("Tell the guy who made this template to implement a proper options menu already.");
    }

    /// <summary>
    /// Calls the ExitGame function from the GameManager
    /// </summary>
    public void ExitGame() {
        GameManager.Instance.ExitGame();
    }
	#endregion
}
