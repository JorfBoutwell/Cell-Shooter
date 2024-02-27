using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronStats : MonoBehaviour
{
    public string currentNeurotransmitter;

    private void Awake()
    {
        currentNeurotransmitter = "adrenaline";
    }
}
