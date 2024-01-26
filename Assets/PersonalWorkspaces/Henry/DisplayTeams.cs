using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class DisplayTeams : MonoBehaviourPunCallbacks
{
    private static readonly string TeamPropKey = "TeamA?";

    private static readonly string ReadyPropKey = "ReadyUp";

    public VerticalLayoutGroup teamA;
    public VerticalLayoutGroup teamB;

    private void Update()
    {
        int blueCounter = 0;
        int redCounter = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            object aTeam;
            object ready;
            if (player.CustomProperties.TryGetValue(TeamPropKey, out aTeam) && player.CustomProperties.TryGetValue(ReadyPropKey, out ready))
            {
                if ((bool)aTeam)
                {
                    teamA.transform.GetChild(blueCounter).gameObject.SetActive(true);
                    teamA.transform.GetChild(blueCounter).GetComponent<TMP_Text>().SetText(player.NickName);

                    if ((bool)ready)
                    {
                        teamA.transform.GetChild(blueCounter).GetChild(1).gameObject.SetActive(true);
                        teamA.transform.GetChild(blueCounter).GetChild(0).gameObject.SetActive(false);
                    } else
                    {
                        teamA.transform.GetChild(blueCounter).GetChild(0).gameObject.SetActive(true);
                        teamA.transform.GetChild(blueCounter).GetChild(1).gameObject.SetActive(false);
                    }
                    blueCounter++;
                } else
                {
                    teamB.transform.GetChild(redCounter).gameObject.SetActive(true);
                    teamB.transform.GetChild(redCounter).GetComponent<TMP_Text>().SetText(player.NickName);

                    if ((bool)ready)
                    {
                        teamB.transform.GetChild(redCounter).GetChild(1).gameObject.SetActive(true);
                        teamB.transform.GetChild(redCounter).GetChild(0).gameObject.SetActive(false);
                    }
                    else
                    {
                        teamB.transform.GetChild(redCounter).GetChild(0).gameObject.SetActive(true);
                        teamB.transform.GetChild(redCounter).GetChild(1).gameObject.SetActive(false);
                    }
                    redCounter++;
                }
            }
        }
    }
}
