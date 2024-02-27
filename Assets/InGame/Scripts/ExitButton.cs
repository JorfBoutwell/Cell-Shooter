using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ExitButton : MonoBehaviour
{

    public void ExitQueue()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel(3);
    }
}
