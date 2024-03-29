using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PointCollectorScript : MonoBehaviour, IPunObservable
{
    public GameObject pointsTextA;
    public GameObject pointsTextB;

    public PlayerManager playerManagerScript;
    public PointUpdateScript pointUpdateScript;
    public WaveStart waveStartScript;

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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            stream.SendNext(time);
            stream.SendNext(pointsA);
            stream.SendNext(pointsB);
            stream.SendNext(currentPlayer);
        }
        else
        {
            time = (float)stream.ReceiveNext();
            pointsA = (float)stream.ReceiveNext();
            pointsB = (float)stream.ReceiveNext();
            ColorChange((string)stream.ReceiveNext());
        }
    }

    private void ColorChange(string team)
    {
            if (team == "A")
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

    private void OnTriggerEnter(Collider collision)
    {
        waveStartScript = GameObject.Find("RoundStartObject").GetComponent<WaveStart>();
        if (collision.gameObject.tag == "Player" && collision.gameObject != currentPlayer && waveStartScript.gameTimerStart)
        {
            collision.gameObject.GetComponent<PlayerManager>().recievePoint(gameObject);
        }
    }

    public void runPointCollision(GameObject player)
    {
        
            Debug.Log("Hellooo");

            //this will run when a new player hits a button after it has been pressed more than once
            if (currentPlayer != null)
            {
                //ansell added these two lines for Henry to look at.
                GetComponentInChildren<Renderer>().material.color = Color.grey;
                GetComponentInChildren<PointCollectorScript>().currentPlayer = null;
                Debug.Log("not null");
                currentPlayer.gameObject.GetComponent<PlayerManager>().pointCollectors.Remove(gameObject);
                currentPlayer.gameObject.GetComponent<PlayerManager>().buttonsPressed -= 1;

            }


            currentPlayer = player;

            playerManagerScript = currentPlayer.GetComponentInChildren<PlayerManager>();

            Debug.Log("p1" + currentPlayer);

            //alreadyPressedA = true;


            player.GetComponent<PlayerManager>().buttonsPressed += 1;


            if (playerManagerScript.team == "A")
            {
                currentTeam = "A";
                gameObject.GetComponentInChildren<Renderer>().material.color = Color.blue;
            }
            else
            {
                currentTeam = "B";
                gameObject.GetComponentInChildren<Renderer>().material.color = Color.red;
            }

        
    }
}
