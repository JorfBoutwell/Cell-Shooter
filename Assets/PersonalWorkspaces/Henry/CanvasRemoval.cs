using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CanvasRemoval : MonoBehaviourPunCallbacks
{
    public Canvas canvas;
    private void Awake()
    {
        if (!photonView.IsMine)
        {
            canvas.gameObject.SetActive(false);
        }
    }
}
