using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    public Image[] warnings;
    bool startWarning = true;
    public float warningDuration = 8f;

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

    public void FlashWarning(float duration)
    {
            foreach (Image warning in warnings)
            {
                Debug.Log("starttween");
                warning.DOFade(1f, duration / 10).SetLoops(10, LoopType.Yoyo).SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    warning.color = new Color(warning.color.r, warning.color.g, warning.color.b, 0f);
                });
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("TRAIN HIT PLAYER" + other);
        playerManagerScript = other.GetComponent<PlayerManager>();
        playerManagerScript.health = 0;
        playerManagerScript.isDead = true;

        if (!killFeedScript.hitByTrain) {
            if (playerManagerScript.team == "A")
            {
                killFeedScript.player2Image.color = Color.blue;
            }
            else
            {
                killFeedScript.player2Image.color = Color.red;
            }

            killFeedScript.player2Icon.sprite = playerManagerScript.charIcon.sprite;

            killFeedScript.player1Image.color = Color.black;
            killFeedScript.player1Icon.sprite = playerManagerScript.charIcon.sprite;

            killFeedScript.hitByTrain = true;
            killFeedScript.player2 = playerManagerScript.username;
            killFeedScript.player1 = "TRAIN";
            killFeedScript.KillFeedInstantiate(killFeedScript.boxesCount);
        }
    }
}
