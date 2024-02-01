using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointUpdateScript : MonoBehaviour
{
    public float time;
    public PlayerManager playerManagerScript;

    public float pointsA;
    public float pointsB;

    public float pointIncrement = 1;

    public GameObject pointsTextA;
    public GameObject pointsTextB;
    //public PointCollectorScript pointCollectorScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if(time > 1f)
        {
            if(playerManagerScript.buttonsPressed > 0)
            {
                if (playerManagerScript.team == "A")
                {
                    pointsA += pointIncrement * playerManagerScript.buttonsPressed;
                    pointsTextA.GetComponentInChildren<TextMeshProUGUI>().text = Mathf.FloorToInt(pointsA).ToString("0");
                }
                else
                {
                    pointsB += pointIncrement * playerManagerScript.buttonsPressed;
                    pointsTextB.GetComponentInChildren<TextMeshProUGUI>().text = Mathf.FloorToInt(pointsB).ToString("0");
                }
            }

            time = 0f;
        }
    }

    
}
