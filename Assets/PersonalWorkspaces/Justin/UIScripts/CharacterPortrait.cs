using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPortrait : MonoBehaviour
{
    [Header("Character Portrait Holder")]
    public GameObject currentCharacterPortrait;

    [Header("Character Portrait Images")]
    public Texture NeuronPFP;
    public Texture RedBloodCellPFP;

    //References
    public GameObject playerManager;
    private PlayerManager playerManagerScript;

    public string currentCharacter;

    //Sets Character Portrait
    void Update()
    {
        currentCharacter = playerManagerScript.character;

        switch (currentCharacter)
        {
            //Sets Neuron Character Portrait
            case "Neuron":
                currentCharacterPortrait.GetComponent<RawImage>().texture = NeuronPFP;
                break;

            //Sets Red Blood Cell Character Portrait
            case "RedBloodCell":
                currentCharacterPortrait.GetComponent<RawImage>().texture = RedBloodCellPFP;
                break;

            default:
                print("No character selected");
                break;



        }
    }

    
}
