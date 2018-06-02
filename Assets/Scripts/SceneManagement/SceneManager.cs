using UnityEngine;
using UnityEngine.SceneManagement;
using Pixelplacement;

namespace SceneManagement
{
    /// <summary>
    /// Manages the overall flow of the game and scene loading. This class is a singleton and won't be destroyed when loading a new scene.
    /// </summary>
    public class SceneManager : Utility.Singleton<SceneManager>
    {

        #region Variable Declarations

        #endregion



        #region Unity Event Functions
        private void Awake()
        {
            RegisterSingleton(this);
        }


        override protected void SubscribedOnEnable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnLevelLoaded;
        }


        override protected void SubscribedOnDisable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnLevelLoaded;
        }


        private void Start()
        {
#if !UNITY_EDITOR
        // Lock and hide cursor in built game
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
#endif
        }


        private void Update()
        {

        }
        #endregion



        #region Custom Event Functions
        // Every child of SubscribedBehaviour can implement these
        #endregion



        #region Public Functions
        /// <summary>
        /// Loads a scene by name. This should only be called by methods within State classes.
        /// </summary>
        /// <param name="name">Name of the scene to load. Use Constants class.</param>
        public void LoadLevel(string name)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(name);
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
}