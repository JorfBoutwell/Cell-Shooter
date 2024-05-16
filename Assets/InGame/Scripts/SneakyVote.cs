using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SneakyVote : MonoBehaviour
{
    public string url;

    public void OpenLink()
    {
        Application.OpenURL(url);
    }

}
