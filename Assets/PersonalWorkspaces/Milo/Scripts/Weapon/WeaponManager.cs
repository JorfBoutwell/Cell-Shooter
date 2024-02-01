//Milo Reynolds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class WeaponManager : MonoBehaviour
{
    PlayerController m_player;
    
    public AnimationController animController;

    public bool isShooting = false;
    public bool isAutoFiring = false;
    public bool isReloading = false;
    public WeaponObject currentWeapon;
    public Transform m_armTransform;
    public Transform bulletTransform;
    public Transform trailTransform;
    [SerializeField] LayerMask m_enemyMask;
    public TrailRenderer bulletTrail;
    [SerializeField]
    private float bulletSpeed = 5;

    public TextMeshProUGUI bulletUI;

    public int currentAmmo;

    private float m_autoTimer;

    private float m_resetBurstTimer = 1;
    private int m_fireLimit = 3;

    private string team;

    PhotonView view;

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
        animController = GetComponentInChildren<AnimationController>();
        m_autoTimer = currentWeapon.fireRate;
        currentAmmo = currentWeapon.maxAmmo;
        bulletUI.text = currentWeapon.maxAmmo.ToString("0");

        team = GetComponent<PlayerManager>().team;

        view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (!isShooting)
            m_fireLimit = 3;
        if (currentAmmo == 0)
        {
            StartCoroutine(Reload());
        }

        StateMachine();
    }

    public void SwitchWeapon()
    {
        var weaponPrefab = Instantiate(currentWeapon.modelPrefab, m_armTransform);
        //bulletTransform = weaponPrefab.transform.GetChild(0);
        m_shootingSystem = weaponPrefab.transform.GetChild(1).GetComponent<ParticleSystem>();
        m_autoTimer = currentWeapon.fireRate;
        currentAmmo = currentWeapon.maxAmmo;
    }

    private void StateMachine()
    {
        WeaponState tempState = state;

        if(isShooting)
        {
            state = WeaponState.shooting;
        }else if (isReloading){
            state = WeaponState.reloading;
        }else{
            state = WeaponState.idle;
        }

        if(tempState != state) //If state has changed this frame, play new anim
        {
            animController.WeaponAnimationController(state);
        }
    }

    public void FireWeapon()
    {
        if(!view.IsMine)
        {
            return;
        }
        if(isShooting == false && isReloading == false)
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
            yield return new WaitForSeconds(currentWeapon.fireFrame / 24);

            currentAmmo--;

            bulletUI.text = currentAmmo.ToString("0");

            if (Physics.Raycast(bulletTransform.transform.position, bulletTransform.transform.forward, out RaycastHit hit, currentWeapon.weaponRange, m_enemyMask))
            {
                switch (hit.transform.gameObject.layer)
                {
                    case 7: //"enemy"
                        Debug.Log("enemy");
                        hit.transform.gameObject.GetComponentInParent<EnemyManager>().health -= currentWeapon.damage;
                        break;
                    case 10: //"enemyHead"
                        Debug.Log("enemyHead");
                        hit.transform.gameObject.GetComponentInParent<EnemyManager>().health -= (currentWeapon.damage * 2);
                        break;
                    case 11: //teamA
                        if(team != "A")
                        {
                            Debug.Log("Team A");
                            PhotonView targetPhotonViewA = hit.transform.GetComponentInParent<PhotonView>();
                            view.RPC("RPC_TakeDamage", RpcTarget.AllBuffered, currentWeapon.damage, targetPhotonViewA.ViewID);
                        }
                        break;
                    case 13: //teamB
                        if (team != "B")
                        {
                            Debug.Log("Team B");
                            PhotonView targetPhotonViewB = hit.transform.GetComponentInParent<PhotonView>();
                            view.RPC("RPC_TakeDamage", RpcTarget.AllBuffered, currentWeapon.damage, targetPhotonViewB.ViewID);
                        }
                        break;
                    default: Debug.Log("nothing");  break;
                }
    
                GameObject trail = PhotonNetwork.Instantiate(bulletTrail.name, trailTransform.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hit));
            }

            yield return new WaitForSeconds(currentWeapon.fireRate - (currentWeapon.fireFrame / 24));

            if (currentWeapon.fireMode == "hitscan" && isAutoFiring)
            {
                StartCoroutine(Shoot());
            }else{
                isShooting = false;
            }
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
            bulletUI.text = currentAmmo.ToString("0");
            isReloading = false;
        }
        else
        {
            Debug.Log("Already at max ammo");
            yield return null;
        }
    }

    public IEnumerator SpawnTrail(GameObject trail, RaycastHit hit)
    {
        Debug.Log("spawn trail");
        Vector3 startPos = trail.transform.position;

        float distance = Vector3.Distance(startPos, hit.transform.position);
        float startDistance = distance;
        Debug.Log(distance);

        while(distance > 0)
        {
            Debug.Log("move");
            trail.transform.position = Vector3.Lerp(startPos, hit.point, 1 - (distance / startDistance));
            distance -= Time.deltaTime * bulletSpeed;
            yield return null;
        }

        trail.transform.position = hit.point;

        Destroy(trail, trail.GetComponent<TrailRenderer>().time);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage, int targetPhotonViewID)
    {
        PhotonView targetPhotonView = PhotonView.Find(targetPhotonViewID);

        if(targetPhotonView != null && targetPhotonView.GetComponent<PlayerManager>().isDead == false)
        {
            targetPhotonView.GetComponent<PlayerManager>().ApplyDamage(damage);
        }
    }
}
