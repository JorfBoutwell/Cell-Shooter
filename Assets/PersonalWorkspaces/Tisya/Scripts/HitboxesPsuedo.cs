using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxesPsuedo : MonoBehaviour
{
    bool headShot = false;
    bool bodyShot = false;
    public float criticalHit = 2;
    public float damageDone = 1;
    public float enemyHealth = 10;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space));
        {
            headShot = true;
        }
        if (headShot)
        {
            enemyHealth -= criticalHit;
            headShot = false;
        
            //crit is double damage or dependent on weapon type
        } else if(bodyShot)
        {
            enemyHealth -= damageDone;
            bodyShot = false;
        }
    }
}
