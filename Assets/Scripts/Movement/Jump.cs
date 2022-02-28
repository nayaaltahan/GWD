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
    private float upVelocity = 10.0f;

    [SerializeField]
    private float downVelocity = 10.0f;
    
    [SerializeField]
    private float maxDownVelocity = 50.0f;
    
    

    private float yVelocity;
    
    private Rigidbody rb;
    private GroundedChecker groundedChecker;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        groundedChecker = GetComponent<GroundedChecker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (groundedChecker.IsGrounded) // probably don't need this when we have states
        {
            
        }
    }

    private void FixedUpdate()
    {
        if (yVelocity < 0)
        {
            yVelocity = Mathf.Clamp(yVelocity - downVelocity, 0, maxDownVelocity);
                        
        }
        
    }

    void DoJump()
    {
        rb.velocity = rb.velocity.WithY(yVelocity);
    }
}
