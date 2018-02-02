using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes the GameObject (with all it's children) a Singleton.
/// </summary>
public class Singleton : MonoBehaviour 
{

    #region Variable Declarations
    public static Singleton Instance;
    #endregion



    #region Unity Event Functions
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
	#endregion
}
