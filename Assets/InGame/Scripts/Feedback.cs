using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;


public class Feedback : MonoBehaviour
{

    [Header("FeedbackRefs")]
    public TMP_InputField feedbackInput;
    public Toggle[] feedbackToggles;

    [Header("BugRefs")]
    public TMP_InputField bugInput;
    public TMP_Dropdown bugDropdown;

    string feedbackURL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSdF-lKLG9cMD5or-BSZqNLr_6Pm81Vknhz-FCE8l3-v961Qxg/formResponse";
    string bugURL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSfITna2grW1kpGDhcn7TqB6Bm5HEua40P9gJ5eQxrz4pxYOuA/formResponse";

    string feedbackForm = "entry.1400390553";
    string ratingForm = "entry.757176778";

    string bugTypeForm = "entry.358566118";
    string bugSummaryForm = "entry.1345663906";

    public void SendFeedback()
    {
        StartCoroutine(FeedbackPost());
    }

    public void SendBugReport()
    {
        StartCoroutine(BugReportPost());
    }

    IEnumerator FeedbackPost()
    {
       if (feedbackInput.text != "")
        {
            Debug.Log("running");
            WWWForm form = new WWWForm();

            string rating = "0";

            for(int i = 1; i <= feedbackToggles.GetLength(0); i++)
            {
                if(feedbackToggles[i-1].isOn)
                {
                    rating = i.ToString();
                }
            }

            form.AddField(ratingForm, rating);
            form.AddField(feedbackForm, feedbackInput.text);

            UnityWebRequest www = UnityWebRequest.Post(feedbackURL, form);

            yield return www.SendWebRequest();

            if(www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("failed");
            }
            else
            {
                Debug.Log("Sent!");
            }
            www.Dispose();
        }
        else
        {
            yield return null;
        }

    }

    IEnumerator BugReportPost()
    {
        if(bugInput.text != "")
        {
            WWWForm form = new WWWForm();
            form.AddField(bugTypeForm, bugDropdown.captionText.text);
            form.AddField(bugSummaryForm, bugInput.text);

            UnityWebRequest www = UnityWebRequest.Post(bugURL, form);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("failed");
            }
            else
            {
                Debug.Log("Sent!");
            }

            www.Dispose();
        }
        else
        {
            yield return null;
        }
    }


}