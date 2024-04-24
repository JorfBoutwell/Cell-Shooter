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
            if(b > 0)
            {
                aCollected += b;
            } else if (b < 0)
            {
                aCollected -= b;
            }
            aBar.GetComponent<RectTransform>().sizeDelta = new Vector2((aCollected * 2731)/buttons.transform.childCount, 45);
        } else
        {
            if (b > 0)
            {
                bCollected += b;
            }
            else if (b < 0)
            {
                bCollected -= b;
            }
            bBar.GetComponent<RectTransform>().sizeDelta = new Vector2((bCollected * 2731) / buttons.transform.childCount, 45);
            Debug.Log("B pc called");
            
        }
    }
}
