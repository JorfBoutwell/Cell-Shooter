using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiloRey/Ability Objects/Neuron/Swap Neurotransmitter")]
public class SwapNeurotransmitter : Ability
{
    public override void StartAbility(GameObject parent)
    {
        NeuronStats neuronStats = parent.GetComponent<NeuronStats>();

        if (neuronStats.currentNeurotransmitter == "adrenaline")
            neuronStats.currentNeurotransmitter = "dopamine";
        else if (neuronStats.currentNeurotransmitter == "dopamine")
            neuronStats.currentNeurotransmitter = "adrenaline";
        else
            neuronStats.currentNeurotransmitter = "adrenaline";
    }
}
