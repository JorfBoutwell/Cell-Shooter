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
    public bool remove = false;

    public int current = 0;

    //Kill Feed Animation variables
    [SerializeField]
    private Vector2 transformPosition = Vector2.zero;

    [Range(1.0f, 10.0f), SerializeField]
    private float animationDuration = 5f;

    [SerializeField]
    private Ease animationType = Ease.Linear;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            int boxesCount = boxes.Count;
            yPos = 125 * count;
            count++;
            boxes.Add(Instantiate(killFeedBox, new Vector3(1700f, 1100f - yPos, 0f), transform.rotation) as GameObject);
            boxes[boxesCount].transform.SetParent(canvas1.transform);
            StartCoroutine("KillFeedTimer", boxesCount);
            //Raise(boxesCount);
        }
    }

    public IEnumerator KillFeedTimer(int boxesCounts)
    {
        yield return new WaitForSeconds(4f);
        Debug.Log("p" + boxesCounts);
        Destroy(boxes[0]);
        boxes.RemoveAt(0);
        //count--;
        for (int i = 0; i < boxes.Count; i++)
        {
            boxes[i].transform.DOMoveY(975f - (i * 125), animationDuration);
        }

        count--;

    }

    /*public void Raise(int boxesCounts)
    {
        for (int i = 0; i < boxesCounts; i++)
        {
            boxes[i].transform.DOMoveY(700f, animationDuration);
        }
    }*/
}
