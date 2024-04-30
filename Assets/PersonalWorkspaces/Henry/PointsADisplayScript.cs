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

    private void Awake()
    {
        pointContainer = GameObject.Find(pointContainerName).GetComponent<PointContainer>();
    }

    // Update is called once per frame
    void Update()
    {
        points = pointContainer.points;
        pointObject.GetComponentInChildren<TextMeshProUGUI>().text = Mathf.FloorToInt(points).ToString("0");
    }
}
