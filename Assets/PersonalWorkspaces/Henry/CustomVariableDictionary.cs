using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CustomVariableDictionary : MonoBehaviourPunCallbacks
{
    public Dictionary<int, string> team = new Dictionary<int, string>();

    //key and local players team variable
    private static readonly string TeamPropKey = "TeamA?";
    private bool teamA = false;

    private void Awake()
    {
        if(team.Count > 0)
        {
            if (photonView.IsMine && team[PhotonNetwork.LocalPlayer.ActorNumber] == "aTeam")
            {
                SetTeam(true);
            } else if (photonView.IsMine)
            {
                SetTeam(false);
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
}
