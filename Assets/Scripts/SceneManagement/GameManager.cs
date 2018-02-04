using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pixelplacement;

public enum GameState { Title, MainMenu, Intro, Play, Credits }

/// <summary>
/// Manages the overall flow of the game and scene loading. This class is a singleton and won't be destroyed when loading a new scene.
/// </summary>
[RequireComponent(typeof(StateMachine))]
public class GameManager : SubscribedBehaviour
{

    #region Variable Declarations
    public static GameManager Instance;

    StateMachine gameStateMachine;
    GameState currentGameState;
    public GameState CurrentGameState { get { return currentGameState; } }
    #endregion



    #region Unity Event Functions
    //Awake is always called before any Start functions
    private void Awake()
    {
        //Check if instance already exists
        if (Instance == null)

            //if not, set instance to this
            Instance = this;

        //If instance already exists and it's not this:
        else if (Instance != this)
        {

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }
    }

    override protected void SubscribedOnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    private void Start()
    {
        gameStateMachine = GetComponent<StateMachine>();
        
        #if !UNITY_EDITOR
        // Lock and hide cursor in built game
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        #endif
    }

    private void Update()
    {

    }

    override protected void SubscribedOnDisable()
    {
        base.OnDisable();

        SceneManager.sceneLoaded -= OnLevelLoaded;
    }
    #endregion



    #region Custom Event Functions
    // Every child of SubscribedBehaviour can implement these
    #endregion



    #region Public Functions
    /// <summary>
    /// Switches the current state of the game to the specified GameState.
    /// </summary>
    public void ChangeGameState(GameState state)
    {
        switch (state)
        {
            case GameState.Title:
                gameStateMachine.ChangeState("Title");
                currentGameState = GameState.Title;
                break;
            case GameState.MainMenu:
                gameStateMachine.ChangeState("MainMenu");
                currentGameState = GameState.MainMenu;
                break;
            case GameState.Intro:
                gameStateMachine.ChangeState("Intro");
                currentGameState = GameState.Intro;
                break;
            case GameState.Play:
                gameStateMachine.ChangeState("Play");
                currentGameState = GameState.Play;
                break;
            case GameState.Credits:
                gameStateMachine.ChangeState("Credits");
                currentGameState = GameState.Credits;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Loads a scene by name. This should only be called by methods within State classes.
    /// </summary>
    /// <param name="name">Name of the scene to load. Use Constants class.</param>
    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }

    /// <summary>
    /// Quits the application or exits play mode when in editor.
    /// </summary>
    public void ExitGame()
    {
        Debug.Log("Exiting the game");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    #endregion



    #region Private Functions
    /// <summary>
    /// This gets called after OnEnable() and Awake(), but before Start() on every scene load.
    /// </summary>
    void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {

    }
    #endregion
}
