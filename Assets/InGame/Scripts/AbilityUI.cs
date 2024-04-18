using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityUI : MonoBehaviour
{
    Ability[] abilities;
    public GameObject[] abilityObjects;
    string keys = "CEXQ";

    public GameObject abilityImage;

    public void GenerateAbilityUI()
    {
        abilities = GetComponentInParent<WeaponManager>().abilityList;
        abilityObjects = new GameObject[abilities.GetLength(0)];
        for(int i = 0; i < abilities.GetLength(0); i++)
        {
            if(abilities[i].name != "Melee")
            {
                GameObject img = (GameObject)Instantiate(abilityImage);
                img.transform.SetParent(transform, false);
                img.GetComponentInChildren<TMP_Text>().text = keys[i].ToString();
                abilityObjects[i] = img;
            }
        }
    }
}
