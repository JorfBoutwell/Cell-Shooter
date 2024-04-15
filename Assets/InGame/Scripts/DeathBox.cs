using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerManager playerManagerScript = other.GetComponent<PlayerManager>();
        playerManagerScript.health -= 100;
        playerManagerScript.isDead = true;

        if(playerManagerScript.goober.GetComponent<GooberFunctionality>().currentPlayer == playerManagerScript.gameObject)
        {
            playerManagerScript.goober.transform.position = new Vector3(8f, -16f, -17f);
        }
    }
}
