using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Loading : MonoBehaviour
{
    public string[] tips;
    public TMP_Text loading;
    public TMP_Text tip;

    private void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        int r = Random.Range(0, tips.GetLength(0) - 1);
        tip.text = tips[r];
    }
}
