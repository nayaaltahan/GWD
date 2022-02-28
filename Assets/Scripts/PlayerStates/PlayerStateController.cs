using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    private PlayerState currentState;
    
    public readonly IdleState IdleState = new IdleState();
    public readonly JumpState JumpState = new JumpState();
    public readonly WalkState WalkState = new WalkState();
    public readonly FallState FallState = new FallState();
    
    public PlayerInputController InputController { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    private GroundedChecker GroundedChecker { get; set; }
    public bool IsGrounded => GroundedChecker.IsGrounded;
    
    private bool isFacingRight;


    // Start is called before the first frame update
    void Start()
    {
        InputController = GetComponent<PlayerInputController>();
        Rigidbody = GetComponent<Rigidbody>();
        GroundedChecker = GetComponent<GroundedChecker>();
        SetCurrentState(IdleState);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (InputController.MoveDirection.x > 0 && !isFacingRight)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            isFacingRight = true;
        }
        else if (InputController.MoveDirection.x < 0 && isFacingRight)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            isFacingRight = false;
        }
        currentState.FixedUpdate(this);
    }

    private void OnCollisionEnter(Collision other)
    {
        currentState.OnCollisionEnter(this, other);
    }

    public void SetCurrentState(PlayerState state)
    {
        currentState.ExitState(this);
        currentState = state;
        currentState.EnterState(this);
    }
}
