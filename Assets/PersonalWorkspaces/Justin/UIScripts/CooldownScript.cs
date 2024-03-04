using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//NEEDS UPDATE WHEN ABILITIES ARE IMPLEMENTED
public class CooldownScript : MonoBehaviour
{

    public GameObject cooldownOverlay;
    public GameObject cooldownOverlayBar;

    public IEnumerator CooldownOverlay(float cooldownTime)
    {
        float timer = 0;
        float size = 100;
        cooldownOverlay = this.gameObject;
        cooldownOverlayBar = this.transform.GetChild(0).transform.gameObject;
        RectTransform barTransform = cooldownOverlayBar.GetComponent<RectTransform>();

        cooldownOverlay.SetActive(true);
        cooldownOverlayBar.SetActive(true);

        while(timer < cooldownTime)
        {
            barTransform.sizeDelta = new Vector2(100, Mathf.Lerp(100, 0, timer/cooldownTime));
            size -= cooldownTime * Time.deltaTime;
            yield return null;
            timer += Time.deltaTime;
        }

        cooldownOverlay.SetActive(false);
        cooldownOverlayBar.SetActive(false);
        barTransform.sizeDelta = new Vector2(100, 100);
    }
}
