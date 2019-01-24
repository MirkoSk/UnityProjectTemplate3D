using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu = null;
    [SerializeField] GameObject mainMenu = null;
    [SerializeField] GameObject optionsMenu = null;
    [SerializeField] GameObject resumeButton = null;
    [SerializeField] EventSystem eventSystem = null;

    bool gameIsPaused;
    public bool GameIsPaused { get { return gameIsPaused; } }



    private void Update()
    {
        if (gameIsPaused && !optionsMenu.gameObject.activeSelf && Input.GetButtonDown(Constants.INPUT_CANCEL))
        {
            resumeButton.GetComponent<Button>().onClick.Invoke();
        }

        if (Input.GetButtonDown(Constants.INPUT_ESCAPE))
        {
            if (gameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        mainMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(resumeButton);
        gameIsPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(null);
        gameIsPaused = false;
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("Not implemented yet!");
    }

    public void ExitGame()
    {
        Debug.Log("Exiting the game");
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}