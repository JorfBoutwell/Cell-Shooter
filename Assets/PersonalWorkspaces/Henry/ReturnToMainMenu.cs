using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{
    public void ReturnToLobby()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
