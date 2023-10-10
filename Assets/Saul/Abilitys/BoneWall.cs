using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneWall : MonoBehaviour
{
    public LayerMask flatFloor;
    float maxDistance = 100;
    public bool BoneWallStatus = false;
    public Renderer BoneRender;
    private Color colorOrig;
    private Color colorTrans;
    RaycastHit hit;
    public BoxCollider BoneCollider;

    // Start is called before the first frame update
    void Start()
    {
        BoneRender = GameObject.Find("BoneWall").GetComponent<Renderer>();
        //BoneCollider = GameObject.Find("BoneWall").GetComponent<BoxCollider>();
        colorOrig = BoneRender.material.color;
        colorTrans = new Color(colorOrig.r, colorOrig.g, colorOrig.b, 0.5f);
       // GameObject.Find("BoneWall").SetActive(false);
        //BoneCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Ray boneWallRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(boneWallRay, out hit, maxDistance, flatFloor))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                BoneWallStatus = true;
              //  GameObject.Find("BoneWall").SetActive(true);
                //BoneCollider.enabled = true;
            }
          
        }

        if (BoneWallStatus == true)
        {
            BoneRender.material.color = colorTrans;
            GameObject.Find("BoneWall").transform.position = hit.point;
            GameObject.Find("BoneWall").transform.rotation = GameObject.Find("Player").transform.rotation;
            


            if (Input.GetKeyUp(KeyCode.E))
            {

                BoneRender.material.color = colorOrig;
                BoneWallStatus = false;
            }
        }

    }
  

}
