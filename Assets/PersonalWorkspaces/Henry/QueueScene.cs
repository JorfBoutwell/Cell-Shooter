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

    //key used for each persons player custom variable
    private static readonly string IndividualCharacter = "individualCharacter";

    //string of names read from to assign player character.
    //This and display team are their locations that both need to be changed if a change is made here
    public string[] characters = new string[] { "Neuron", "Player2", "Player3", "PLayer4" };

    //gameobject for start button needed to make it visible or not
    public GameObject start;

    //timer I use for update checks
    int recentJoin = 60;

    //gameobject I don't destory to pass custom variables to actual game
    public GameObject dictionary;

    //bool to tell what team this player is on. True = A (blue), False = B (red)
    public bool team;
    //string that will be stored here and passed into the game as a custom variable
    //telling what character they are
    public string character = "";


    private void Start()
    {
        //if you are a using this script, clear your previous custom properties
        if (photonView.IsMine) PhotonNetwork.LocalPlayer.CustomProperties.Clear();
        //find dictionary gameobject for later
        dictionary = GameObject.Find("CustomVariableStorage");
        //if recent join timer > 0 and this is a live player
        if (photonView.IsMine && recentJoin > 0)
        {
            //remove from recentjoin counter
            recentJoin -= 1;
            //get list of all team blue players
            List<Player> teamA = GetTeamA();
            //if less people are on team A than half the total amount of players
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
        //allow others to join the room, only runs on master clients script
        if(PhotonNetwork.IsMasterClient && photonView.IsMine)
        {
            PhotonNetwork.CurrentRoom.IsOpen = true;
        }

        if (photonView.IsMine)
        {
            //function to set characters that only lets you get an avialable character
            setCharacter();
        }
        
        //says you are not ready
        updateReadyState(false);
    }

    private void Update()
    {
        //check how many people are ready
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

        //on maser clients script, uses the count to turn on or off start button for master client
        if (PhotonNetwork.IsMasterClient && readyCount == PhotonNetwork.PlayerList.Length && start != null)
        {
            start.SetActive(true);
        }
        else if (start!= null)
        {
            start.SetActive(false);
        }
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
    public void setCharacter(int choice = -1)
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
                    if (character != "")
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
                    if (character != "")
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
    //checks if someone on your team already is using the character you want
    public bool OpUsed(string character, bool team)
    {
        //makes a list of charectors used on each team
        List<string> teamAOps = new List<string>();
        List<string> teamBOps = new List<string>();
        //runs through every player
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            //checks team and current character they are using and adds them to their respective list
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
        //if it is in your team already the function returns true  otherwise false
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
        //resets recently joined counter
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
    //function called when you hit the change team button
    public void SwitchTeam()
    {
        //checks if the opposite team has less than 4 players
        if(team && PhotonNetwork.PlayerList.Length - GetTeamA().Count < 4)
        {
            //sets opposite team
            SetTeam(false);
            //runs set character function, trying to use your current player as choice
            setCharacter(IndexOfStringArray(characters, character));

        } else if (!team && GetTeamA().Count - PhotonNetwork.PlayerList.Length < 4)
        {
            //sets opposite team
            SetTeam(true);
            //runs set character function, trying to use your current player as choice
            setCharacter(IndexOfStringArray(characters, character));
        }

    }
    //ran when you use the exit button in the queue scene
    public void ExitQueue()
    {
        //clears your custom properties
        ExitGames.Client.Photon.Hashtable customProperties = photonView.Owner.CustomProperties;
        customProperties.Clear();
        //synchs the clearing
        photonView.Owner.SetCustomProperties(customProperties);
        //leaves the room and loads the lobby
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
    //ran by master client when they hit start game button
    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //runs through each player
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                
                string teamInput;
                object aTeam;
                object playerCharacter;
                //gets current team and assigns a string value
                if (player.CustomProperties.TryGetValue(TeamPropKey, out aTeam) && (bool)aTeam)
                {
                    teamInput = "aTeam";

                } else
                {
                    teamInput = "bTeam";
                }
                //adds to dictionary variable with a key of actor number and a input of their character
                if (player.CustomProperties.TryGetValue(IndividualCharacter, out playerCharacter))
                {
                    dictionary.GetComponent<CustomVariableDictionary>().character.Add(player.ActorNumber, (string)playerCharacter);
                }
                //adds to dictionary variable with key value actor number and input value of team string value
                dictionary.GetComponent<CustomVariableDictionary>().team.Add(player.ActorNumber, teamInput);
                
            }
            //runs new scene on everyones screen
            photonView.RPC("RPC_NewScene", RpcTarget.All);
            //locks room to others
            PhotonNetwork.CurrentRoom.IsOpen = false;
            //loads nex level
            PhotonNetwork.LoadLevel("CaptureTheFlagTrainMap");
            //PhotonNetwork.LoadLevel("Multiplayer World");
        }
    }

    [PunRPC]
    void RPC_NewScene()
    {
        //tells everyone not to destory the dictionary gameobject
        PlayerManager.DontDestroyOnLoad(dictionary);
        //synchs scenes so people change together
        PhotonNetwork.AutomaticallySyncScene = true;

    }
}
