using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CustomVariableDictionary : MonoBehaviourPunCallbacks
{
    public Dictionary<int, string> team = new Dictionary<int, string>();

    public Dictionary<int, string> character = new Dictionary<int, string>();

    //key and local players team variable
    private static readonly string TeamPropKey = "TeamA?";
    private bool teamA = false;

    //key and local players ready variable
    private static readonly string ReadyPropKey = "ReadyUp";
    private bool loadedIn = false;

    //character key
    private static readonly string IndividualCharacter = "individualCharacter";

    private void Awake()
    {
        if(team.Count > 0)
        {
            if (photonView.IsMine && team[PhotonNetwork.LocalPlayer.ActorNumber] == "aTeam")
            {
                SetTeam(true);
                LoadIn(true);
            } else if (photonView.IsMine)
            {
                SetTeam(false);
                LoadIn(true);
            }
        }
        
    }

    //sets team based on what is passeds
    public void SetTeam(bool value)
    {
        if (photonView.IsMine)
        {
            //stores passed variable
            teamA = value;
            //makes a custom variable in photon for team, will always be with player while in the game
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { TeamPropKey, value } });
        }
    }

    public void setCharacter(string value)
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { IndividualCharacter, character[PhotonNetwork.LocalPlayer.ActorNumber] } });
        }
    }

    //sets ready status to true
    public void LoadIn(bool value)
    {
        if(photonView.IsMine)
        {
            //stores passed variable
            loadedIn = value;
            //makes a custom variable in photon for ready status, will always be with player while in game
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { ReadyPropKey, value } });
        }
    }
}
