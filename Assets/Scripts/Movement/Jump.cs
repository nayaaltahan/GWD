using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(GroundedChecker), typeof(Rigidbody))]
public class Jump : MonoBehaviour
{
    [SerializeField]
    private float jumpHoldTimer = 0.3f;

    [SerializeField]
    private float initialVelocity = 20.0f;
    
    [SerializeField]
    private float upVelocity = 10.0f;

    [SerializeField]
    private float downVelocity = -10.0f;
    
    [SerializeField]
    private float maxDownVelocity = 50.0f;

    private float jumpTimer;
    private bool gotInitialBoost;
    
    private Rigidbody rb;
    private GroundedChecker groundedChecker;

    private PlayerInputController inputController;
    
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        groundedChecker = GetComponent<GroundedChecker>();
        inputController = GetComponent<PlayerInputController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (inputController.IsJumping && jumpTimer < jumpHoldTimer) // jumped and is holding
        {
            if (!gotInitialBoost)
            {
                rb.velocity = rb.velocity.WithY(rb.velocity.y + initialVelocity);
                gotInitialBoost = true;
            }
            jumpTimer += Time.deltaTime;
            rb.velocity = rb.velocity.WithY(rb.velocity.y + upVelocity);
        }
        else if (!inputController.IsJumping && groundedChecker.IsGrounded) // landed
        {
            jumpTimer = 0;
            rb.velocity = rb.velocity.WithY(0);
            gotInitialBoost = false;
        }
        else if (!groundedChecker.IsGrounded && rb.velocity.y < 0) // falling
        {
            var clamped = Mathf.Clamp(rb.velocity.y + downVelocity, maxDownVelocity, 100);
            rb.velocity = rb.velocity.WithY(clamped);
        }
        else if (!groundedChecker.IsGrounded) // released jump but still jumping
        {
            jumpTimer = 1;
            rb.velocity = rb.velocity.WithY(0);
        }
    }

    void EnterJump()
    {
        rb.velocity = rb.velocity.WithY(initialVelocity);
    }
    

}
