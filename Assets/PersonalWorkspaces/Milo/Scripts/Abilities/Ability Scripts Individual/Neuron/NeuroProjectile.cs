using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuroProjectile : MonoBehaviour
{
    public int type;
    SphereCollider gameObjectCollider;
    public GameObject aoePrefab;

    private void Awake()
    {
        gameObjectCollider = GetComponent<SphereCollider>();
        gameObjectCollider.isTrigger = true;
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        yield return new WaitForSeconds(0.1f);
        gameObjectCollider.isTrigger = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            DestroyAOE();
            if (type == 1)
            {
                Instantiate(aoePrefab, gameObject.transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            else if (type == 2)
            {
                Instantiate(aoePrefab, gameObject.transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

    private void DestroyAOE()
    {
        NeuronAOE[] existingAoe = FindObjectsOfType(typeof(NeuronAOE)) as NeuronAOE[];
        foreach (NeuronAOE aoe in existingAoe)
        {
            aoe.gameObject.SetActive(false);
        }
    }
}
