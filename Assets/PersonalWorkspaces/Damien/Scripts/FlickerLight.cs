using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    private float TimeUntilFlicker = 0.0f;
    private float OffTime = 0;
    private GameObject NewTrain;

    // Start is called before the first frame update
    void Start()
    {
        TimeUntilFlicker = Random.Range(1.0f, 2.0f);

        NewTrain = GameObject.Find("Trains");
    }

    // Update is called once per frame
    void Update()
    {
        TimeUntilFlicker -= Time.deltaTime;

        if (NewTrain.GetComponent<NewTrain>().NextTrain > 1.5)
        {
            OffTime -= Time.deltaTime;
        }

        if (NewTrain.GetComponent<NewTrain>().NextTrain <= 4)
        {
            TimeUntilFlicker -= Time.deltaTime * 25;
        }

        if (TimeUntilFlicker <= 0)
        {
            if (NewTrain.GetComponent<NewTrain>().NextTrain <= 4)
            {
                TimeUntilFlicker = Random.Range(100.0f, 500.0f) / 100.0f;
            }
            else
            {
                TimeUntilFlicker = Random.Range(100.0f, 200.0f) / 100.0f;
            }

            OffTime = .05f;
        }

        if (OffTime > 0)
        {
            if (NewTrain.GetComponent<NewTrain>().NextTrain <= 4.5)
            {
                transform.gameObject.GetComponent<Light>().intensity = 0;
            }
            else
            {
                transform.gameObject.GetComponent<Light>().intensity = 1;
            }
        }
        else
        {
            transform.gameObject.GetComponent<Light>().intensity += .05f;

            transform.gameObject.GetComponent<Light>().intensity = Mathf.Min(transform.gameObject.GetComponent<Light>().intensity, 2);
        }
    }
}
