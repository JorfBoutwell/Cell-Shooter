using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiloRey/Ability Objects/RBC/MysteryBox")]
public class MysteryBox : Ability
{
    public GameObject mysteryBoxPrefab;
    public float throwForce;
    public override void StartAbility(GameObject parent)
    {
        var mysteryBoxObj = Instantiate(mysteryBoxPrefab, new Vector3(parent.transform.position.x, parent.transform.position.y, parent.transform.position.z + 3), Quaternion.identity);
        Rigidbody mbRb = mysteryBoxObj.GetComponent<Rigidbody>();
        Vector3 forceDirection = parent.GetComponent<PlayerControllerNEW>().FPSCam.transform.forward * 10;
        mbRb.AddForce(forceDirection * throwForce);
    }

    public override void StartCooldown(GameObject parent)
    {
        base.StartCooldown(parent);
    }
}
