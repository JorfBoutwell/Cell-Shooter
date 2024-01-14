using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : ScriptableObject
{
    public string abilityName;
    public Image iconImage;
    public float cooldownTime;
    public float activeTime;
    
    public virtual void StartAbility(GameObject parent) {}
    public virtual void BeginCooldown(GameObject parent) {}
}
