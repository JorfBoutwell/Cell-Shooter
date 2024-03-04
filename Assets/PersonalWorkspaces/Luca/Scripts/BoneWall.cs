using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoneWall : MonoBehaviour
{
    
    public float targetHeight = 4f;
    public float growthDuration = 0.5f;

    private Vector3 originalScale;
    private Vector3 targetScale;
    private bool shouldGrow = false;

    public GameObject wallPrefab;
    private Camera cam;
    private GameObject currentWall; 
    private float wallHalfHeight; 
    public float maxPlacementDistance = 100f;

    List<GameObject> walls = new List<GameObject>();
    int indexWallRemovable = 0;

    public void removeWall(GameObject wall) {
       // indexWallRemovable++;
        Destroy(wall);
        Debug.Log(walls[1]);
        walls.RemoveAt(0);
        Debug.Log(walls.Count);
    }

    public void StartGrowing(GameObject cube)
    {
        originalScale = new Vector3(cube.transform.localScale.x, 1f, cube.transform.localScale.z);
        targetScale = new Vector3(originalScale.x, targetHeight, originalScale.z);
        StartCoroutine(Grow(cube));
    }

    IEnumerator Grow(GameObject cube)
    {
        float timer = 0f;
        while (timer < growthDuration)
        {
            cube.transform.localScale = Vector3.Lerp(originalScale, targetScale, timer / growthDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        cube.transform.localScale = targetScale; 
        shouldGrow = false; 
    }

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

        ray = cam.ScreenPointToRay(Input.mousePosition);

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
                walls.Add(currentWall);
                StartGrowing(currentWall); 
                CreateNewWall();
                if (walls.Count > 2) removeWall(walls[0]);
            }
        }
        else
        {
            currentWall.GetComponent<Renderer>().enabled = false; 
        }
    }

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

}

