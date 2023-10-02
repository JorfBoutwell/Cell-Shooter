//Milo Reynolds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiloRey/Character Object")]
public class CharacterObject : ScriptableObject
{
    [Header("Character Stats")]
    public string characterName;
    //public int characterHP;
    public float characterSpeed;

    [Header("Character Objects")]
    public WeaponObject[] characterWeapons;
    //public string[] characterAbilities
}
