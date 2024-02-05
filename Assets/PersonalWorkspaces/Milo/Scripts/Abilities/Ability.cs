//Milo Reynolds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : ScriptableObject
{
    public string abilityName;
    public Image iconImage;
    public float cooldownTime;
    public bool isUsable;
    private void Awake()
    {
        isUsable = true;
    }
    public virtual void StartAbility(GameObject parent) {}
}
