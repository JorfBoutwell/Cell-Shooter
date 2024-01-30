using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoneWall : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask groundLayer;

    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            transform.position = hit.point;

            transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }
    }
}

