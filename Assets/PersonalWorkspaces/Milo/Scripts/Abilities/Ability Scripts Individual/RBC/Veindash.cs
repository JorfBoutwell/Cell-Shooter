using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiloRey/Ability Objects/RBC/Veindash")]
public class Veindash : Ability
{
    public float dashPower;
    bool isAirDash;

    public override void StartAbility(GameObject parent)
    {
        Debug.Log("Use Dash");
        PlayerControllerNEW player = parent.GetComponent<PlayerControllerNEW>();
        Rigidbody rb = parent.GetComponent<Rigidbody>();
        RBCStats rbcStats = parent.GetComponent<RBCStats>();
        Vector3 forceToApply;

        if(rbcStats.oxygen > 0)
        {
            rb.useGravity = false;
            player.isDashing = true;

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
            rbcStats.oxygen -= 20;
        }
    }

    public override void StartCooldown(GameObject parent)
    {
        PlayerControllerNEW player = parent.GetComponent<PlayerControllerNEW>();
        Rigidbody rb = parent.GetComponent<Rigidbody>();

        player.isDashing = false;
        rb.useGravity = true;
        
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
