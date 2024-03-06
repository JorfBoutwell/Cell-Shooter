using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject spawnPoint;
    
    public List<GameObject> spawnPointsA;
    public List<GameObject> spawnPointsB;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 5; i++)
        {
            spawnPointsA.Add(Instantiate(spawnPoint, new Vector3(0, 0, 0), Quaternion.identity));
            spawnPointsB.Add(Instantiate(spawnPoint, new Vector3(0, 0, 0), Quaternion.identity));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
