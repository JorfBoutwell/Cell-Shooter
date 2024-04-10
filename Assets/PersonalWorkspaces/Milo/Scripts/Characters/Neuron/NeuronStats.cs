using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuronStats : Stats
{
    public string currentNeurotransmitter;

    private void Awake()
    {
        currentNeurotransmitter = "adrenaline"; 
    }
}
