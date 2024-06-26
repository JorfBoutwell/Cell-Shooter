
//Milo Reynolds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : ScriptableObject
{
    public string abilityName;
    public Sprite iconImage;
    public float abilityTime;
    public float animationFlag;
    public float cooldownTime;
    public bool isUsable;

    private void Awake()
    {
        isUsable = true;
    }
    public virtual void StartAbility(GameObject parent) { }

    public virtual void StartCooldown(GameObject parent) { }
}