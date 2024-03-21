using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddHyphen : MonoBehaviour
{
    public InputField input;

    public void Changed()
    {
        if(input.text.Length > 3 && input.text[3] != '-')
        {
            input.text = input.text.Insert(3, "-");
            input.caretPosition = input.text.Length;
        }
    }
}
