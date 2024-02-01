using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 12f;
    public float ascentSpeed = 8f; 

    private Camera cam;
    private Vector3 velocity;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * x + transform.forward * z;
        moveDirection.Normalize(); 

        transform.position += moveDirection * speed * Time.deltaTime;

        // Ascend and Descend
        if (Input.GetKey(KeyCode.Space)) // Ascend
        {
            velocity.y = ascentSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftControl)) // Descend
        {
            velocity.y = -ascentSpeed;
        }
        else
        {
            velocity.y = 0;
        }

        // Apply vertical movement
        transform.position += velocity * Time.deltaTime;
    }
}
