using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    public float TimeUntilTrain = 60f;
    public bool Direction = false;
    private float NewTrain;
    public float Moving = 0f;
    private float Delta;
    private Vector3 StartPos;
    private BoxCollider Collider;

    public bool test;

    KillFeed killFeedScript;

    PlayerManager playerManagerScript;

  // Start is called before the first frame update
    void Start()
    {
        killFeedScript = GameObject.Find("KillFeed").GetComponent<KillFeed>();
        NewTrain = TimeUntilTrain;
        StartPos = transform.position;
        //Collider = transform.gameObject.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        Delta = Time.deltaTime;
        

        if (Moving > 0)
        {
            if (Direction)
            {
                transform.position += new Vector3(2f * Delta * 60f, 0, 0);
            } else
            {
                transform.position -= new Vector3(2f * Delta * 60f, 0, 0);
            }
            Moving -= Delta;
        } else
        {
            transform.position = StartPos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("TRAIN HIT PLAYER" + other);
        playerManagerScript = other.GetComponent<PlayerManager>();
        playerManagerScript.health = 0;
        playerManagerScript.isDead = true;

        if (!killFeedScript.hitByTrain) {
            killFeedScript.hitByTrain = true;
            killFeedScript.player2 = playerManagerScript.username;
            killFeedScript.player1 = "TRAIN";
            killFeedScript.KillFeedInstantiate(killFeedScript.boxesCount);
        }
    }
}
