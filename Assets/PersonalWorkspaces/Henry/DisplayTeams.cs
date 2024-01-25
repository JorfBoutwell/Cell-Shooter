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

    private static readonly string ReadyPropKey = "ReadyUp";

    public VerticalLayoutGroup TeamBlue;
    public VerticalLayoutGroup TeamRed;

    private void Update()
    {
        int blueCounter = 0;
        int redCounter = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            object blueTeam;
            object ready;
            if (player.CustomProperties.TryGetValue(TeamPropKey, out blueTeam) && player.CustomProperties.TryGetValue(ReadyPropKey, out ready))
            {
                if ((bool)blueTeam)
                {
                    TeamBlue.transform.GetChild(blueCounter).gameObject.SetActive(true);
                    TeamBlue.transform.GetChild(blueCounter).GetComponent<TMP_Text>().SetText(player.NickName);

                    if ((bool)ready)
                    {
                        TeamBlue.transform.GetChild(blueCounter).GetChild(1).gameObject.SetActive(true);
                        TeamBlue.transform.GetChild(blueCounter).GetChild(0).gameObject.SetActive(false);
                    } else
                    {
                        TeamBlue.transform.GetChild(blueCounter).GetChild(0).gameObject.SetActive(true);
                        TeamBlue.transform.GetChild(blueCounter).GetChild(1).gameObject.SetActive(false);
                    }
                    blueCounter++;
                } else
                {
                    TeamRed.transform.GetChild(redCounter).gameObject.SetActive(true);
                    TeamRed.transform.GetChild(redCounter).GetComponent<TMP_Text>().SetText(player.NickName);

                    if ((bool)ready)
                    {
                        TeamRed.transform.GetChild(redCounter).GetChild(1).gameObject.SetActive(true);
                        TeamRed.transform.GetChild(redCounter).GetChild(0).gameObject.SetActive(false);
                    }
                    else
                    {
                        TeamRed.transform.GetChild(redCounter).GetChild(0).gameObject.SetActive(true);
                        TeamRed.transform.GetChild(redCounter).GetChild(1).gameObject.SetActive(false);
                    }
                    redCounter++;
                }
            }
        }
    }
}
