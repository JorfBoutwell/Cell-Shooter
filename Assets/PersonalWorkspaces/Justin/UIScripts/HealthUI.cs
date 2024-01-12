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
    public float healthShadow;
    public float lastHealth = 100;

    float damageTaken = 50f; //damageTaken value for testing
    float damageTakenUI;

    // Start is called before the first frame update
    void Start()
    {
        healthShadow = healthBarUI;
        damageTakenUI = damageTaken * 6.8f; //6.8 to scale with the UI
    }

    // Update is called once per frame
    void Update()
    {
        //Health UI Damage
        /*if (Input.GetKeyDown(KeyCode.L)) //For Testing
        {
            
            playerManagerScript.health -= damageTaken;
            Debug.Log("L");

            if(playerManagerScript.health <= 0)
            {
                playerManagerScript.isDead = true;
            }
        }*/

        if(lastHealth != playerManagerScript.health)
        {
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
