using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTrain : MonoBehaviour
{
    public float NextTrain = 30f;
    private GameObject Train = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        NextTrain -= Time.deltaTime;

        if (NextTrain <= 0f)
        {
            NextTrain = Random.Range(20.0f, 20.0f);
        }

        if (NextTrain <= 1.4 && !Train)
        {
            if (Mathf.Round(Random.Range(0f, 1f)) == 1)
            {
                Train = GameObject.Find("Train1");
            }
            else
            {
                Train = GameObject.Find("Train0");
            }
        }

        if (NextTrain <= 15 && NextTrain >= 10)
        {
            Train = null;
            GameObject.Find("Train0").transform.position = new Vector3(-7.5f, 5f, -240f);
            GameObject.Find("Train1").transform.position = new Vector3(7.5f, 5f, 240f);
        }

        if (Train)
        {
            if (Train.name == "Train1")
            {
                Train.transform.position += new Vector3(0, 0, -Time.deltaTime * 250);
                Debug.Log("Train0");
            }
            if (Train.name == "Train0")
            {   
                Debug.Log("Train0");
                Train.transform.position += new Vector3(0, 0, Time.deltaTime * 250);
            }
        }
    }
}
