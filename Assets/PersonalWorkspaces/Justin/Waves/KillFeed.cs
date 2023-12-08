using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class KillFeed : MonoBehaviour
{
    //Kill Feed variables
    public GameObject killFeedBox;
    float yPos = 0f;
    public GameObject canvas1;
    public List<GameObject> boxes = new List<GameObject>();
    public int count = 1;

    //Alert Box variables
    public GameObject alertBox;

    //Player variables
    public List<string> usernames = new List<string>();

    [Header("Animation Variables")]
    [SerializeField]
    private Vector2 transformPosition = Vector2.zero;

    [Range(1.0f, 10.0f), SerializeField]
    private float animationDuration = 5f;

    [SerializeField]
    private Ease animationType = Ease.Linear;


    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 4; i++)
        {
            //8 character cap and no CAPS
            usernames.Add("killers" + i);
            Debug.Log(usernames[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        int boxesCount = boxes.Count;

        if (Input.GetKeyDown(KeyCode.K))
        {
            yPos = 125 * count;
            count++;
            boxes.Add(Instantiate(killFeedBox, new Vector3(2195f, 1120f - yPos, 0f), transform.rotation) as GameObject);
            boxes[boxesCount].transform.DOMoveX(1695f, 0.1f);

            boxes[boxesCount].transform.SetParent(canvas1.transform);
            KillFeedText(boxesCount);
            StartCoroutine("KillFeedTimer");
        }

        if(Input.GetKeyDown(KeyCode.J))
        {
            Alert(boxesCount);
        }
    }

    public IEnumerator KillFeedTimer()
    {
        yield return new WaitForSeconds(4f);

        Destroy(boxes[0]);
        boxes.RemoveAt(0);

        for (int i = 0; i < boxes.Count; i++)
        {
            boxes[i].transform.DOMoveY(995f - (i * 125), animationDuration);
        }

        count--;

    }

    public void KillFeedText(int boxesCounts)
    {
        boxes[boxesCounts].GetComponentInChildren<TextMeshProUGUI>().text = usernames[0] + " -> " + usernames[1];
    }

    public void Alert(int boxesCounts)
    {
        yPos = 125 * count;
        count++;
        boxes.Add(Instantiate(alertBox, new Vector3(2195f, 1120f - yPos, 0f), transform.rotation) as GameObject);
        boxes[boxesCounts].transform.DOMoveX(1695f, 0.1f);

        boxes[boxesCounts].transform.SetParent(canvas1.transform);
        AlertText(boxesCounts);
        StartCoroutine("AlertTimer");
    }

    public void AlertText(int boxesCounts)
    {
        boxes[boxesCounts].GetComponentInChildren<TextMeshProUGUI>().text = "INCOMING BOSS";


    }

    public IEnumerator AlertTimer()
    {
        yield return new WaitForSeconds(8f);

        Destroy(boxes[0]);
        boxes.RemoveAt(0);

        for (int i = 0; i < boxes.Count; i++)
        {
            boxes[i].transform.DOMoveY(995f - (i * 125), animationDuration);
        }

        count--;
    }
}
