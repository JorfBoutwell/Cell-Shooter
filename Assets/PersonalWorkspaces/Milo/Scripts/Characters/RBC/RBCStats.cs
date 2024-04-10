using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBCStats : Stats
{
    PlayerControllerNEW playerController;
    public float oxygen;

    private void Awake()
    {
        playerController = GetComponent<PlayerControllerNEW>();
        oxygen = 100f;
        playerController.maxJumpAmount = 1;
    }

    private void Update()
    {
        oxygen = Mathf.Clamp(0, 100, oxygen);

        if(oxygen == 0)
        {
            StartCoroutine(ReloadOxygen());
        }
    }

    IEnumerator ReloadOxygen()
    {
        Debug.Log("Reloading Oxygen...");
        yield return new WaitForSeconds(2);
        oxygen = 100;
        Debug.Log("Oxygen Reloaded!");
    }
}
