using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    public Ability[] abilityList;
    public Ability currentAbility;
    float cooldownTime;
    float activeTime;

    enum AbilityState
    {
        ready,
        active,
        cooldown
    }
    AbilityState state = AbilityState.ready;

    private void Start()
    {
        currentAbility = null;
    }

    void Update()
    {
        switch (state)
        {
            case AbilityState.ready:
                if (currentAbility != null)
                {
                    currentAbility.StartAbility(gameObject);
                    state = AbilityState.active;
                    activeTime = currentAbility.activeTime;
                }
            break;
            case AbilityState.active:
                if (activeTime > 0)
                {
                    activeTime -= Time.deltaTime;
                }
                else
                {
                    currentAbility.BeginCooldown(gameObject);
                    state = AbilityState.cooldown;
                    cooldownTime = currentAbility.cooldownTime;
                    currentAbility = null;
                }
            break;
            case AbilityState.cooldown:
                if (cooldownTime > 0)
                {
                    cooldownTime -= Time.deltaTime;
                }
                else
                {
                    state = AbilityState.ready;
                }
                break;
            default:
                break;
        }
        
    }
}
