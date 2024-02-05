//Milo Reynolds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public Ability[] abilityList;
    public Ability currentAbility;

    enum AbilityState
    {
        ready,
        active,
        inactive
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
                    state = AbilityState.active;
                }
            break;
            case AbilityState.active:
                if(currentAbility.isUsable)
                {
                    currentAbility.StartAbility(gameObject);
                    state = AbilityState.inactive;
                }
                else
                {
                    Debug.Log(currentAbility.abilityName + " still on Cooldown!");
                    currentAbility = null;
                    state = AbilityState.ready;
                }
            break;
            case AbilityState.inactive:
                StartCoroutine(HandleCooldown(currentAbility));
                currentAbility = null;
                state = AbilityState.ready;
            break;
            default:
                break;
        }
        
    }

    IEnumerator HandleCooldown(Ability currentAbility)
    {
        currentAbility.isUsable = false;
        yield return new WaitForSeconds(currentAbility.cooldownTime);
        currentAbility.isUsable = true;
    }
}
