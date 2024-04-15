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

    private static readonly string IndividualCharacter = "individualCharacter";

    public VerticalLayoutGroup teamA;
    public VerticalLayoutGroup teamB;
    public int prevCount = 0;

    public bool game;

    public Sprite[] sprites;
    public string[] characters = new string[] { "Neuron", "Player2", "Player3", "PLayer4", "" };

    public Dictionary<string, Sprite> sprite = new Dictionary<string, Sprite>();

    private void Start()
    {
        if (!game)
        {
            for (int i = 0; i < characters.Length; i++)
            {
                sprite.Add(characters[i], sprites[i]);
            }
        }
    }

    private void Update()
    {
        if(PhotonNetwork.PlayerList.Length != prevCount)
        {
            foreach(Transform child in teamA.transform)
            {
                child.gameObject.SetActive(false);
            }
            foreach(Transform child in teamB.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
        Debug.Log(PhotonNetwork.PlayerList.Length);
        int blueCounter = 0;
        int redCounter = 0;
        int counter = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            object aTeam;
            object ready;
            object character;
            if (player.CustomProperties.TryGetValue(TeamPropKey, out aTeam) && player.CustomProperties.TryGetValue(ReadyPropKey, out ready) && player.CustomProperties.TryGetValue(IndividualCharacter, out character))
            {
                if ((bool)aTeam)
                {
                    teamA.transform.GetChild(blueCounter).gameObject.SetActive(true);
                    teamA.transform.GetChild(blueCounter).GetComponent<TMP_Text>().SetText(player.NickName);

                    /*if ((bool)ready && !game)
                    {
                        teamA.transform.GetChild(blueCounter).GetChild(1).gameObject.SetActive(true);
                        teamA.transform.GetChild(blueCounter).GetChild(0).gameObject.SetActive(false);
                    } else if (!game)
                    {
                        teamA.transform.GetChild(blueCounter).GetChild(0).gameObject.SetActive(true);
                        teamA.transform.GetChild(blueCounter).GetChild(1).gameObject.SetActive(false);
                    }*/

                    if(!game)
                    {
                        teamA.transform.GetChild(blueCounter).GetChild(0).GetComponent<Image>().enabled = true;
                        teamA.transform.GetChild(blueCounter).GetChild(0).GetComponent<Image>().sprite = sprite[(string)character];
                    }
                    blueCounter++;
                } else if(!(bool)aTeam)
                {
                    teamB.transform.GetChild(redCounter).gameObject.SetActive(true);
                    teamB.transform.GetChild(redCounter).GetComponent<TMP_Text>().SetText(player.NickName);

                    /*if ((bool)ready && !game)
                    {
                        teamB.transform.GetChild(redCounter).GetChild(1).gameObject.SetActive(true);
                        teamB.transform.GetChild(redCounter).GetChild(0).gameObject.SetActive(false);
                    }
                    else if (!game)
                    {
                        teamB.transform.GetChild(redCounter).GetChild(0).gameObject.SetActive(true);
                        teamB.transform.GetChild(redCounter).GetChild(1).gameObject.SetActive(false);
                    }*/

                    if (!game)
                    {
                        teamB.transform.GetChild(redCounter).GetChild(0).GetComponent<Image>().enabled = true;
                        teamB.transform.GetChild(redCounter).GetChild(0).GetComponent<Image>().sprite = sprite[(string)character];
                    }
                    redCounter++;
                }
            }
        }
    }
}
