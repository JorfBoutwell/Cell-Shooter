using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button neuronBtn;
    public Button redBloodBtn;
    public Button tcellBtn;
    public Button osteoclastBtn;
    public Button startBtn;


    bool characterSelected = false;
    public string character;

    // Start is called before the first frame update
    void Start()
    {
        Button neuron = neuronBtn.GetComponent<Button>();
        neuron.onClick.AddListener(neuronSelect);

        Button redBlood = redBloodBtn.GetComponent<Button>();
        redBlood.onClick.AddListener(redBloodSelect);

        Button tCell = tcellBtn.GetComponent<Button>();
        tCell.onClick.AddListener(tcellSelect);

        Button osteoClast = osteoclastBtn.GetComponent<Button>();
        osteoClast.onClick.AddListener(osteoclastSelect);

        Button start = startBtn.GetComponent<Button>();
        start.onClick.AddListener(startGame);
    }

    // Update is called once per frame
    void Update()
    {

    
    }

    void neuronSelect()
    {
        character = "Neuron";
        characterSelected = true;
        print(character + " selected");
        
    }

    void redBloodSelect()
    {
        character = "Red Blood Cell";
        characterSelected = true;
        print(character + " selected");

    }

    void tcellSelect()
    {
        character = "T-Cell";
        characterSelected = true;
        print(character + " selected");
    }

    void osteoclastSelect()
    {
        character = "Osteoclast";
        characterSelected = true;
        print(character + " selected");
    }
    
    void startGame()
    {
        if (characterSelected)
        {
            SceneManager.LoadScene("Matchmaking");
        }
        else
        {
            print("Please Select a Character");
        }
    }
}
