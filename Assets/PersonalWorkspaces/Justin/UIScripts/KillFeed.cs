using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;

public class KillFeed : MonoBehaviour
{
    [Header("Kill Feed Variables")]
    public GameObject killFeedBox;
    float yPos = 0f;
    public GameObject canvas1;
    public List<GameObject> boxes = new List<GameObject>();
    public int count = 1;

    //Alert Box variables
    public GameObject alertBox;

    //Username Variables
    public List<string> usernames = new List<string>();

    [Header("Animation Variables")]
    [SerializeField]
    private Vector2 transformPosition = Vector2.zero;

    [Range(1.0f, 10.0f), SerializeField]
    private float animationDuration = 5f;

    [SerializeField]
    //private Ease animationType = Ease.Linear;

    public PointContainer pointsA;
    public PointContainer pointsB;
    bool aHalfPoint = false;
    static bool bHalfPoint = false;

    public Image player1Image;
    public Image player2Image;

    public Image player1Icon;
    public Image player2Icon;

    bool test = false;
    public Train trainScript;

    public int boxesCount;

    public string player1;
    public string player2;

    public string alertText;

    public int pointsNeeded = 1000;

    public bool hitByTrain = false;


    void Start()
    {
        trainScript = GameObject.Find("Train").GetComponent<Train>();
        //canvas1 = GameObject.Find("GameCanvas");
        StartCoroutine("findCanvas");
        //Sets Player Usernames
        for(int i = 0; i < 4; i++)
        {
            usernames.Add("killers" + i); //8 Character Limit and no CAPS based on UI Size
            
        }

        if (GameObject.Find("Goober"))
        {
            pointsNeeded = 100;
        }
    }

    void Update()
    {
        boxesCount = boxes.Count;

        //Calls KillFeedInstantiate
        if (Input.GetKeyDown(KeyCode.K))
        {
            KillFeedInstantiate(boxesCount);
        }

        //Calls AlertFeedInstantiate
        if(Input.GetKeyDown(KeyCode.J))
        {
            AlertFeedInstantiate(boxesCount, alertText);
        }

        if((pointsA.points >= (pointsNeeded / 2) && !aHalfPoint) || (pointsB.points >= (pointsNeeded / 2) && !bHalfPoint))
        {
            AlertFeedInstantiate(boxesCount,alertText);
            
        }
    }

    //Instantiates Kill Feeds
    public void KillFeedInstantiate(int boxesCount)
    {
        yPos = 125 * count;
        count++;
        boxes.Add(Instantiate(killFeedBox, new Vector3(2195f, 1120f - yPos, 0f), transform.rotation) as GameObject);
        //boxes.Add(Instantiate(killFeedBox, new Vector3(4095f, 2120f - yPos, 0f), transform.rotation) as GameObject); //For monitor

        boxes[boxesCount].transform.DOMoveX(1695f, 0.1f);
        //boxes[boxesCount].transform.DOMoveX(3595f, 0.1f); //For monitor

        boxes[boxesCount].transform.SetParent(canvas1.transform);
        KillFeedText(boxesCount);
        StartCoroutine("KillFeedTimer");
    }

    //Kill Feed Duration
    public IEnumerator KillFeedTimer()
    {
        yield return new WaitForSeconds(4f);

        Destroy(boxes[0]);
        boxes.RemoveAt(0);

        for (int i = 0; i < boxes.Count; i++)
        {
            boxes[i].transform.DOMoveY(995f - (i * 125), animationDuration);
            //boxes[i].transform.DOMoveY(2895f - (i * 125), animationDuration); //For monitor

        }

        count--;
        player1 = "";
        player2 = "";
        hitByTrain = false;
    }

    //Sets Usernames in Kill Feed
    public void KillFeedText(int boxesCounts)
    {
        boxes[boxesCounts].GetComponentInChildren<TextMeshProUGUI>().text = player1 + " -> " + player2;
    }

    //Instantiates Alert Feeds
    public void AlertFeedInstantiate(int boxesCounts, string aText)
    {
        yPos = 125 * count;
        count++;
        boxes.Add(Instantiate(alertBox, new Vector3(2195f, 1120f - yPos, 0f), transform.rotation) as GameObject);
        //boxes.Add(Instantiate(alertBox, new Vector3(4095f, 2120f - yPos, 0f), transform.rotation) as GameObject); //For monitor

        boxes[boxesCounts].transform.DOMoveX(1695f, 0.1f);
        //boxes[boxesCounts].transform.DOMoveX(3595f, 0.1f); //For monitor


        boxes[boxesCounts].transform.SetParent(canvas1.transform);
        AlertText(boxesCounts, aText);
        StartCoroutine("AlertTimer");
    }

    //Alert Feed Duration
    public IEnumerator AlertTimer()
    {
        yield return new WaitForSeconds(8f);

        Destroy(boxes[0]);
        boxes.RemoveAt(0);

        for (int i = 0; i < boxes.Count; i++)
        {
            boxes[i].transform.DOMoveY(995f - (i * 125), animationDuration);
            //boxes[i].transform.DOMoveY(2895f - (i * 125), animationDuration); //For monitor

        }

        count--;
    }

    //Sets Text in Alert Feed
    public void AlertText(int boxesCounts, string aText)
    {
        if (pointsA.points >= (pointsNeeded/2) && !aHalfPoint) 
        {
            boxes[boxesCounts].GetComponentInChildren<TextMeshProUGUI>().text = "Team A is halfway there!";
            aHalfPoint = true;
        }
        else if(!bHalfPoint && (pointsB.points >= (pointsNeeded / 2))) 
        {
            bHalfPoint = true;
            boxes[boxesCounts].GetComponentInChildren<TextMeshProUGUI>().text = "Team B is halfway there!";
        }
        else { 
            boxes[boxesCounts].GetComponentInChildren<TextMeshProUGUI>().text = aText;
        }
    }

    public IEnumerator findCanvas()
    {
        yield return new WaitForSeconds(5f);
        canvas1 = GameObject.Find("PlayerDataCanvas");

    }

}
