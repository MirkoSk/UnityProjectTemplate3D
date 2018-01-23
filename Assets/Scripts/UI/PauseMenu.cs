using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : SubscribedBehaviour {

    GameObject pauseMenu,
               mainMenu,
               optionsMenu;
    bool gameIsPaused;
    public bool GameIsPaused { get { return gameIsPaused; } }

	// Use this for initialization
	void Start () {
        pauseMenu = transform.Find("PauseMenu").gameObject;
        mainMenu = pauseMenu.transform.Find("MainMenu").gameObject;
        optionsMenu = pauseMenu.transform.Find("OptionsMenu").gameObject;
    }

    private void Update() {
        if (Input.GetButtonDown(Constants.INPUT_ESCAPE)) {
            if (gameIsPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame() {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        gameIsPaused = true;
    }

    public void ResumeGame() {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
        gameIsPaused = false;
    }
}
