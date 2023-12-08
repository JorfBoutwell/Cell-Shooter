//Milo Reynolds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    PlayerController m_player;

    public bool isShooting = false;
    public bool isAutoFiring = false;
    public bool isReloading = false;
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
        m_autoTimer = currentWeapon.fireRate;
        currentAmmo = currentWeapon.maxAmmo;
    }

    private void Update()
    {
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

    public void FireWeapon()
    {
        if(isShooting == false)
        {
            isShooting = true;
            if (currentWeapon.fireMode == "hitscan")
            {
                if (currentWeapon.fireRate == 0f) // Semi Auto
                {
                    SemiFire();
                }
                else //Full Auto
                {
                    AutoFire();
                }
            }
            else if (currentWeapon.fireMode == "burst") // Burst Fire
            {
                BurstFire();
            }
            else if (currentWeapon.fireMode == "projectile")
            {
                Projectile();
            }
            else if (currentWeapon.fireMode == "shotgun")
            {
                Shotgun();
            }
        }
    }

    private void SemiFire()
    {
        if (isShooting)
        {
            StartCoroutine(Shoot());
        }
    }

    private void AutoFire()
    {
        if (isShooting)
        {
            StartCoroutine(Shoot());
        }

    }

    //Burst fire, Projectile, and Shotgun still rely on a timer ticking down every frame, so they won't work and need to be rewritten. As it stands, auto isn't "auto"
    //you still have to click once per fire. However, we can get to holding down to fire another time. Should be easy to implement. Hopefully :,-)
    private void BurstFire()
    {
        if (isShooting)
        {
            m_autoTimer -= Time.deltaTime;

            if (m_autoTimer <= 0)
            {
                if (m_fireLimit != 0)
                {
                    StartCoroutine(Shoot());
                    m_autoTimer =currentWeapon.fireRate;
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

    private void Projectile()
    {
        if (isShooting)
        {
            m_autoTimer -= Time.deltaTime;

            if (m_autoTimer <= 0)
            {
                StartCoroutine(Shoot());
                m_autoTimer = currentWeapon.fireRate;
            }
        }
        else
            m_autoTimer -= Time.deltaTime;
    }

    private void Shotgun()
    {
        if(isShooting)
        {
            StartCoroutine(Shoot());
            isShooting = false;
        
            m_autoTimer -= Time.deltaTime;

            if (m_autoTimer <= 0)
            {
                isShooting = true;
                m_autoTimer = currentWeapon.fireRate;
            }
        }   
    }

    private IEnumerator Shoot()
    {
        if (currentAmmo > 0)
        {
            yield return new WaitForSeconds(currentWeapon.fireRate);

            if (Physics.Raycast(bulletTransform.transform.position, bulletTransform.transform.forward, out RaycastHit hit, currentWeapon.weaponRange, m_enemyMask))
            {
                Debug.Log(hit.transform.name);
            }

            if (currentWeapon.fireMode == "hitscan" && isAutoFiring)
            {
                StartCoroutine(Shoot());
            }else{
                isShooting = false;
            }

            currentAmmo--;
            yield return null;
        }else{
            isShooting = false;
            StartCoroutine(Reload());
            yield return null;
        }
    }

    public IEnumerator Reload()
    {
        if (currentAmmo != currentWeapon.maxAmmo)
        {
            isReloading = true;

            yield return new WaitForSeconds(currentWeapon.reloadTime);
            currentAmmo = currentWeapon.maxAmmo;
            isReloading = false;
        }
        else
        {
            Debug.Log("Already at max ammo");
            yield return null;
        }
    }
}
