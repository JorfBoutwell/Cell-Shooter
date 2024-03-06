using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject spawnPoint;
    
    public List<GameObject> spawnPointsA;
    public List<GameObject> spawnPointsB;

    public float xIncrement = 0;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 4; i++)
        {
            
            spawnPointsA.Add(Instantiate(spawnPoint, new Vector3((-314f + xIncrement), 19.01464f, -362f), Quaternion.identity));
            spawnPointsB.Add(Instantiate(spawnPoint, new Vector3(-221f - xIncrement, 19.01464f, -380f), Quaternion.identity));
            xIncrement += 3f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
