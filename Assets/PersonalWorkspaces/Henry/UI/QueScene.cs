using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class QueScene : MonoBehaviourPunCallbacks
{
    private static readonly string TeamPropKey = "TeamBlue?";
    private bool teamBlue = false;

    private void Start()
    {
        if (photonView.IsMine)
        {
            List<Player> teamBlue = GetTeamBlue();
            if (teamBlue.Count < PhotonNetwork.PlayerList.Length / 2)
            {
                SetTeam(true);
                
            } else
            {
                SetTeam(false);
            }
        }
    }

    public void SetTeam(bool value)
    {
        if (photonView.IsMine)
        {
            teamBlue = value;
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { TeamPropKey, value } });
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (targetPlayer != null && targetPlayer == PhotonNetwork.LocalPlayer)
        {
            if (changedProps.ContainsKey(TeamPropKey))
            {
                teamBlue = (bool)changedProps[TeamPropKey];
                if (teamBlue)
                {
                    Debug.Log("Player is team Blue:" + targetPlayer.NickName);
                } else
                {
                    Debug.Log("Player is team Red:" + targetPlayer.NickName);
                }
                
            }
        }
    }

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
}
