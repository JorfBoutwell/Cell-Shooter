using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//NEEDS UPDATE WHEN ABILITIES ARE IMPLEMENTED
public class CooldownScript : MonoBehaviour
{
    [Header("C Cooldown Overlay")]
    public GameObject cooldownOverlayC;
    public GameObject cooldownOverlayBarC;

    [Header("E Cooldown Overlay")]
    public GameObject cooldownOverlayE;
    public GameObject cooldownOverlayBarE;

    [Header("X Cooldown Overlay")]
    public GameObject cooldownOverlayX;
    public GameObject cooldownOverlayBarX;

    [Header("Q Cooldown Overlay")]
    public GameObject cooldownOverlayQ;
    public GameObject cooldownOverlayBarQ;

    [Header("Ability Actives")]
    public bool cooldownActiveC = false;
    public bool cooldownActiveE = false;
    public bool cooldownActiveX = false;
    public bool cooldownActiveQ = false;

    [Header("Ability Cooldowns")]
    public float heightC = 100;
    public float heightE = 100;
    public float heightX = 100;
    public float heightQ = 100;

    [Header("Cooldown Variables")]
    public GameObject cooldownOverlay;
    public GameObject cooldownOverlayBar;
    public bool cooldownActive;

    public float cooldownTime;

    void Update()
    {
        //Cooldown Activation
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

        //Cooldown Decrease Overlay
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

        //Cooldown Reset Overlay
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

    //Cooldown Overlay Activate
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
