using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class Credits : MonoBehaviour
    {

        #region Variable Declarations

        #endregion



        #region Unity Event Functions
        private void Start()
        {

        }

        private void Update()
        {
            if (Input.GetButtonDown(Constants.INPUT_CANCEL) || Input.GetButtonDown(Constants.INPUT_ESCAPE))
            {
                SceneManager.Instance.LoadLevel(Constants.SCENE_MAIN_MENU);
            }
        }
        #endregion



        #region Public Functions

        #endregion



        #region Private Functions

        #endregion
    }
}