using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityImage : MonoBehaviour
{
    //public string currentCharacter;

    public PlayerManager playerManagerScript;

    [Header ("Ability Image Holders")]
    public GameObject abilityImageC;
    public GameObject abilityImageE;
    public GameObject abilityImageX;
    public GameObject abilityImageQ;

    [Header("Neuron Images")]
    public Texture NeuronC;
    public Texture NeuronE;
    public Texture NeuronX;
    public Texture NeuronQ;

    [Header("Red Blood Cell Images")]
    public Texture RedBloodCellC;
    public Texture RedBloodCellE;
    public Texture RedBloodCellX;
    public Texture RedBloodCellQ;

    // Start is called before the first frame update
    void Start()
    {

        switch (playerManagerScript.character)
        {
            //Sets Neuron Ability Icons
            case "Neuron":
                abilityImageC.GetComponent<RawImage>().texture = NeuronC;
                abilityImageE.GetComponent<RawImage>().texture = NeuronE;
                abilityImageX.GetComponent<RawImage>().texture = NeuronX;
                abilityImageQ.GetComponent<RawImage>().texture = NeuronQ;
                break;

            //Sets Red Blood Cell Ability Icons
            case "RedBloodCell":
                abilityImageC.GetComponent<RawImage>().texture = RedBloodCellC;
                abilityImageE.GetComponent<RawImage>().texture = RedBloodCellE;
                abilityImageX.GetComponent<RawImage>().texture = RedBloodCellX;
                abilityImageQ.GetComponent<RawImage>().texture = RedBloodCellQ;
                break;

            default:
                print("No character selected");
                break;


        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
