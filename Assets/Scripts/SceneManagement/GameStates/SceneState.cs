using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using UnityEngine.SceneManagement;

/// <summary>
/// 
/// </summary>
public class SceneState : State
{
    #region Variable Declarations
    public Dictionary<string, Vector3> pickedUpItems = new Dictionary<string, Vector3>();
    #endregion



    #region Unity Event Functions
    virtual protected void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    virtual protected void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }
	#endregion
	
	
	
	#region Protected Functions
    /// <summary>
    /// This gets called after OnEnable and Awake, but before Start.
    /// </summary>
	virtual protected void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        DespawnItems();
    }

    /// <summary>
    /// Despawns all items that have already been picked up.
    /// </summary>
    protected void DespawnItems()
    {
        foreach (KeyValuePair<string, Vector3> item in pickedUpItems)
        {
            Collider2D collider = Physics2D.OverlapPoint(item.Value);
            if (collider != null && collider.name == item.Key)
            {
                Destroy(collider.gameObject);
            }
        }
    }
    #endregion
}
