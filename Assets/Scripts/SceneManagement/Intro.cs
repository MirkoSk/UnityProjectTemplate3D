using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class Intro : MonoBehaviour
    {

        #region Variable Declarations
        [SerializeField] float introDuration = 3f;
        #endregion



        #region Unity Event Functions
        private void Start()
        {

        }

        private void Update()
        {
            if (Time.timeSinceLevelLoad >= introDuration)
            {
                SceneManager.Instance.LoadLevel(Constants.SCENE_LEVEL01);
            }
        }
        #endregion
    }
}