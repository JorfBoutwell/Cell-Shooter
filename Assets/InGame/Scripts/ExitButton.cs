using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ExitButton : MonoBehaviour
{

    public void ExitQueue()
    {
        PhotonNetwork.LocalPlayer.CustomProperties.Clear();
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(3);
    }
}
