using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiloRey/Ability Objects/Neuron/Neurostep")]
public class Neurostep : Ability
{
    public float dashPower;
    bool isAirDash;
    public override void StartAbility(GameObject parent)
    {
        Debug.Log("Use Dash");
        PlayerControllerNEW player = parent.GetComponent<PlayerControllerNEW>();
        Rigidbody rb = parent.GetComponent<Rigidbody>();
        Vector3 forceToApply;

        player.isDashing = true;
        rb.useGravity = false;

        if (player.state == PlayerControllerNEW.MovementState.air)
        {
            isAirDash = true;
            forceToApply = player.FPSCam.transform.forward * dashPower;
        }
        else
        {
            isAirDash = false;
            forceToApply = player.orientation.forward * dashPower;
        }

        rb.AddForce(forceToApply, ForceMode.Impulse);
    }

    public override void StartCooldown(GameObject parent)
    {
        PlayerControllerNEW player = parent.GetComponent<PlayerControllerNEW>();
        Rigidbody rb = parent.GetComponent<Rigidbody>();

        player.isDashing = false;
        rb.useGravity = false;
        
        if (isAirDash)
        {
            rb.velocity /= 4f;
        }
        else
        {
            rb.velocity /= 2.5f;
        }
    }
}
