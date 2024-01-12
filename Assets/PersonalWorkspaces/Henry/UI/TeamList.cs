using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TeamList : MonoBehaviourPunCallbacks
{
    public List<string> TeamA = new List<string>();
    public List<string> TeamB = new List<string>();
    public Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;

    private void Start()
    {
        UpdateList();
    }
    private void Update()
    {
        if (players != PhotonNetwork.PlayerList)
        {
            UpdateList();
        }
    }

    public void UpdateList()
    {
        TeamA.Clear();
        TeamB.Clear();
        players = PhotonNetwork.PlayerList;

        foreach (var player in players)
        {
            if (player.ActorNumber % 2 == 0)
            {
                TeamA.Add(player.NickName);
            }
            else
            {
                TeamB.Add(player.NickName);
            }
        }
    }
}
