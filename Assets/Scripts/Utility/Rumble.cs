using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

/// <summary>
/// Class providing functions to make an XBox Controller Rumble
/// </summary>
public class Rumble : SubscribedBehaviour {

    #region Variable Declarations
    Coroutine rumbleCoroutine;
    float timer;

    public static Rumble Instance;
    #endregion



    #region Unity Event Functions
    //Awake is always called before any Start functions
    void Awake() {
        //Check if instance already exists
        if (Instance == null)

            //if not, set instance to this
            Instance = this;

        //If instance already exists and it's not this:
        else if (Instance != this) {

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of an AudioManager.
            Debug.Log("There can only be one Rumble class instantiated. Destroying this Instance...");
            Destroy(this);
        }
    }

    private void Start () {
		
	}
	
	private void Update () {
        if (timer == 0) RumbleConstant(PlayerIndex.One, -1f, 1f, 1f);
        if (ExtensionMethods.InRange(timer, 2f, 0.05f)) RumbleConstant(PlayerIndex.One, 1f, 1f, 1f);
        if (ExtensionMethods.InRange(timer, 4f, 0.05f)) RumbleConstant(PlayerIndex.One, 1f, 12348f, 0f);
        if (ExtensionMethods.InRange(timer, 6f, 0.05f)) RumbleConstant(PlayerIndex.One, 1f, 0f, 12348f);

        timer += Time.deltaTime;
	}
    #endregion



    #region Public Functions
    /// <summary>
    /// Controller-Rumble for one second with both motors constantly at max velocity.
    /// </summary>
    /// <param name="playerIndex">PlayerIndex of the controller that shall rumble.</param>
    public void RumbleConstant(PlayerIndex playerIndex) {
        if (CheckCallForValidity()) {
            rumbleCoroutine = StartCoroutine(RumbleConstantCoroutine(playerIndex, 1f, 1f, 1f));
        }
    }
    
    /// <summary>
    /// Controller-Rumble with both motors constantly at max velocity.
    /// </summary>
    /// <param name="playerIndex">PlayerIndex of the controller that shall rumble.</param>
    /// <param name="duration">Duration to rumble</param>
    public void RumbleConstant(PlayerIndex playerIndex, float duration) {
        if (CheckCallForValidity()) {
            rumbleCoroutine = StartCoroutine(RumbleConstantCoroutine(playerIndex, duration, 1f, 1f));
        }
    }

    /// <summary>
    /// Controller-Rumble at constant velocity.
    /// </summary>
    /// <param name="playerIndex">PlayerIndex of the controller that shall rumble.</param>
    /// <param name="duration">Duration to rumble</param>
    /// <param name="leftMotor">Strength of the left motor (heavy rumble). Range from 0 to 1.</param>
    /// <param name="rightMotor">Strength of the right motor (light rumble). Range from 0 to 1.</param>
    public void RumbleConstant(PlayerIndex playerIndex, float duration, float leftMotor, float rightMotor) {
        if (CheckCallForValidity()) {
            rumbleCoroutine = StartCoroutine(RumbleConstantCoroutine(playerIndex, duration, leftMotor, rightMotor));
        }
    }
    #endregion



    #region Private Functions
    private bool CheckCallForValidity() {
        if (rumbleCoroutine != null) {
            Debug.LogWarning("Couldn't trigger rumble, because another rumble is already in progress.");
            return false;
        }

        return true;
    }
    #endregion



    #region Coroutines
    IEnumerator RumbleConstantCoroutine(PlayerIndex playerIndex, float duration, float leftMotor, float rightMotor) {
        GamePad.SetVibration(playerIndex, leftMotor, rightMotor);
        yield return new WaitForSecondsRealtime(duration);
        GamePad.SetVibration(playerIndex, 0f, 0f);

        rumbleCoroutine = null;
    }
    #endregion
}
