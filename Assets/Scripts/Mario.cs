using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario : MonoBehaviour
{
    float speed = 0f;
    float maxSpeed;
    float maxWalkSpeed = 0.025f;
    float sprintSpeed = 0.01f;
    float maxSprintSpeed;
    float horizontalaAcc = 0.00025f;

    float jumpSpeedMin = 4f;
    float jumpSpeedMax = 9.5f;

    bool isJumping = false;
    bool jumpCancel = false;
    bool isGrounded = false;

    bool isSprinting = false;

    Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        maxSpeed = maxWalkSpeed;
        maxSprintSpeed = maxWalkSpeed * 1.6f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("left") && speed > -maxSpeed)
        {
            speed -= horizontalaAcc;
        }

        else if(speed < 0)
        {
            speed += horizontalaAcc;

            if (speed < 0.002 && speed > -0.002)
            {
                speed = 0;
            }
        }

        if (Input.GetKey("right") && speed < maxSpeed)
        {
            speed += horizontalaAcc;
        }

        else if(speed > 0)
        {
            speed -= horizontalaAcc;

            if (speed < 0.002 && speed > -0.002)
            {
                speed = 0;
            }
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!isSprinting)
            {
                if(speed > 0)
                {
                    speed = speed - sprintSpeed;
                }

                if(speed < 0)
                {
                    speed = speed + sprintSpeed;
                }
                maxSpeed = maxSprintSpeed;
                isSprinting = true;
            }
            
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            maxSpeed = maxWalkSpeed;
            isSprinting = false;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
        }

        if(Input.GetButtonUp("Jump") && !isGrounded)
        {
            jumpCancel = true;
        }

        if(speed > maxSpeed)
        {
            speed = maxSpeed;
        }

        transform.position = new Vector2(transform.position.x + speed, transform.position.y);
    }

    private void FixedUpdate()
    {
        if (isJumping)
        {
            body.velocity = new Vector2(body.velocity.x, jumpSpeedMax);
            isJumping = false;
        }

        if (jumpCancel)
        {
            if(body.velocity.y > jumpSpeedMin)
            {
                body.velocity = new Vector2(body.velocity.x, jumpSpeedMin);
            }
            jumpCancel = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
