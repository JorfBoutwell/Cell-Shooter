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

    public float cooldownTime;

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
        else if (Input.GetKeyDown(KeyCode.X) && cooldownActiveX == false)
        {
            cooldownOverlay = cooldownOverlayX;
            CooldownActive(cooldownOverlay);
            cooldownActiveX = true;
        }
        else if (Input.GetKeyDown(KeyCode.Q) && cooldownActiveQ == false)
        {
            cooldownOverlay = cooldownOverlayQ;
            CooldownActive(cooldownOverlay);
            cooldownActiveQ = true;
        }

        //Cooldown decrease
        if (heightC > 0 && cooldownActiveC)
            {
                cooldownOverlayBarC.GetComponent<RectTransform>().sizeDelta = new Vector2(100, heightC);
                cooldownTime = 5f; //Change cooldown time
                heightC -= cooldownTime * Time.deltaTime;
            }
        if (heightE > 0 && cooldownActiveE)
            {
                cooldownOverlayBarE.GetComponent<RectTransform>().sizeDelta = new Vector2(100, heightE);
                cooldownTime = 20f; //Change cooldown time
                heightE -= cooldownTime * Time.deltaTime;
            }
        if (heightX > 0 && cooldownActiveX)
        {
            cooldownOverlayBarX.GetComponent<RectTransform>().sizeDelta = new Vector2(100, heightX);
            cooldownTime = 10f; //Change cooldown time
            heightX -= cooldownTime * Time.deltaTime;
        }
        if (heightQ > 0 && cooldownActiveQ)
        {
            cooldownOverlayBarQ.GetComponent<RectTransform>().sizeDelta = new Vector2(100, heightQ);
            cooldownTime = 15f; //Change cooldown time
            heightQ -= cooldownTime * Time.deltaTime;
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
        else if (cooldownActiveX && heightX <= 0)
        {
            cooldownActiveX = false;
            heightX = 100;

            cooldownOverlayX.SetActive(false);
            cooldownOverlayBarX.SetActive(false);
        }
        else if (cooldownActiveQ && heightQ <= 0)
        {
            cooldownActiveQ = false;
            heightQ = 100;

            cooldownOverlayQ.SetActive(false);
            cooldownOverlayBarQ.SetActive(false);
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
        else if (cooldownOverlay == cooldownOverlayX)
        {
            cooldownOverlayX.SetActive(true);
            cooldownOverlayBarX.SetActive(true);
        }
        else if (cooldownOverlay == cooldownOverlayQ)
        {
            cooldownOverlayQ.SetActive(true);
            cooldownOverlayBarQ.SetActive(true);
        }
    }

}
