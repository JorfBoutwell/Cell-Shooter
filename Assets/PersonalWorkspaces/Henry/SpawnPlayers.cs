using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject player;
    public TMP_Text code;

    private void Start()
    {
        player = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(-15, 2, -20), Quaternion.identity);
        code = player.transform.GetChild(1).GetChild(1).GetChild(1).gameObject.GetComponent<TMP_Text>();
        code.SetText("Code: " + PhotonNetwork.CurrentRoom.Name);
    }


}
