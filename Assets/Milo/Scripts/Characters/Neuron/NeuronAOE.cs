//Milo Reynolds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronAOE : MonoBehaviour
{
    private float m_timer = 5;
    public int type;
    private void Awake()
    {
        if (gameObject.tag == "adrenaline")
            type = 1;
        else if (gameObject.tag == "gaba")
            type = 2;
        else if (gameObject.tag == "dopamine")
            type = 3;
        else if (gameObject.tag == "glutamate")
            type = 4;
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
        if (col.gameObject.tag == "player" || col.gameObject.tag == "enemy")
        {
            if (type == 1)
                col.gameObject.GetComponent<PlayerController>().speed = 16;
            if (type == 2)
                col.gameObject.GetComponent<PlayerController>().speed = 4;
            if (type == 3)
                Debug.Log("you are being healed over time!");
            if (type == 4)
                Debug.Log("you are being damaged over time!");
        }
    }
}
