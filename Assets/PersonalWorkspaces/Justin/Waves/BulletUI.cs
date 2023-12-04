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

    float reloadTime = 3f;

    
    public bool reloadActive = false;

    // Start is called before the first frame update
    void Start()
    {
        bulletCount.GetComponentInChildren<TextMeshProUGUI>().text = bulletAmount.ToString("0");
        }

    // Update is called once per frame
    void Update()
    {
        time++;

        if (Input.GetMouseButtonDown(0) && bulletAmount > 0f && !reloadActive)
        {
            if (Input.GetMouseButtonUp(0)) { 
                bulletAmount -= 1f;
                bulletCount.GetComponentInChildren<TextMeshProUGUI>().text = bulletAmount.ToString("0");
            }
        }
        else if(Input.GetMouseButton(0) && bulletAmount > 0f && time >= 100f && !reloadActive)
        {
            time = 0f;
            bulletAmount -= 1f;
            bulletCount.GetComponentInChildren<TextMeshProUGUI>().text = bulletAmount.ToString("0");
            
        }

        

        if (Input.GetKeyDown(KeyCode.R))
        {
            reloadActive = true;
            StartCoroutine("ReloadDelay");
        }
    }

    public void Shoot()
    {
        
    }

    public IEnumerator BulletDelay()
    {
        yield return new WaitForSeconds(2f);
        bulletAmount -= 1f;
        bulletCount.GetComponentInChildren<TextMeshProUGUI>().text = bulletAmount.ToString("0");
    }

    public IEnumerator ReloadDelay()
    {
        yield return new WaitForSeconds(3f);
        bulletAmount = 100f;
        bulletCount.GetComponentInChildren<TextMeshProUGUI>().text = bulletAmount.ToString("0");
        reloadActive = false;
    }
}
