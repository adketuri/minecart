using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    float horizontalMove = 0f;
    float runSpeed = 40f;
    bool jump = false;
    bool facingRight = true;

    private Vector3 initialPos;
    public bool fallen = false;

    public LayerMask groundLayers; // the layers that should be hit by the ground check
    public float groundCheckDistance = 5f; // the length of the raycast
    public Animator animator;

    void Start()
    {
        initialPos = gameObject.transform.position;
    }

    private void OnEnable()
    {
        CharacterController2D.OnFlipped += OnFlipped;
    }

    private void OnDisable()
    {
        CharacterController2D.OnFlipped -= OnFlipped;
    }

    private void OnFlipped(bool facingRight)
    {
        this.facingRight = facingRight;
    }

    //Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);

        }
        animator.SetFloat("Angle", getGroundAngle());

        if (!fallen && GameObject.Find("Character").transform.position.y < -10)
        {
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsFalling", true);
            fallen = true;
        }
        if (fallen && gameObject.transform.position.y < -20)
        {
            animator.SetBool("IsFalling", false);
            fallen = false;
            gameObject.transform.position = initialPos;
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    // return a degree 0 to 90 for slope
    private float getGroundAngle()
    {
        float groundAngle = -1;

        // define a ray starting at this object's position
        // in this object's local downward direction
        Ray2D ray = new Ray2D(transform.position, -transform.up);

        // shoot that ray and get the results
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, groundCheckDistance, groundLayers);
        // something was hit
        if (hit.collider != null)
        {

            // get which direction the ground is facing
            Vector3 groundFacingDirection = hit.normal;

            // y axis (when ground is perfectly flat, the normal equals this)
            Vector3 verticalDirection = Vector3.up; // same as Vector3(0,1,0)

            // find the angle between the two directions
            // returns the acute angle (0 to 180)
            groundAngle = Vector3.SignedAngle(verticalDirection, groundFacingDirection, verticalDirection);

            // if the angle is greater than 90, make it go back towards 0 instead of up to 180
            if (groundAngle > 90)
            {
                //groundAngle = 90 - (groundAngle - 90);
            }

            // Let's flip the angle when we face LEFT
            if (!facingRight)
            {
                groundAngle *= -1;
            }

            //Debug.Log(facingRight + " " + groundAngle);
            
        }

        return groundAngle;
    }

    private void FixedUpdate()
    {
        // move our character
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }
}
