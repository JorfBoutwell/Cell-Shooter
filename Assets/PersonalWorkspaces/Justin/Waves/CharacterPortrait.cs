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

    public GameObject AbilityImage;
    private AbilityImage abilityImageScript;

    public string currentCharacter;
    
    // Start is called before the first frame update
    void Start()
    {
        abilityImageScript = AbilityImage.GetComponent<AbilityImage>();
    }

    // Update is called once per frame
    void Update()
    {
        currentCharacter = abilityImageScript.currentCharacter;

        switch (currentCharacter)
        {
            case "Neuron":
                currentCharacterPortrait.GetComponent<RawImage>().texture = NeuronPFP;
                break;

            case "RedBloodCell":
                currentCharacterPortrait.GetComponent<RawImage>().texture = RedBloodCellPFP;
                break;

            default:
                print("No character selected");
                break;



        }
    }

    
}
