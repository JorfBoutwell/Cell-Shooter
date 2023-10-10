using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            Debug.Log("hi :)" + boxesCount);
            yPos = 125 * count;
            count++;
            boxes.Add(Instantiate(killFeedBox, new Vector3(1700f, 1100f - yPos, 0f), transform.rotation) as GameObject);
            boxes[boxesCount].transform.SetParent(canvas1.transform);
            StartCoroutine("KillFeedTimer", boxesCount);
            
            
        }
    }

    public IEnumerator KillFeedTimer(int boxesCounts)
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(boxes[boxesCounts]);
        count--;
    }
}
