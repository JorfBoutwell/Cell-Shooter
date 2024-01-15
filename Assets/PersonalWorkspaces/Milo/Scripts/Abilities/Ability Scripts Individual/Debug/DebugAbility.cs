using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiloRey/Ability Objects/Ability1")]
public class DebugAbility : Ability
{
    public override void StartAbility(GameObject parent)
    {
        Debug.Log("Ability 1 Started!");
    }

    public override void BeginCooldown(GameObject parent)
    {
        Debug.Log("Ability 1 On Cooldown!");
    }
}
