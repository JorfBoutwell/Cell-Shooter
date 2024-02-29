using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsADisplayScript : MonoBehaviour
{
    public int points;
    public GameObject pointObject;

    // Update is called once per frame
    void Update()
    {
        pointObject.GetComponentInChildren<TextMeshProUGUI>().text = Mathf.FloorToInt(points).ToString("0");
    }
}
