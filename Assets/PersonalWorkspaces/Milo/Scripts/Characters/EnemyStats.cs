//Milo Reynolds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    private float m_hp = 100;
    public bool marked = false;
    private GameObject m_markObject;

    private void Awake()
    {
        m_markObject = this.gameObject.transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (marked)
        {
            m_markObject.SetActive(true);
        }
        else
        {
            m_markObject.SetActive(false);
        }
    }
}
