//Milo Reynolds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronAOE : MonoBehaviour
{
    private float m_timer = 5;
    public int type;

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
                    col.gameObject.GetComponent<PlayerControllerNEW>().movementSpeed *= 2;
                if (type == 2)
                    Debug.Log("Dopamine Effect");
            }
        }
    }
}
