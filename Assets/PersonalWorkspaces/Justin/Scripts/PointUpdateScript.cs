using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PointUpdateScript : MonoBehaviour
{
    public float time;
    public PlayerManager playerManagerScript;
    //public PointCollectorScript pointCollectorScript;

    public float pointsA;
    public float pointsB;
    public int change = 0;

    public float pointIncrement = 1;

    public GameObject pointsTextA;
    public GameObject pointsTextB;
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
        time += Time.deltaTime;
        if (PhotonNetwork.LocalPlayer.NickName == "henry")
        {
            change = 5;
        }
        

        if(time > 1f)
        {
            if(playerManagerScript.buttonsPressed > 0)
            {
                if (playerManagerScript.team == "A") //use currentteam to prevent both teams getting points from the same buttons?
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

    //sends and recieves data to everyone elses versions of this script and will synch data
    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //make sure oreder of send is same as recieve variable wise
        if (stream.IsWriting)
        {
            stream.SendNext(change);
            //stream.SendNext(variableA);
            //stream.SendNext(variableB);
        }
        else if (stream.IsReading)
        {
            SetChange((int)stream.ReceiveNext());
            //variableA = (type like bool)stream.ReceiveNext();
            //SetVarB((type)stream.ReceiveNext());
            //would recommend B version where you call a function like example below but A works too
        }
    }

    /*private void SetVarB(type varB) 
     * {
     *      if (varB == variableB)
     *          return;
     *          
     *      variableB = varB
     * }*/

    private void SetChange(int newVar)
    {
        if (newVar == change)
            return;

        change = newVar;
    }
}
