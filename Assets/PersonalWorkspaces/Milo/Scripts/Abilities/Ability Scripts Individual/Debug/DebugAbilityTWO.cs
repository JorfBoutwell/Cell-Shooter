using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiloRey/Ability Objects/Ability2")]
public class DebugAbilityTWO : Ability
{
    public override void StartAbility(GameObject parent)
    {
        Debug.Log("Ability 2 Started!");
    }

    public override void BeginCooldown(GameObject parent)
    {
        Debug.Log("Ability 2 On Cooldown!");
    }
}
