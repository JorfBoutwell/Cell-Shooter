using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsADisplayScript : MonoBehaviour
{
    public int points;
    public GameObject pointObject;
    public PointContainer pointContainer;
    public string pointContainerName;

    private void Start()
    {
        GameObject go = GameObject.Find(pointContainerName);
        Debug.Log(go.GetComponent<PointContainer>());
        pointContainer = go.GetComponent<PointContainer>();
        Debug.Log(pointContainer);
    }

    // Update is called once per frame
    void Update()
    {
        points = pointContainer.points;
        pointObject.GetComponentInChildren<TextMeshProUGUI>().text = Mathf.FloorToInt(points).ToString("0");
    }
}
