//Milo Reynolds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuroProjectile : MonoBehaviour
{
    private Rigidbody m_rb;
    [SerializeField] GameObject m_aoeObject;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision col)
    {
        m_rb.velocity = Vector3.zero;
        if (col.gameObject.tag == "ground")
        {
            Instantiate(m_aoeObject, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
