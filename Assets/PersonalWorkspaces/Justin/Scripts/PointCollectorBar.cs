using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointCollectorBar : MonoBehaviour
{
    public int aCollected;
    public int bCollected;

    public GameObject aBar;
    public GameObject bBar;

    public GameObject buttons;

    //button.getchildren.length
    // Start is called before the first frame update
    void Start()
    {
        buttons = GameObject.Find("Goobers");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("A pc: " + aCollected + " B pc: " + bCollected);
    }

    public void updateBar (string team, int b)
    {
        Debug.Log("pcpc: " + b);
        if(team == "A")
        {
            aCollected += b;
            if (aCollected < 0) aCollected = 0;

            aBar.GetComponent<RectTransform>().sizeDelta = new Vector2((aCollected * 2731)/buttons.transform.childCount, 45);
        } else
        {

            bCollected += b;
            if (bCollected < 0) bCollected = 0;

            bBar.GetComponent<RectTransform>().sizeDelta = new Vector2((bCollected * 2731) / buttons.transform.childCount, 45);
            Debug.Log("B pc called");
            
        }
    }
}
