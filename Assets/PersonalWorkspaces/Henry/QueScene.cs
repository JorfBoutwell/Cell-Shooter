using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class QueScene : MonoBehaviourPunCallbacks
{
    //key and local players team variable
    private static readonly string TeamPropKey = "TeamBlue?";
    private bool teamBlue = false;

    //key and local players ready status
    private static readonly string ReadyPropKey = "ReadyUp";
    private bool ready = false;

    public GameObject start;

    int recentJoin = 0;

    private void Start()
    {
        if (photonView.IsMine && recentJoin > 0)
        {
            recentJoin -= 1;
            //get list of all team blue players
            List<Player> teamBlue = GetTeamBlue();
            if (teamBlue.Count < PhotonNetwork.PlayerList.Length / 2)
            {
                //if less than or equal # of players are blue to red
                SetTeam(true);
                
            } else
            {
                //otherwise set this player on red team
                SetTeam(false);
            }
        }
        updateReadyState(false);
    }

    //sets team based on what is passed
    public void SetTeam(bool value)
    {
        if (photonView.IsMine)
        {
            //stores passed variable
            teamBlue = value;
            //makes a custom variable in photon for team, will always be with player while in the game
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { TeamPropKey, value } });
        }
    }

    //sets ready status based on what is passed
    public void updateReadyState(bool value)
    {
        if (photonView.IsMine)
        {
            //stores passed value
            ready = value;
            //makes custom variable in photon for ready status, will always be with player in the game
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { ReadyPropKey, value } });
        }
    }
    private void OnPlayerConnected()
    {
        recentJoin = 60;
    }

    //runs every time a property is updated
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        //if the change being made is for the local user
        if (targetPlayer != null && targetPlayer == PhotonNetwork.LocalPlayer)
        {
            //check if it is a team update
            if (changedProps.ContainsKey(TeamPropKey))
            {
                //check if the player is blue or red and report to all
                teamBlue = (bool)changedProps[TeamPropKey];
                if (teamBlue)
                {
                    Debug.Log("Player is team Blue:" + targetPlayer.NickName);
                } else
                {
                    Debug.Log("Player is team Red:" + targetPlayer.NickName);
                }
                
            }
            //check if update is for ready status
            if (changedProps.ContainsKey(ReadyPropKey))
            {
                //if ready or not will update others
                ready = (bool)changedProps[ReadyPropKey];
                if (ready)
                {
                    Debug.Log(targetPlayer.NickName + " is ready");
                } else
                {
                    Debug.Log(targetPlayer.NickName + " is not ready");
                }

                int readyCount = 0;
                foreach (Player player in PhotonNetwork.PlayerList)
                {
                    object readyUp;
                    if (player.CustomProperties.TryGetValue(ReadyPropKey, out readyUp)) {
                        if ((bool)readyUp)
                        {
                            readyCount++;
                        }
                    }

                }
                if (PhotonNetwork.IsMasterClient && readyCount == PhotonNetwork.PlayerList.Length)
                {
                    Debug.Log(readyCount + "      " + PhotonNetwork.PlayerList.Length);
                    start.SetActive(true);
                } else
                {
                    start.SetActive(false);
                }
            }
        }
    }
    //gets list of all players on the blue team
    public List<Player> GetTeamBlue()
    {
        List<Player> bluePlayers = new List<Player>();

        foreach(Player player in PhotonNetwork.PlayerList)
        {
            object blueTeam;
            if (player.CustomProperties.TryGetValue(TeamPropKey, out blueTeam))
            {
                if ((bool)blueTeam)
                {
                    bluePlayers.Add(player);
                }
            }
        }
        return bluePlayers;
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Multiplayer World");
        }
    }
}
