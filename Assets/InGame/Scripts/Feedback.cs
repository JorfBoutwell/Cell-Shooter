using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class Survey : MonoBehaviour
{

    [SerializeField] InputField feedback1;

    string URL = "";


    public void Send()
    {
        StartCoroutine(Post(feedback1.text));
    }

    IEnumerator Post(string s1)
    {
        WWWForm form = new WWWForm();
        form.AddField("", s1);




        UnityWebRequest www = UnityWebRequest.Post(URL, form);

        yield return www.SendWebRequest();

    }


}