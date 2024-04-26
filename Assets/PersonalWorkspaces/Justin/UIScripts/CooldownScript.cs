using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//NEEDS UPDATE WHEN ABILITIES ARE IMPLEMENTED
public class CooldownScript : MonoBehaviour
{

    public GameObject cooldownOverlay;


    public IEnumerator CooldownOverlay(float cooldownTime)
    {
        float timer = 0;
        float size = 100;
        cooldownOverlay = this.gameObject;

        RectTransform barTransform = cooldownOverlay.GetComponent<RectTransform>();

        cooldownOverlay.SetActive(true);


        while(timer < cooldownTime)
        {
            barTransform.sizeDelta = new Vector2(barTransform.sizeDelta.x, Mathf.Lerp(100, 0, timer/cooldownTime));
            size -= cooldownTime * Time.deltaTime;
            yield return null;
            timer += Time.deltaTime;
        }

        cooldownOverlay.SetActive(false);
    
        barTransform.sizeDelta = new Vector2(barTransform.sizeDelta.x, 100);
    }
}
