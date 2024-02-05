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

    private string currentTeam;
    public GameObject currentPlayer;



    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponentInChildren<Renderer>().material.color = Color.grey;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Player" && !currentPlayer)
        {
            Debug.Log("Hellooo");

            currentPlayer = collision.gameObject;

            playerManagerScript = currentPlayer.GetComponentInChildren<PlayerManager>();

            Debug.Log("p1" + currentPlayer);

            //alreadyPressedA = true;

            playerManagerScript.buttonsPressed += 1;

            if (playerManagerScript.team == "A")
            {
                currentTeam = "A";
                gameObject.GetComponentInChildren<Renderer>().material.color = Color.red;
            }
            else
            {
                currentTeam = "B";
                gameObject.GetComponentInChildren<Renderer>().material.color = Color.blue;
            }


        }
    }
}
