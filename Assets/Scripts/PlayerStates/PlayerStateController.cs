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

    [Header("Jump Settings")]
    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    
    [Header("Turn Around Settings")]
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;

    [Header("Fall Settings")]
    public float downVelocity = -0.2f;

    public float maxDownVelocity = -100.0f;

    [Header("Walk Settings")]
    [Tooltip("Walking speed of the player.")]
    public float moveSpeed = 6;

    [Header("Components")]
    [SerializeField]
    private Animator animator;

    [Header("Model Settings")]
    [SerializeField]
    private Transform modelTransform;


    public PlayerInputController InputController { get; private set; }
    private MovementController MovementController { get; set; }
    public Animator Animations => animator;
    
    float gravity;
    float jumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;


    // Start is called before the first frame update
    void Start()
    {
        MovementController = GetComponent<MovementController>();
        InputController = GetComponent<PlayerInputController>();
        SetCurrentState(IdleState);
        
        gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        print ("Gravity: " + gravity + "  Jump Velocity: " + jumpVelocity);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (MovementController.collisions.above || MovementController.collisions.below) {
            velocity.y = 0;
        }

        if (InputController.IsJumping && MovementController.collisions.below)
        {
            velocity.y = jumpVelocity;
        }
        
        var input = InputController.MoveDirection;
        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, 
            (MovementController.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
        velocity.y += gravity * Time.fixedDeltaTime;
        MovementController.Move (velocity * Time.fixedDeltaTime);
        currentState.FixedUpdate(this);
    }

    private void OnCollisionEnter(Collision other)
    {
        currentState.OnCollisionEnter(this, other);
    }

    public void SetCurrentState(PlayerState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
}
