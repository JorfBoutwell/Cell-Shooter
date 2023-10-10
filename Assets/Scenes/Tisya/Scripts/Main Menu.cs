using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button neuronSelect;
    public Button startBtn;
    bool characterSelected = false;
    public string character;

    // Start is called before the first frame update
    void Start()
    {
        Button neuron = neuronSelect.GetComponent<Button>();
        neuron.onClick.AddListener(TaskOnClick);
        Button start = startBtn.GetComponent<Button>();
        start.onClick.AddListener(startGame);
    }

    // Update is called once per frame
    void Update()
    {

    
    }

    void TaskOnClick()
    {
        character = "Neuron";
        characterSelected = true;
        print(character + " selected");

    }

    void startGame()
    {
        if (characterSelected)
        {
            SceneManager.LoadScene("MapWIP");
        }
        else
        {
            print("Please Select a Character");
        }
    }
}
