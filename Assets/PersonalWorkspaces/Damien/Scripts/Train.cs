using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    public float TimeUntilTrain = 60f;
    public bool Direction = false;
    private float NewTrain;
    private float Moving = 0f;
    private float Delta;
    private Vector3 StartPos;
    private BoxCollider Collider;

<<<<<<< HEAD
    PlayerManager playerManagerScript;

=======
>>>>>>> parent of 90ef0b0 (Revert "Merge branch 'main' of https://github.com/JorfBoutwell/Cell-Shooter")
    // Start is called before the first frame update
    void Start()
    {
        NewTrain = TimeUntilTrain;
        StartPos = transform.position;
        //Collider = transform.gameObject.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        Delta = Time.deltaTime;
        NewTrain -= Delta;

        if (NewTrain <= 0)
        {
            NewTrain = TimeUntilTrain + Random.Range(-10f, 10f);
            Moving = 3.2f;
        }

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
<<<<<<< HEAD
        Debug.Log("TRAIN HIT PLAYER" + other);
        playerManagerScript = other.GetComponent<PlayerManager>();
        playerManagerScript.health -= 100;
        playerManagerScript.isDead = true;
=======
        //Debug.Log(other);
>>>>>>> parent of 90ef0b0 (Revert "Merge branch 'main' of https://github.com/JorfBoutwell/Cell-Shooter")
    }
}
