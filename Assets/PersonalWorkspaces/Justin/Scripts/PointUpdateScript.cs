using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PointUpdateScript : MonoBehaviour
{
    public float time;
    public PlayerManager playerManagerScript;
    //public PointCollectorScript pointCollectorScript;
    public WaveStart waveStartScript;

    public float pointsA;
    public float pointsB;

    public float pointIncrement = 1;

    public GameObject pointsTextA;
    public GameObject pointsTextB;

    bool aHalfPoint;
    bool bHalfPoint;

    //keys for teamA and teamB scores
    private static readonly string TeamAScore = "TeamAScore";
    private static readonly string TeamBScore = "TeamBScore";

    //public PointCollectorScript pointCollectorScript;



    // Start is called before the first frame update
    void Start()
    {
        pointsTextA = GameObject.Find("PointsA");
        pointsTextB = GameObject.Find("PointsB");
        
    }

    // Update is called once per frame
    void Update()
    {
        pointsA = 0;
        pointsB = 0;
        time += Time.deltaTime;
        

        if(time > 1f)
        {
            if(playerManagerScript.buttonsPressed > 0 && waveStartScript.gameTimerStart)
            {
                if (playerManagerScript.team == "A") 
                {
                    pointsA += pointIncrement * playerManagerScript.buttonsPressed;

                    pointsTextA.GetComponentInChildren<PointsADisplayScript>().points += (int)pointsA;
                }
                else
                {
                    pointsB += pointIncrement * playerManagerScript.buttonsPressed;
                   
                    pointsTextB.GetComponentInChildren<PointsADisplayScript>().points += (int)pointsB;
                }
            }

            time = 0f;
        }

        
    }
}
