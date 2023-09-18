//Milo Reynolds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    PlayerController m_player;


    public bool isFiring;
    public WeaponObject currentWeapon;
    [SerializeField] LayerMask m_enemyMask;
    Ray m_gunRay;

    private void Awake()
    {
        m_player = GetComponent<PlayerController>();
        m_gunRay = new Ray(m_player.FPS_camera.transform.position, m_player.FPS_camera.transform.forward);
    }

    private void Update()
    {
        FireWeapon(currentWeapon);
    }

    public void FireWeapon(WeaponObject weapon)
    {
        if (isFiring)
        {
            if (weapon.fireMode == "hitscan")
            {
                if (weapon.fireRate == 0f) // Semi Auto
                {
                    Debug.Log("Meow");
                    isFiring = false;
                }
                else //Full Auto
                {
                    Debug.Log("Woof");
                }
            }
            else if (weapon.fireMode == "burst")
            {

            }
            else if (weapon.fireMode == "projectile")
            {

            }
            else if (weapon.fireMode == "shotgun")
            {

            }
        }
    }
}
