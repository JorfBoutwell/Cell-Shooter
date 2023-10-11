using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBloodCell : MonoBehaviour
{
    public bool Grounded = true;
    float xDirection;
    float zDirection;
    float xVector;
    float zVector;
    public float speed;
    Rigidbody rb;
    public float jumpForce;
    public int jumpCount = 2;
    bool isGrounded;

    //Dashing Variables
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown;

    //public float coolDownTime = 5;
    //private float nextFireTime = 0;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }

        
        xDirection = Input.GetAxis("Horizontal");
        zDirection = Input.GetAxis("Vertical");

        xVector = speed * xDirection * Time.deltaTime;
        zVector = speed * zDirection * Time.deltaTime;

        transform.position = transform.position + new Vector3(xVector, 0, zVector);

        if(jumpCount <= 2 || isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0, jumpForce));
            Grounded = false;
            jumpCount += 1;
            
        }

        //TO DO
        //Implement cooldown time
        //Work on Double Jump

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
