using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAbility : MonoBehaviour
{

    void print(string str) { Debug.Log(str); }
    
    enum Direction { ASCENDING, DESCENDING, STATIC };
    Direction direction;

    public float velocity;
    public float speed;

    Rigidbody rb;
    Vector3 startPos;
    float v;

    void Start()
    {
        velocity = 0;
        direction = Direction.STATIC;
        rb = GetComponent<Rigidbody>();
        
        speed = 0.7f;
        v = speed / 10;
    }

    void Update()
    {
        rb.AddForce(new Vector3(0,2,0) * velocity, ForceMode.Impulse);
        Debug.Log(direction + " " + transform.position.y);    

        if (Input.GetMouseButtonDown(0) && velocity == 0) {
            direction = Direction.ASCENDING;
            startPos = transform.position;    
        }
        if (direction == Direction.ASCENDING) { 
            v += speed / 10;
            velocity += speed - v;      

            if (speed - v == 0)
                direction = Direction.DESCENDING;
        }
        if (direction == Direction.DESCENDING) velocity -= speed*2;
    }

    void OnCollisionEnter(Collision other)
    {
        GameObject obj = other.gameObject;
        if (obj.tag == "ground") {
            velocity = 0;
            direction = Direction.STATIC;
            print("hit ground");
        }
    }
}
