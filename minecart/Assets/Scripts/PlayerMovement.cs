using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    float horizontalMove = 0f;
    float runSpeed = 60f;
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
        animator.SetFloat("Speed", Math.Abs(horizontalMove));
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
            Rigidbody2D rb2d = gameObject.GetComponent<Rigidbody2D>();
            rb2d.velocity = Vector2.zero;
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    // return a degree 0 to 90 for slope
    private float getGroundAngle()
    {
        // define a ray starting at this object's position
        // in this object's local downward direction
        Ray2D ray = new Ray2D(transform.position, -transform.up);

        // shoot that ray and get the results
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, groundCheckDistance, groundLayers);
        // something was hit
        float angle = 0;
        if (hit.collider != null)
        {
            // get the angle from the normal (perpendicular to slope) and the up direction
            angle = Vector2.SignedAngle(hit.normal, Vector2.up) * (facingRight ? -1 : 1);
            
        }
        return angle;
    }

    private void FixedUpdate()
    {
        // move our character
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }
}
