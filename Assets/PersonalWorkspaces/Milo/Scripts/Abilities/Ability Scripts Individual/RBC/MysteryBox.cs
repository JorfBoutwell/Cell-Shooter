using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiloRey/Ability Objects/RBC/MysteryBox")]
public class MysteryBox : Ability
{
    public GameObject mysteryBoxPrefab;
    public float throwForce;
    public LayerMask targetLayer;
    private bool inAir = false;
    public override void StartAbility(GameObject parent)
    {
        inAir = false;
        var mysteryBoxObj = Instantiate(mysteryBoxPrefab, parent.transform.position, Quaternion.identity);
        mysteryBoxObj.GetComponent<Rigidbody>().AddRelativeForce(parent.GetComponent<PlayerControllerNEW>().FPSCam.transform.forward * throwForce, ForceMode.Force);
        inAir = true;

        if(inAir)
        {
            Collider[] colliders = Physics.OverlapSphere(mysteryBoxObj.transform.position, 2f, targetLayer);

            if (colliders != null)
            {
                int boxEffect = Random.Range(0, 12);

                if (boxEffect >= 0 && boxEffect <= 4)
                {
                    Debug.Log("Antibody Release");
                }
                else if (boxEffect >= 5 && boxEffect <= 8)
                {
                    Debug.Log("Neurotransmitter Bomb");
                }
                else if (boxEffect >= 9 && boxEffect <= 11)
                {
                    Debug.Log("Color Bomb");
                }
                else if (boxEffect == 12)
                {
                    Debug.Log("Mini Nuke");
                }
                else
                {
                    Debug.Log("Unkown Box");
                }
                Destroy(mysteryBoxObj);
            }
        }
    }

    public override void StartCooldown(GameObject parent)
    {
        base.StartCooldown(parent);
    }
}
