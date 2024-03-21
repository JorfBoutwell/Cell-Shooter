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


        //gets the UI element
        code = GameObject.Find("GameCode").GetComponent<TMP_Text>();
        code.SetText("Code: " + PhotonNetwork.CurrentRoom.Name);

        //set character specific variables
        if(player.GetComponent<PlayerManager>() != null)
        {
            //character = PASS PLAYERS CHOSEN CHARACTER
            PlayerRef playerRef = null; //set playerRefs based on character

            /* I'll add the logic to set everything
             * 
             * player.animator = playerRef.animator
             * player.abilities = playerRef.abilities
             * etc.
             * 
             */
        }

    }

}
