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
        
    }

    public void updateBar (string team, int buttonsCount)
    {
        if(buttonsCount > 0)
        {
            if (team == "A")
            {
                aCollected += buttonsCount;
            }
            else
            {
                bCollected += buttonsCount;
            }
        } else if(buttonsCount < 0)
        {
            if (team == "A")
            {
                aCollected -= buttonsCount;
            }
            else
            {
                bCollected -= buttonsCount;
            }
        }
        if(!GameObject.Find("Goober"))
        {
            if (team == "A")
            {
                aBar.GetComponent<RectTransform>().sizeDelta = new Vector2((aCollected / buttons.transform.childCount * 2731)/100, 45);
            }
            else
            {
                bBar.GetComponent<RectTransform>().sizeDelta = new Vector2((bCollected / buttons.transform.childCount * 2731)/100, 45);

            }
        } else
        {
            if (team == "A" && aCollected <= 100)
            {
                aBar.GetComponent<RectTransform>().sizeDelta = new Vector2((aCollected * 1365) / 100, 45);
            }
            else if (team == "B" && bCollected <= 100)
            {
                bBar.GetComponent<RectTransform>().sizeDelta = new Vector2((bCollected * 1365)/100, 45);

            }
        }
    }
}
