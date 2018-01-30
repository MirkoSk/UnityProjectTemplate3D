using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Healthbar : SubscribedBehaviour {

    #region Variable Declarations
    [SerializeField] Gradient hpColor;
    [SerializeField] float punchAmount = 0.1f;
    [SerializeField] float punchDuration = 0.3f;

    Transform healthbarForeground;
	#endregion
	
	
	
	#region Unity Event Functions
	private void Start () {
        healthbarForeground = transform.Find("HealthbarForeground");
	}
    #endregion



    #region Public Functions
    void TakeDamage(int hp, int maxHP) {
        // Set new color of the healthbar
        healthbarForeground.GetComponent<UnityEngine.UI.Image>().color = hpColor.Evaluate((float)hp / maxHP);

        // Rescale the green part of the healthbar
        Vector3 newScale = new Vector3((float)hp / maxHP, healthbarForeground.localScale.y, healthbarForeground.localScale.z);
        healthbarForeground.transform.localScale = newScale;

        // Add Juiciness
        LeanTween.scale(healthbarForeground.gameObject, healthbarForeground.localScale + Vector3.one * punchAmount, punchDuration).setEase(LeanTweenType.punch);
    }
	#endregion
}
