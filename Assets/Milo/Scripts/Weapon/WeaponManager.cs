//Milo Reynolds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    PlayerController m_player;

    public bool isShooting;
    public bool isReloading;
    public WeaponObject currentWeapon;
    [SerializeField] Transform m_armTransform;
    public Transform bulletTransform;
    [SerializeField] LayerMask m_enemyMask;

    public int currentAmmo;

    private float m_autoTimer;

    private float m_resetBurstTimer = 1;
    private int m_fireLimit = 3;

    [SerializeField] GameObject m_projectile;

    [Header("Particle System")]
    [SerializeField] ParticleSystem m_shootingSystem;

    public enum WeaponState
    {
        idle,
        shooting,
        reloading,
        melee, 
        abilty1,
    }

    public WeaponState state = WeaponState.idle;

    private void Awake()
    {
        m_player = GetComponent<PlayerController>();
        
    }

    private void Update()
    {
        FireWeapon(currentWeapon);
        if (!isShooting)
            m_fireLimit = 3;
        if (currentAmmo == 0)
        {
            StartCoroutine(Reload());
        }

        StateHandler();
    }

    public void SwitchWeapon()
    {
        var weaponPrefab = Instantiate(currentWeapon.modelPrefab, m_armTransform);
        bulletTransform = weaponPrefab.transform.GetChild(0);
        m_shootingSystem = weaponPrefab.transform.GetChild(1).GetComponent<ParticleSystem>();
        m_autoTimer = currentWeapon.fireRate;
        currentAmmo = currentWeapon.maxAmmo;
    }

    private void StateHandler()
    {
        if(isShooting)
        {
            state = WeaponState.shooting;
        }else if (isReloading){
            state = WeaponState.reloading;
        }else{
            state = WeaponState.idle;
        }

        /*
        Use inputs for other states: press e -> ability 1 = true -> state = ability one
        */
    }

    public void FireWeapon(WeaponObject weapon)
    {
        if (weapon.fireMode == "hitscan")
        {
            if (weapon.fireRate == 0f) // Semi Auto
            {
                SemiFire(weapon);
            }
            else //Full Auto
            {
                AutoFire(weapon);
            }
        }
        else if (weapon.fireMode == "burst") // Burst Fire
        {
            BurstFire(weapon);
        }
        else if (weapon.fireMode == "projectile")
        {
            Projectile(weapon);
        }
        else if (weapon.fireMode == "shotgun")
        {
            Shotgun(weapon);
        }
    }

    private void SemiFire(WeaponObject weapon)
    {
        if (isShooting)
        {
            Shoot();
            isShooting = false;
        }
    }

    private void AutoFire(WeaponObject weapon)
    {
        if (isShooting)
        {

            if (m_autoTimer <= 0)
            {
                Shoot();
                m_autoTimer = currentWeapon.fireRate;
            }
        }
        else
            m_autoTimer -= Time.deltaTime;
    }

    private void BurstFire(WeaponObject weapon)
    {
        if (isShooting)
        {
            m_autoTimer -= Time.deltaTime;

            if (m_autoTimer <= 0)
            {
                if (m_fireLimit != 0)
                {
                    Shoot();
                    m_autoTimer = weapon.fireRate;
                    m_fireLimit--;
                }
                else
                {

                    m_resetBurstTimer -= Time.deltaTime;

                    if (m_resetBurstTimer <= 0)
                    {
                        m_fireLimit = 3;
                        m_resetBurstTimer = 1;
                    }
                }
            }
        }
        else
            m_autoTimer -= Time.deltaTime;
    }

    private void Projectile(WeaponObject weapon)
    {
        if (isShooting)
        {
            m_autoTimer -= Time.deltaTime;

            if (m_autoTimer <= 0)
            {
                Shoot();
                m_autoTimer = currentWeapon.fireRate;
            }
        }
        else
            m_autoTimer -= Time.deltaTime;
    }

    private void Shotgun(WeaponObject weapon)
    {
        if(isShooting)
        {
            Shoot();
            isShooting = false;
        
            m_autoTimer -= Time.deltaTime;

            if (m_autoTimer <= 0)
            {
                isShooting = true;
                m_autoTimer = weapon.fireRate;
            }
        }   
    }

    private void Shoot()
    {
        if (currentAmmo > 0)
        {
            currentAmmo--;
            //m_shootingSystem.Play();
            if (Physics.Raycast(bulletTransform.transform.position, bulletTransform.transform.forward, out RaycastHit hit, currentWeapon.weaponRange, m_enemyMask))
            {
                Debug.Log(hit.transform.name);
            }
        }
    }

    public IEnumerator Reload()
    {
        if (currentAmmo != currentWeapon.maxAmmo)
        {
            Debug.Log("Reloading...");
            isReloading = true;

            yield return new WaitForSeconds(currentWeapon.reloadTime);
            currentAmmo = currentWeapon.maxAmmo;
            Debug.Log("Reloaded. Your ammo is " + currentAmmo);
            isReloading = false;
        }
        else
        {
            Debug.Log("Already at max ammo");
            yield return null;
        }
    }
}
