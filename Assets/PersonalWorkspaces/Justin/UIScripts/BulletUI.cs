using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BulletUI : MonoBehaviour
{
    //Bullet variables
    public GameObject bulletCount;
    float bulletAmount = 100f;
    float time = 0f;

    public GameObject player;
    private PlayerManager playerManagerScript;
    private WeaponManager weaponManagerScript;


    public bool reloadActive = false;

    // Start is called before the first frame update
    void Start()
    {
        bulletCount.GetComponentInChildren<TextMeshProUGUI>().text = bulletAmount.ToString("0");

        playerManagerScript = player.GetComponent<PlayerManager>();
        weaponManagerScript = player.GetComponent<WeaponManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if ((weaponManagerScript.state == WeaponManager.WeaponState.shooting) && playerManagerScript.ammo > 0f && !weaponManagerScript.isReloading)
        {
            if (Input.GetMouseButtonUp(0)) {

                bulletCount.GetComponentInChildren<TextMeshProUGUI>().text = playerManagerScript.ammo.ToString("0");
            }
        }

    }


}
