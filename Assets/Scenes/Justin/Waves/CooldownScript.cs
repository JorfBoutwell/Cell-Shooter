using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CooldownScript : MonoBehaviour
{
    //Cooldown variables
    public GameObject cooldownOverlayC;
    public GameObject cooldownOverlayBarC;

    public GameObject cooldownOverlayE;
    public GameObject cooldownOverlayBarE;

    public GameObject cooldownOverlayX;
    public GameObject cooldownOverlayBarX;

    public GameObject cooldownOverlayQ;
    public GameObject cooldownOverlayBarQ;

    public bool cooldownActiveC = false;
    public bool cooldownActiveE = false;
    public bool cooldownActiveX = false;
    public bool cooldownActiveQ = false;

    float heightC = 100;
    float heightE = 100;
    float heightX = 100;
    float heightQ = 100;

    public GameObject cooldownOverlay;
    public GameObject cooldownOverlayBar;
    public bool cooldownActive;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Key down
        if (Input.GetKeyDown(KeyCode.C) && cooldownActiveC == false)
            {
                cooldownOverlay = cooldownOverlayC;
                CooldownActive(cooldownOverlay);
                cooldownActiveC = true;
            }
        else if(Input.GetKeyDown(KeyCode.E) && cooldownActiveE == false)
            {
                cooldownOverlay = cooldownOverlayE;
                CooldownActive(cooldownOverlay);
                cooldownActiveE = true;
            }

        //Cooldown decrease
        if(heightC > 0 && cooldownActiveC)
            {
                cooldownOverlayBarC.GetComponent<RectTransform>().sizeDelta = new Vector2(100, heightC);
                heightC -= 10 * Time.deltaTime;
            }
        if (heightE > 0 && cooldownActiveE)
            {
                cooldownOverlayBarE.GetComponent<RectTransform>().sizeDelta = new Vector2(100, heightE);
                heightE -= 10 * Time.deltaTime;
            }

        //Cooldown reset
        if (cooldownActiveC && heightC <= 0)
            {
                cooldownActiveC = false;
                heightC = 100;

                cooldownOverlayC.SetActive(false);
                cooldownOverlayBarC.SetActive(false);
            }
        else if (cooldownActiveE && heightE <= 0)
            {
                cooldownActiveE = false;
                heightE = 100;

                cooldownOverlayE.SetActive(false);
                cooldownOverlayBarE.SetActive(false);
            }
    }

   private void CooldownActive(GameObject cooldownOverlay)
    {
        if(cooldownOverlay == cooldownOverlayC)
        {
            cooldownOverlayC.SetActive(true);
            cooldownOverlayBarC.SetActive(true);
        }
        else if (cooldownOverlay == cooldownOverlayE)
        {
            cooldownOverlayE.SetActive(true);
            cooldownOverlayBarE.SetActive(true);
        }
    }

}
