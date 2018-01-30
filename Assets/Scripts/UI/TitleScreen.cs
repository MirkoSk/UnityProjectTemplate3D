using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 
/// </summary>
public class TitleScreen : MonoBehaviour
{

    #region Variable Declarations
    [SerializeField] float titleScreenDuration = 3f;

    float timer;
    bool nextSceneLoaded;
	#endregion
	
	
	
	#region Unity Event Functions
	private void Start() {

	}
	
	private void Update() {
        timer += Time.deltaTime;

        if (timer >= titleScreenDuration && !nextSceneLoaded) {
            GameManager.Instance.LoadNextScene();
            nextSceneLoaded = true;
        }
	}
    #endregion



    #region Private Functions
    #endregion
}
