using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MiloRey/Ability Objects/Neuron/Neurotransmitter")]
public class Neurotransmitter : Ability
{   
    public GameObject adrenalineObject;
    public GameObject dopamineObject;
    public float projectileSpeed;
    public override void StartAbility(GameObject parent)
    {
        NeuronStats neuronStats = parent.GetComponent<NeuronStats>();
        WeaponManager weapon = parent.GetComponent<WeaponManager>();
        PlayerControllerNEW movement = parent.GetComponent<PlayerControllerNEW>();

        Transform firePoint = weapon.bulletTransform.transform;
        
        if(neuronStats.currentNeurotransmitter == "adrenaline")
        {
            var currentNeuroObj = Instantiate(adrenalineObject, firePoint.position, Quaternion.identity) as GameObject;
            currentNeuroObj.GetComponent<Rigidbody>().velocity = weapon.destination.forward * projectileSpeed;
        }
        if(neuronStats.currentNeurotransmitter == "dopamine")
        {
            var currentNeuroObj = Instantiate(dopamineObject, firePoint.position, Quaternion.identity) as GameObject;
            currentNeuroObj.GetComponent<Rigidbody>().velocity = weapon.destination.forward * projectileSpeed;
        }
    }
}
