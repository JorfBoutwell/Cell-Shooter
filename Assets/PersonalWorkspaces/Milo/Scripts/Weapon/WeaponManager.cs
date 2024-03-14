//Milo Reynolds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class WeaponManager : MonoBehaviour
{
    PlayerController m_player;
    PlayerManager playerManagerScript;

    public AnimationController animController;

    public bool isShooting = false;
    public bool isAutoFiring = false;
    public bool isReloading = false;
    public WeaponObject currentWeapon;
    public Transform m_armTransform;
    public Transform bulletTransform;
    public Transform trailTransform;
    public GameObject hitInd;
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

    public Ability[] abilityList;
    public Ability currentAbility;
    public AbilityUI abilityUI;
    int ability = -1;

    public enum AbilityState
    {
        idle,
        ready,
        active,
        cooldown
    }

    public AbilityState abilityState = AbilityState.idle;

    [Header("Particle System")]
    [SerializeField] ParticleSystem m_shootingSystem;

    public Transform destination;

    public enum WeaponState
    {
        idle,
        shooting,
        reloading,
        melee,
        ability1,
        ability2,
        ability3
    }

    bool stateChange = true;
    public WeaponState state = WeaponState.idle;
    public WeaponState State
    {
        get { return state; }
        set
        {
            if (value != state)
            {
                stateChange = true;
            }
            state = value;
        }
    }

    private void Awake()
    {
        playerManagerScript = GetComponent<PlayerManager>();

        abilityUI = GetComponentInChildren<AbilityUI>();
        m_player = GetComponent<PlayerController>();
        animController = GetComponentInChildren<AnimationController>();
        currentAbility = null;
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

        AbilityStateMachine();
        StateMachine();

        destination = Camera.main.transform;
        if (stateChange) //If state has changed this frame, play new anim
        {
            Debug.Log("switch");
            animController.WeaponAnimationController(State);
            stateChange = false;
        }
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

        if (isShooting)
        {
            State = WeaponState.shooting;
        }

        if (isReloading)
        {
            State = WeaponState.reloading;
        }

        if ((abilityState == AbilityState.idle || abilityState == AbilityState.cooldown) && isReloading == false && isShooting == false)
        {
            State = WeaponState.idle;
        }

    }

    public void FireWeapon()
    {
        if (!view.IsMine)
        {
            return;
        }
        if(abilityState == AbilityState.active)
        {
            return;
        }
        if (isShooting == false && isReloading == false)
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

    private void AbilityStateMachine()
    {
        switch (abilityState)
        {
            case AbilityState.ready:
                if (currentAbility != null)
                {
                    abilityState = AbilityState.active;
                }
                break;
            case AbilityState.active:
                if (currentAbility.isUsable)
                {
                    if (State == WeaponState.idle) StartCoroutine(HandleAbility(currentAbility));
                }
                else
                {
                    currentAbility = null;
                    abilityState = AbilityState.idle;
                }
                break;
            case AbilityState.cooldown:
                StartCoroutine(HandleCooldown(currentAbility));
                currentAbility = null;
                abilityState = AbilityState.idle;
                break;
            default:
                break;
        }
    }

    IEnumerator HandleAbility(Ability currentAbility)
    {
        Debug.Log("handle ability");
        switch (ability)
        {
            case 0:
                State = WeaponState.ability1;
                currentAbility = abilityList[0];
                break;
            case 1:
                State = WeaponState.ability2;
                currentAbility = abilityList[1];
                break;
            case 2:
                State = WeaponState.ability3;
                currentAbility = abilityList[2];
                break;
            case 3:
                State = WeaponState.melee;
                currentAbility = abilityList[3];
                break;
        }

        currentAbility.StartAbility(gameObject);

        yield return new WaitForSeconds(currentAbility.abilityTime);

        currentAbility.StartCooldown(gameObject);

        Debug.Log("done");
        abilityState = AbilityState.cooldown;
        State = WeaponState.idle;
    }

    IEnumerator HandleCooldown(Ability currentAbility)
    {
        currentAbility.isUsable = false;
        yield return new WaitForSeconds(currentAbility.cooldownTime);
        currentAbility.isUsable = true;
    }

    public void UseAbility(int index)
    {
        if (view.IsMine)
        {
            if (State == WeaponState.idle)
            {
                switch (index)
                {
                    case 0:
                        ability = 0;
                        abilityState = AbilityState.ready;
                        currentAbility = abilityList[0];
                        StartCoroutine(abilityUI.abilityObjects[0].GetComponentInChildren<CooldownScript>(true).CooldownOverlay(currentAbility.cooldownTime));
                        break;
                    case 1:
                        ability = 1;
                        abilityState = AbilityState.ready;
                        currentAbility = abilityList[1];
                        StartCoroutine(abilityUI.abilityObjects[1].GetComponentInChildren<CooldownScript>(true).CooldownOverlay(currentAbility.cooldownTime));

                        break;
                    case 2:
                        ability = 2;
                        abilityState = AbilityState.ready;
                        currentAbility = abilityList[2];
                        StartCoroutine(abilityUI.abilityObjects[2].GetComponentInChildren<CooldownScript>(true).CooldownOverlay(currentAbility.cooldownTime));

                        break;
                    case 3:
                        ability = 3;
                        abilityState = AbilityState.ready;
                        currentAbility = abilityList[3];

                        break;
                    default: ability = -1; break;
                }
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
                    m_autoTimer = currentWeapon.fireRate;
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
        if (isShooting)
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
        if (state != WeaponState.idle) yield return null;
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
                        if (hit.transform.gameObject.GetComponentInParent<EnemyManager>().health <= 0)
                        {
                            Debug.Log("Killed enemy");
                            this.gameObject.GetComponentInChildren<KillFeed>().player2 = hit.transform.gameObject.GetComponent<PlayerManager>().username; //Not sure if this will work, try w/o transform?
                            //Later we have to use ... to sync to server: GameObject.Find("KillFeedObject").GetComponentInChildren<KillFeed>().player2 = hit.transform.gameObject.GetComponentInChildren<PlayerManager>().username;

                            this.gameObject.GetComponentInChildren<KillFeed>().player1 = playerManagerScript.username;
                            GameObject.Find("KillFeedObject").GetComponent<KillFeed>().KillFeedInstantiate(GameObject.Find("KillFeedObject").GetComponent<KillFeed>().boxesCount);
                        }
                        break;
                    case 10: //"enemyHead"
                        Debug.Log("enemyHead");
                        hit.transform.gameObject.GetComponentInParent<EnemyManager>().health -= (currentWeapon.damage * 2);
                        break;
                    case 11: //teamA
                        if (team != "A")
                        {
                            Debug.Log("Team A");
                            PhotonView targetPhotonViewA = hit.transform.GetComponentInParent<PhotonView>();
                            view.RPC("RPC_TakeDamage", RpcTarget.AllBuffered, currentWeapon.damage, targetPhotonViewA.ViewID);
                            StartCoroutine(ShowHitIndicator(0.4f));
                        }
                        break;
                    case 13: //teamB
                        if (team != "B")
                        {
                            Debug.Log("Team B");
                            PhotonView targetPhotonViewB = hit.transform.GetComponentInParent<PhotonView>();
                            view.RPC("RPC_TakeDamage", RpcTarget.AllBuffered, currentWeapon.damage, targetPhotonViewB.ViewID);
                            StartCoroutine(ShowHitIndicator(0.4f));
                        }
                        break;
                    default: Debug.Log("nothing          " + hit.transform.gameObject.layer); break;
                }

                GameObject trail = PhotonNetwork.Instantiate(bulletTrail.name, trailTransform.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hit));
            }

            yield return new WaitForSeconds(currentWeapon.fireRate - (currentWeapon.fireFrame / 24));

            if (currentWeapon.fireMode == "hitscan" && isAutoFiring)
            {
                StartCoroutine(Shoot());
            }
            else
            {
                isShooting = false;
            }
            yield return null;
        }
        else
        {
            Debug.Log("empty");
            isShooting = false;
            StartCoroutine(Reload());
            yield return null;
        }
    }

    public IEnumerator Reload()
    {
        Debug.Log("rstart");
        if (abilityState == AbilityState.active || state != WeaponState.idle)
        {
            isReloading = false;
            yield return null;
        }
        if (currentAmmo != currentWeapon.maxAmmo)
        {
            isReloading = true;

            yield return new WaitForSeconds(currentWeapon.reloadTime);
            Debug.Log(currentWeapon.reloadTime);
            Debug.Log("rdone");
            currentAmmo = currentWeapon.maxAmmo;
            bulletUI.text = currentAmmo.ToString("0");
            isReloading = false;
        }
        else
        {
            isReloading = false;
            Debug.Log("Already at max ammo");
            yield return null;
        }
    }

    public IEnumerator SpawnTrail(GameObject trail, RaycastHit hit)
    {
        Vector3 startPos = trail.transform.position;

        float distance = Vector3.Distance(startPos, hit.transform.position);
        float startDistance = distance;


        while (distance > 0)
        {
            trail.transform.position = Vector3.Lerp(startPos, hit.point, 1 - (distance / startDistance));
            distance -= Time.deltaTime * bulletSpeed;
            yield return null;
        }

        trail.transform.position = hit.point;

        Destroy(trail, trail.GetComponent<TrailRenderer>().time);
    }

    public IEnumerator ShowHitIndicator(float time)
    {
        hitInd.SetActive(true);
        yield return new WaitForSeconds(time);
        hitInd.SetActive(false);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage, int targetPhotonViewID)
    {
        PhotonView targetPhotonView = PhotonView.Find(targetPhotonViewID);

        if (targetPhotonView != null && targetPhotonView.GetComponent<PlayerManager>().isDead == false)
        {
            targetPhotonView.GetComponent<PlayerManager>().ApplyDamage(damage, transform.gameObject);
        }
    }
}