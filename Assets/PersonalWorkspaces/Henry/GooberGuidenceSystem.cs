using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooberGuidenceSystem : MonoBehaviour
{
    public GameObject goober;

    private void Start()
    {
        if(!GameObject.FindGameObjectWithTag("Goober"))
        {
            gameObject.SetActive(false);
        } else
        {
            goober = GameObject.FindGameObjectWithTag("Goober");
        }
    }
    // Update is called once per frame
    void Update()
    {

        gameObject.transform.rotation = Quaternion.Inverse(transform.root.GetChild(0).GetChild(2).rotation) * Quaternion.LookRotation(goober.transform.position - transform.root.position);
    }
}
