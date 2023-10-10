//Milo Reynolds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public CharacterObject currentCharacter;
    private PlayerController m_movement;
    private WeaponManager m_weapon;

    private void Awake()
    {
        m_movement = GetComponent<PlayerController>();
        m_weapon = GetComponent<WeaponManager>();
        CharacterSwitch();
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.L))
        {
            CharacterSwitch();
        }
    }

    private void CharacterSwitch()
    {
        Debug.Log("You're currently playing as the " + currentCharacter.name);

        m_movement.speed = currentCharacter.characterSpeed;
        m_weapon.currentWeapon = currentCharacter.characterWeapons[0];
        m_weapon.SwitchWeapon();
    }
}
