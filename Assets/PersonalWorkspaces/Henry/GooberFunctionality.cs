using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GooberFunctionality : MonoBehaviour
{
    public WaveStart waveStartScript;
    public KillFeed killFeedScript;
    public GameObject currentPlayer;
    public float dropped = 0;
    public string team = null;
    public bool atpClaimed = false;

    private void Start()
    {
        killFeedScript = GameObject.Find("KillFeed").GetComponent<KillFeed>();
    }

    private void Update()
    {
        if(currentPlayer != null)
        {
            Debug.Log(currentPlayer);
            transform.position = currentPlayer.transform.position + new Vector3(0, 1.5f, 0);
        }
        if (dropped > 0)
        {
            dropped -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("collided");
        waveStartScript = GameObject.Find("RoundStartObject").GetComponent<WaveStart>();
        if (collision.gameObject.tag == "Player" && currentPlayer == null && waveStartScript.gameTimerStart && dropped <= 0)
        {
            collision.gameObject.GetComponent<PlayerManager>().CapturingTheFlag();
            GetComponent<SphereCollider>().enabled = false;

            atpClaimed = true;

            //killFeedScript.AlertFeedInstantiate(killFeedScript.boxesCount, (collision.gameObject.GetComponent<PlayerManager>().username + " claimed the ATP!"));

            
        }
    }

    public void RunCollision(GameObject player)
    {
        currentPlayer = player;
        
        PlayerManager playerManagerScript = currentPlayer.GetComponent<PlayerManager>();
        playerManagerScript.buttonsPressed += 1;

        killFeedScript.AlertFeedInstantiate(killFeedScript.boxesCount, (playerManagerScript.username + " claimed the ATP!"));

        //Turnoff goober guidance
        GameObject gooberGuide = player.GetComponentInChildren<GooberGuidenceSystem>().transform.gameObject;
        gooberGuide.SetActive(false);

        //HENRY: SET EACH OTHER PLAYERS GOOBER GUIDE TO THE COLOR OF THE TEAM THAT PICKED IT UPs

        transform.position = currentPlayer.transform.position + new Vector3(0, 1.5f, 0);
        transform.SetParent(currentPlayer.transform);

        team = currentPlayer.GetComponent<PlayerManager>().team;

        foreach (GameObject a in currentPlayer.GetComponent<WeaponManager>().abilityUI.abilityObjects)
        {
            a.transform.GetChild(3).gameObject.GetComponent<Image>().enabled = true;
            a.transform.GetChild(0).GetChild(1).GetComponent<Image>().enabled = false;
        }
    }
}
