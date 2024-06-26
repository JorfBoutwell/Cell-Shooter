using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthUI : MonoBehaviour
{
    public PlayerManager playerManagerScript;

    //Healthbar variables
    public GameObject healthBar;
    public GameObject healthBarShadow;
    public float healthBarUI = 680f;
    public float maxWidth = 680f;
    public float healthShadow;
    public float lastHealth = 100;

    RectTransform healthRect;

    float damageTaken = 50f; //damageTaken value for testing
    float damageTakenUI;

    // Start is called before the first frame update
    void Start()
    {
        healthRect = healthBar.GetComponent<RectTransform>();
        healthShadow = healthBarUI;
        damageTakenUI = damageTaken * 6.8f; //6.8 to scale with the UI
    }

    // Update is called once per frame
    void Update()
    {
        if(healthRect.sizeDelta.x > maxWidth)
        {
            healthRect.sizeDelta = new Vector2(maxWidth, 90f);
        }
        //Health UI Damage
        if (Input.GetKeyDown(KeyCode.L)) //For Testing
        { 
                playerManagerScript.isDead = true;
            
        }

        if(lastHealth != playerManagerScript.health)
        {
            healthBarUI = playerManagerScript.health * 6.8f;

            damageTaken = lastHealth - playerManagerScript.health;
            damageTakenUI = damageTaken * 6.8f;
            healthBarUI -= damageTakenUI;
            healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(healthBarUI, 90);

            lastHealth = playerManagerScript.health;
        }

        if (healthShadow > healthBarUI)
        {
            healthBarShadow.GetComponent<RectTransform>().sizeDelta = new Vector2(healthShadow, 90);
            healthShadow -= 100 * Time.deltaTime;
        }
    }
}
