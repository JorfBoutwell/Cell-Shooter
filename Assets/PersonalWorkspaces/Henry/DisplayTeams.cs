using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class DisplayTeams : MonoBehaviourPunCallbacks
{
    private static readonly string TeamPropKey = "TeamBlue?";

    public VerticalLayoutGroup TeamBlue;
    public VerticalLayoutGroup TeamRed;

    private void Update()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            object blueTeam;
            if (player.CustomProperties.TryGetValue(TeamPropKey, out blueTeam))
            {
                if ((bool)blueTeam)
                {
                    TeamBlue.transform.GetChild(0).GetComponent<TMP_Text>().SetText(player.NickName);
                } else
                {
                    TeamRed.transform.GetChild(0).GetComponent<TMP_Text>().SetText(player.NickName);
                }
            }
        }
    }
}
