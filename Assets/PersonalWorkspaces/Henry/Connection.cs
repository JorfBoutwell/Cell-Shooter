using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

/// <summary>
///
/// This script is the what connects the player to the server.
/// Lobby is what will be the menu
/// This script is used in loading screen, if you make a logo you could put it there like you see in other scenes when it says connecting
/// 
/// </summary>
public class Connection : MonoBehaviourPunCallbacks
{
    
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}
