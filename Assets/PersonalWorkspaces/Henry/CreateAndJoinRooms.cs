using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

/// <summary>
///
/// This scene creates the rooms that people play in. The inputs are located in the lobby scene. These are the Keys to get into games.
/// 
/// </summary>
public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    //input variables for room codes
    public string createInput;
    public InputField joinInput;
    public InputField Nickname;

    //runs when you create a room, makes a room in server
    public void CreateRoom()
    {
        createInput = Random.Range(100, 999) + "-" + Random.Range(100, 999);
        PhotonNetwork.CreateRoom(createInput);
    }

    //joins a room already made
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    //opens the scene that the room is in
    public override void OnJoinedRoom()
    {
        if (Nickname.text != "")
        {
            PhotonNetwork.NickName = Nickname.text;
        }
        else
        {
            PhotonNetwork.NickName = "IM a Loser LOL";
        }
        PhotonNetwork.LoadLevel("Que");
    }
}
