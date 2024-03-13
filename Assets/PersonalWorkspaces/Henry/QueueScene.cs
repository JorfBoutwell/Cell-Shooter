using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class QueueScene : MonoBehaviourPunCallbacks
{
    //key and local players team variable
    private static readonly string TeamPropKey = "TeamA?";
    public bool teamA = false;

    //key and local players ready status
    private static readonly string ReadyPropKey = "ReadyUp";
    private bool ready = false;

    //keys for characters custom variabels that will be attatched to the master client
    private static readonly string TeamATeam = "TeamACharacters";
    private static readonly string TeamBTeam = "TeamBCharacters";

    private static readonly string IndividualCharacter = "individualCharacter";

    //arrays that will keep track of characters used by each team
    public string[] TeamAArray = new string[4] { "", "", "", "" };
    public string[] TeamBArray = new string[4] { "", "", "", "" };

    public string[] characters = new string[] { "Player1", "Player2", "Player3", "PLayer4" };

    public GameObject start;

    int recentJoin = 60;

    public GameObject dictionary;

    public bool team;
    public string character = "";


    private void Start()
    {
        if (photonView.IsMine) PhotonNetwork.LocalPlayer.CustomProperties.Clear();
        dictionary = GameObject.Find("CustomVariableStorage");
        if (photonView.IsMine && recentJoin > 0)
        {
            recentJoin -= 1;
            //get list of all team blue players
            List<Player> teamA = GetTeamA();
            if (teamA.Count < PhotonNetwork.PlayerList.Length / 2)
            {
                //if less than or equal # of players are blue to red
                SetTeam(true);
                
            } else
            {
                //otherwise set this player on red team
                SetTeam(false);
               
            }
        }


        if (photonView.IsMine)
        {
            //function to set characters that only lets you get an avialable character
            setCharacter();
        }
        

        updateReadyState(false);
    }

    private void Update()
    {
        int readyCount = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            object readyUp;
            if (player.CustomProperties.TryGetValue(ReadyPropKey, out readyUp))
            {
                if ((bool)readyUp)
                {
                    readyCount++;
                }
            }

        }
        if (PhotonNetwork.IsMasterClient && readyCount == PhotonNetwork.PlayerList.Length && start != null)
        {
            start.SetActive(true);
        }
        else if (start!= null)
        {
            start.SetActive(false);
        }

        Debug.Log(TeamAArray[1]);
        Debug.Log(TeamBArray[1]);
    }

    public void CreateCharacterArays()
    {
        PhotonNetwork.MasterClient.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { TeamATeam, TeamAArray } });
        PhotonNetwork.MasterClient.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { TeamBTeam, TeamBArray } });
    }

    //finds element of an array made of strings
    public int IndexOfStringArray(string[] array, string element)
    {
        for(int i = 0; i < array.Length; i++)
        {
            if(array[i] == element)
            {
                return i;
            }
        }
        return 0;
    }

    //functon to set character, default choice is to be given an available one
    public void setCharacter(int choice = -1, bool teamSwitch = false)
    {


        //default choice
        if(choice == -1)
        {
            //runs through available characters
            for(int i = 0; i < characters.Length; i++)
            {
                //a team check
                if(team)
                {
                    //if no other player on your team has this operator
                    if(!OpUsed(characters[i], team))
                    {
                        //gives you the character
                        character = characters[i];
                        break;
                    }
                } else
                {
                    Debug.Log("I ended up on team B");
                    //same as above but if you are on team b
                    if (!OpUsed(characters[i], team))
                    {
                        character = characters[i];
                        break;
                    }
                }
            }
        }
        //if you give a choice
        else
        {
            //check if team a
            if(team)
            {
                //if your choice is taken
                if(OpUsed(characters[choice], team))
                {
                    //option wasn't there so don't change character
                    if (character != "" && teamSwitch)
                        return;

                    //recall this function but this time with default option (don't think this can happen but just in case)
                    setCharacter();
                } else
                {
                    //you choice works so sets character 
                    character = characters[choice];
                }
            } else
            {
                //same but team b
                if (OpUsed(characters[choice], team))
                {
                    if (character != "" && teamSwitch)
                        return;

                    setCharacter();
                }
                else
                {
                    character = characters[choice];
                }
            }
        }
        
        //set your custom variable to be the character you were assigned
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { IndividualCharacter, character } });
    }

    public bool OpUsed(string character, bool team)
    {
        List<string> teamAOps = new List<string>();
        List<string> teamBOps = new List<string>();

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            object teamP;
            object op;
            if (player.CustomProperties.TryGetValue(TeamPropKey, out teamP) && player.CustomProperties.TryGetValue(IndividualCharacter, out op) && !player.IsLocal)
            {
                if((bool)teamP)
                {
                    teamAOps.Add((string)op);
                } else
                {
                    teamBOps.Add((string)op);
                }
            }
        }
        if (team && teamAOps.Contains(character))
        {
            return true;
        } else if (!team && teamBOps.Contains(character))
        {
            return true;
        } else
        {
            return false;
        }
       
    }



    //sets team based on what is passeds
    public void SetTeam(bool value)
    {
        team = value;
        if (photonView.IsMine)
        {
            //stores passed variable
            teamA = value;
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
                teamA = (bool)changedProps[TeamPropKey];
                if (teamA)
                {
                    Debug.Log("Player is team A:" + targetPlayer.NickName);
                } else
                {
                    Debug.Log("Player is team B:" + targetPlayer.NickName);
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

                
            }

            //recieve that persons character
            if (changedProps.ContainsKey(IndividualCharacter))
            {
                Debug.Log("Player: " + targetPlayer.NickName + " is now character: " + (string)changedProps[IndividualCharacter]);
            }
        }
    }

    public void SwitchTeam()
    {
        if(team && PhotonNetwork.PlayerList.Length - GetTeamA().Count < 4)
        {
            SetTeam(false);
            setCharacter(IndexOfStringArray(characters, character), true);

        } else if (!team && GetTeamA().Count - PhotonNetwork.PlayerList.Length < 4)
        {
            SetTeam(true);
            setCharacter(IndexOfStringArray(characters, character), true);
        }

    }

    public void ExitQueue()
    {
        ExitGames.Client.Photon.Hashtable customProperties = photonView.Owner.CustomProperties;
        customProperties.Clear();
        photonView.Owner.SetCustomProperties(customProperties);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(3);
    }

    //gets list of all players on the blue team
    public List<Player> GetTeamA()
    {
        List<Player> aPlayers = new List<Player>();

        foreach(Player player in PhotonNetwork.PlayerList)
        {
            object aTeam;
            if (player.CustomProperties.TryGetValue(TeamPropKey, out aTeam))
            {
                if ((bool)aTeam)
                {
                    aPlayers.Add(player);
                }
            }
        }
        return aPlayers;
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                string teamInput;
                object aTeam;
                if (player.CustomProperties.TryGetValue(TeamPropKey, out aTeam) && (bool)aTeam)
                {
                    teamInput = "aTeam";

                } else
                {
                    teamInput = "bTeam";
                }
                dictionary.GetComponent<CustomVariableDictionary>().team.Add(player.ActorNumber, teamInput);
            }
            
            photonView.RPC("RPC_NewScene", RpcTarget.AllBuffered);
            PhotonNetwork.LoadLevel("TrainMap");
            //PhotonNetwork.LoadLevel("Multiplayer World");
        }
    }

    [PunRPC]
    void RPC_NewScene()
    {
        PlayerManager.DontDestroyOnLoad(dictionary);
        PhotonNetwork.AutomaticallySyncScene = true;

    }
}
