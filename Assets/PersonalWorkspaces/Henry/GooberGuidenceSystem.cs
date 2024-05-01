using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooberGuidenceSystem : MonoBehaviour
{
    public GameObject goober;
    public Transform playerCamera;

    private void Start()
    {
        if(!GameObject.FindGameObjectWithTag("Goober"))
        {
            gameObject.SetActive(false);
        } else
        {
            goober = GameObject.FindGameObjectWithTag("Goober");
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(gameObject.activeSelf /*&& goober.GetComponent<GooberFunctionality>().currentPlayer == null*/)
        {
            // Calculate the direction from the player's camera to the goober
            Vector3 directionToObjective = (goober.transform.position - playerCamera.position).normalized;

            // Project the direction onto the horizontal plane (ignoring vertical)
            Vector3 directionOnHorizontalPlane = Vector3.ProjectOnPlane(directionToObjective, Vector3.up).normalized;

            // Calculate the angle between the forward direction of the player's view and the direction to the objective
            float angleToObjective = Vector3.SignedAngle(playerCamera.forward, directionOnHorizontalPlane, Vector3.up);

            // Rotate the arrow UI to point towards the objective
            transform.rotation = Quaternion.Euler(0f, 0f, -angleToObjective);
        }

    }
}
