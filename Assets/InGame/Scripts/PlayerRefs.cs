using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(menuName = "GeorgeBoutwell/PlayerReferences")]
public class PlayerRef : ScriptableObject
{
    public AnimatorController animator;
    public List<Ability> abilities;
    public WeaponObject weapon;
    public TrailRenderer bulletTrail;
}
