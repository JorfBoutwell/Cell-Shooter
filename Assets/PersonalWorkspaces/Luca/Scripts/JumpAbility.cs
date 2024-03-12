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
    bool moving;

    void Start()
    {
        velocity = 0;
        direction = Direction.STATIC;
        rb = GetComponent<Rigidbody>();
        moving = false;

        speed = 0.01f;
        v = speed / 100;
    }

    void Update()
    {
        //transform.Translate(new Vector3(0, 1, 0) * velocity);
        rb.AddForce(new Vector3(0,2,0) * velocity, ForceMode.Impulse);
        Debug.Log(direction + " " + transform.position.y + " " + velocity + " " + moving);    

        if (Input.GetMouseButtonDown(0) && velocity == 0) {
            direction = Direction.ASCENDING;
            startPos = transform.position;
        }
        if (direction == Direction.ASCENDING) {
            v += speed / 100;
            velocity += speed - v;

            if (speed - v <= 0 && transform.position.y != startPos.y)
                direction = Direction.DESCENDING;
        }
        if (direction == Direction.DESCENDING) velocity -= speed*2;
        if (transform.position.y < startPos.y)
        {
            print("stopped");
            direction = Direction.STATIC;
            speed = .01f;
            v = speed / 100;
            transform.position = new Vector3(transform.position.x, startPos.y, transform.position.z);
            velocity = 0;
        }
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
