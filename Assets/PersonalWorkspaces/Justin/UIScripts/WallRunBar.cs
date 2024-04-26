using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WallRunBar : MonoBehaviour
{
    public PlayerControllerNEW playerControllerNEW;

    public GameObject wallRunBar;
    public GameObject wallRunBarBackground;

    float wallRunLength;
    float lastWallRunTime;

    private void Start()
    {
        lastWallRunTime = playerControllerNEW.m_wallRunTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (lastWallRunTime != playerControllerNEW.m_wallRunTimer)
        {
            wallRunLength = playerControllerNEW.m_wallRunTimer * 65;
            wallRunBar.GetComponent<RectTransform>().sizeDelta = new Vector2(wallRunLength, 100);
        }
        else if (playerControllerNEW.isGrounded)
        {
            if (wallRunLength < 100) { 
            wallRunLength += 1;
            }
            wallRunBar.GetComponent<RectTransform>().sizeDelta = new Vector2(wallRunLength, 100);
        }

        lastWallRunTime = playerControllerNEW.m_wallRunTimer;
    }
}
