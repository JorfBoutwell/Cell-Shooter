using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthUI : MonoBehaviour
{
    public GameObject playerManager;
    private PlayerManager playerManagerScript;

    //Healthbar variables
    public GameObject healthBar;
    public GameObject healthBarShadow;
    public float healthBarUI = 680f;
    public float healthShadow;

    float damageTaken = 50f; //damageTaken value for testing
    float damageTakenUI;

    // Start is called before the first frame update
    void Start()
    {
        playerManagerScript = playerManager.GetComponent<PlayerManager>();
        healthShadow = healthBarUI;
        damageTakenUI = damageTaken * 6.8f; //6.8 to scale with the UI
    }

    // Update is called once per frame
    void Update()
    {
        //Health UI Damage
        if (Input.GetKeyDown(KeyCode.L)) //For Testing
        {
            healthBarUI -= damageTakenUI;
            playerManagerScript.health -= damageTaken;
            healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(healthBarUI, 90);
        }

        if (healthShadow > healthBarUI)
        {
            healthBarShadow.GetComponent<RectTransform>().sizeDelta = new Vector2(healthShadow, 90);
            healthShadow -= 100 * Time.deltaTime;
        }

        if (healthBarUI <= 0)
        {
            playerManagerScript.isDead = true;
        }
    }
}
