using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointCollectorScript : MonoBehaviour
{
    public GameObject pointsTextA;
    public GameObject pointsTextB;

    public PlayerManager playerManagerScript;
    public PointUpdateScript pointUpdateScript;

    public float time;
    public float pointsA;
    public float pointsB;

    private bool alreadyPressed = false;

    private string currentTeam;



    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponentInChildren<Renderer>().material.color = Color.grey;
    }

    // Update is called once per frame
    void Update()
    {
        //UpdatePoints(playerManagerScript.updatePoints);

        
    }

    void UpdatePoints(string updatePoints)
    {
        if (updatePoints == "A") {
            pointsA += 1 * Time.deltaTime;
            pointsTextA.GetComponentInChildren<TextMeshProUGUI>().text = Mathf.FloorToInt(pointsA).ToString("0");

            Debug.Log("Why" + playerManagerScript.currentPointCollectorsA); //updates by 2?

            playerManagerScript.pointCollectors[playerManagerScript.currentPointCollectorsA].GetComponentInChildren<Renderer>().material.color = Color.red;
            playerManagerScript.currentPointCollectorsA += 1; //causes error?
        }
        else if(updatePoints == "B")
        {
            pointsB += 1 * Time.deltaTime;
            pointsTextB.GetComponentInChildren<TextMeshProUGUI>().text = Mathf.FloorToInt(pointsB).ToString("0");
            gameObject.GetComponentInChildren<Renderer>().material.color = Color.blue;
        }
        //use array in player manager to prevent the one script affecting both of the point collectors

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && !alreadyPressed)
        {
            Debug.Log("Hellooo");

            alreadyPressed = true;

            playerManagerScript.buttonsPressed += 1;

            if (playerManagerScript.team == "A")
            {
                
                gameObject.GetComponentInChildren<Renderer>().material.color = Color.red;
            }
            else
            {
                gameObject.GetComponentInChildren<Renderer>().material.color = Color.blue;
            }


        }
    }
}
