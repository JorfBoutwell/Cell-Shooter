//Milo Reynolds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronAOE : MonoBehaviour
{
    PlayerManager playerManager;

    private float m_timer = 5;
    public int type;

    private void Awake()
    {
        playerManager = FindObjectOfType<PlayerManager>();
    }

    private void Update()
    {
        if (m_timer > 0)
            m_timer -= Time.deltaTime;
        else
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "enemy")
        {
            if(gameObject.activeSelf == true)
            {
                if (type == 1)
                    if(!playerManager.activeEffects.Contains("adrenaline"))
                        playerManager.activeEffects.Add("adrenaline");
                if (type == 2)
                    if(!playerManager.activeEffects.Contains("dopamine"))
                        playerManager.activeEffects.Add("dopamine");

                playerManager.HandleEffects();
            }
        }
    }
}
