using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "GeorgeBoutwell/PlayerReferences")]
public class PlayerRef : ScriptableObject
{
    //public AnimatorController animator;
    public RuntimeAnimatorController animator;
    public List<Ability> abilities;
    public WeaponObject weapon;
    public TrailRenderer bulletTrail;
    public Stats statScript;
    public Sprite icon;
}
