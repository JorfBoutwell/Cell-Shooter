using System.Collections.Generic;
using UnityEngine;

public class WallPlacement : MonoBehaviour
{
    public GameObject wallPrefab;
    private Camera cam;
    private GameObject currentWall; 
    private float wallHalfHeight; 
    public LayerMask groundLayer;
    public float maxPlacementDistance = 100f;

    List<GameObject> walls = new List<GameObject>();

    void Start()
    {
        cam = Camera.main;
        CreateNewWall();
    }

    void CreateNewWall()
    {
        currentWall = Instantiate(wallPrefab, Vector3.zero, Quaternion.identity);
        wallHalfHeight = currentWall.transform.localScale.y / 2;
        currentWall.GetComponent<Renderer>().enabled = false;
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxPlacementDistance, groundLayer))
        {
            Vector3 placementPosition = hit.point;
            placementPosition.y += wallHalfHeight;

            currentWall.transform.position = placementPosition;

            Vector3 directionToCamera = (cam.transform.position - currentWall.transform.position).normalized;
            directionToCamera.y = 0; 
            currentWall.transform.right = directionToCamera;

            currentWall.GetComponent<Renderer>().enabled = true;

            if (Input.GetMouseButtonDown(0))
            {
                currentWall.GetComponent<CubeGrow>().StartGrowing(); 
                CreateNewWall();
            }
        }
        else
        {
            currentWall.GetComponent<Renderer>().enabled = false; 
        }
    }
}
