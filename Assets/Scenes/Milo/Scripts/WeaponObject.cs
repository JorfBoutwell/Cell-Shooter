//Milo Reynolds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiloRey/Weapon Object")]
public class WeaponObject : ScriptableObject
{
    public float maxAmmo;
    public float fireRate;
    public float damage;
    public float reloadTime;
    public float weaponRange;
    public string fireMode;
}
