//Milo Reynolds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiloRey/Ability Objects/Melee")]
public class Melee : Ability
{
    public override void StartAbility(GameObject parent)
    {
        Debug.Log("Melee Start");
    }
}
