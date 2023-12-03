using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Matchmaking : MonoBehaviour
{
   
    public TMP_Text timer;
    //int counter = 10;
    public string charName;
    
    // Start is called before the first frame update
    void Start()
    {
        //TMP_Text changeText = timer.GetComponent<TMP_Text>();
        //changeText.text = counter.ToString();

        //MainMenu.character = charName;
    }

    // Update is called once per frame
    void Update()
    {
        TMP_Text changeText = timer.GetComponent<TMP_Text>();
        //changeText.text = counter.ToString();
        changeText.text = charName;
        //counter -= 1;
        print(charName);
        //print("Character is: " + MainMenu.character);
    }
}
