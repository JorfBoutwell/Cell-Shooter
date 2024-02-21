using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinOverlay : MonoBehaviour
{
    PointUpdateScript pointUpdateScript;
    public GameObject winOverlay;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI returnTimer;
    public float returnTime = 5f;
    string winTeam;

    // Start is called before the first frame update
    void Start()
    {
        //winOverlay.SetActive(false);
        pointUpdateScript = GameObject.Find("PointObject").GetComponentInChildren<PointUpdateScript>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("HELO");
        if (pointUpdateScript.pointsA >= 10)
        {
            winOverlay.SetActive(true);
        }
        else if(pointUpdateScript.pointsB >= 10)
        {
            Debug.Log("HII");

            winOverlay.SetActive(true);
        }

        Debug.Log("bruh" + pointUpdateScript.pointsB);
    }
}
