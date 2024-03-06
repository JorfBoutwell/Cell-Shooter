using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

/// <summary>
///
/// This script spawns the players into the world. It takes a player prefab (that has photon view and photon transform view classic)
/// It also changes part of the UI to show the game code to make it easier for others to join
/// 
/// </summary>
public class SpawnPlayers : MonoBehaviour
{
    //variabels needed
    public GameObject playerPrefab;
    public GameObject player;
    public TMP_Text code;

    public Spawn spawnScript;

    


    private void Start()
    {
        //spawns in player at -15, 2, -20 (this can be changed to a spawn point by changing the vector 3 to something else
        //player = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(-240, 15, -370), Quaternion.identity);
        player = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 0, 0), Quaternion.identity);

        /*if (player.GetComponent<PlayerManager>())
        {
            Debug.Log("found");
            if (player.GetComponent<PlayerManager>().team == "A")
            {
                Debug.Log("spawnteama");
                player.transform.position = spawnScript.spawnPointsA[spawnIncrementA].GetComponent<Vector3>();
                spawnIncrementA++;
            }
            else if (player.GetComponent<PlayerManager>().team == "B")
            {
                Debug.Log("SPAWNTEAMB");
                player.transform.position = spawnScript.spawnPointsB[spawnIncrementB].GetComponent<Vector3>();
                spawnIncrementB++;
            }
        }*/
        //player = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(-15, 2, -20), Quaternion.identity);



        //gets the UI element
        code = GameObject.Find("GameCode").GetComponent<TMP_Text>();
        //code = PhotonNetwork.CurrentRoom.Name;
        //changes the UI element to the rooms code
        code.SetText("Code: " + PhotonNetwork.CurrentRoom.Name);
    }

}
