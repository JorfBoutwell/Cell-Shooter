using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class Survey : MonoBehaviour
{

    [SerializeField] InputField feedback1;

    string feedbackURL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSfITna2grW1kpGDhcn7TqB6Bm5HEua40P9gJ5eQxrz4pxYOuA/formResponse";
    string bugURL = "";

    string ratingForm = "1400390553";
    string feedbackForm = "757176778";

    string bugTypeForm = "358566118";
    string bugSummaryForm = "1345663906";

    public void Send()
    {
        //StartCoroutine(Post(feedback1.text));
    }

    /*IEnumerator Post(string s1)
    {
        WWWForm form = new WWWForm();
        form.AddField("358566118", s1);

//        UnityWebRequest www = UnityWebRequest.Post(URL, form);

        yield return www.SendWebRequest();

    }*/


}