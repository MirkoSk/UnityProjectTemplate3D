using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

namespace SceneManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class CreditsState : State
    {

        #region Variable Declarations

        #endregion



        #region Unity Event Functions
        private void OnEnable()
        {
            SceneManager.Instance.LoadLevel(Constants.SCENE_CREDITS);
        }

        private void OnDisable()
        {

        }

        private void Update()
        {

        }
        #endregion
    }
}
