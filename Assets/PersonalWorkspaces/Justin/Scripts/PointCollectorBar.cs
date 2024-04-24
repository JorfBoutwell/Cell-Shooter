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

    public void updateBar (string team)
    {
        if(team == "A")
        {
            aBar.GetComponent<RectTransform>().sizeDelta = new Vector2((aCollected * 100)/buttons.transform.childCount, 45);
        } else
        {
            bBar.GetComponent<RectTransform>().sizeDelta = new Vector2((bCollected * 100) / buttons.transform.childCount, 45);

        }
    }
}
