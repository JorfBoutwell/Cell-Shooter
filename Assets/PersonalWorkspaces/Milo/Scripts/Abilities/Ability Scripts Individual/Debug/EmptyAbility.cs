using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiloRey/Ability Objects/Empty Ability")]
public class EmptyAbility : Ability
{
    public override void StartAbility(GameObject parent)
    {
        return;
    }

    public override void BeginCooldown(GameObject parent)
    {
        return;
    }
}
