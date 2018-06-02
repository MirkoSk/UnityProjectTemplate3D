using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class TitleScreen : MonoBehaviour
    {

        #region Variable Declarations
        [SerializeField] float titleScreenDuration = 3f;
        #endregion



        #region Unity Event Functions
        private void Start()
        {

        }

        private void Update()
        {
            if (Time.timeSinceLevelLoad >= titleScreenDuration)
            {
                SceneManager.Instance.LoadLevel(Constants.SCENE_MAIN_MENU);
            }
        }
        #endregion
    }
}