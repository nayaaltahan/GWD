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

    [Header("Walk Settings")]
    [Tooltip("Walking speed of the player.")]
    public float fastSpeed = 6.0f;

    public float slowSpeed = 3.0f;

    public float moveSpeed => isSlowed ? slowSpeed : fastSpeed;

    [Header("Components")]
    [SerializeField]
    private Animator animator;

    [Header("Model Settings")]
    [SerializeField]
    private Transform modelTransform;
    
    public PlayerInputController InputController { get; private set; }
    private MovementController MovementController { get; set; }
    public Animator Animations => animator;

    public Vector3 SpringVelocity { get => springVelocity; set => springVelocity = value; }

    float gravity;
    float jumpVelocity;
    private Vector3 velocity;
    float velocityXSmoothing;
    
    private bool isSlowed = false;
    private bool canMove = true;
    private Vector3 springVelocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        MovementController = GetComponent<MovementController>();
        animator = modelTransform.GetComponent<Animator>();
        InputController = GetComponent<PlayerInputController>();
        SetCurrentState(IdleState);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        if (MovementController.collisions.above || MovementController.collisions.below)
        {
            velocity.y = 0;
        }

        if (!canMove)
            return;

        if (InputController.IsJumping && MovementController.collisions.below)
        {
            velocity.y = jumpVelocity;
        }

        if(springVelocity != Vector3.zero)
        {
            velocity = springVelocity;
            springVelocity = Vector3.zero;
        }
        
        var input = InputController.MoveDirection;
        if (input.x * transform.localScale.x < 0)
        {
            transform.localScale = transform.localScale.WithX(-1*transform.localScale.x);
            
        }
        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, 
            (MovementController.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
        velocity.y += gravity * Time.fixedDeltaTime;
        MovementController.Move (velocity * Time.fixedDeltaTime);
        currentState.FixedUpdate(this);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Pushable"))
        {
            var force = InputController.MoveDirection * 1000;
            Debug.Log($"Pushing object {other.gameObject.name} with force {force}");
            other.GetComponentInParent<Rigidbody>()?.AddForce(force);
        }
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

    public void SetIsWalkingSlow(bool val)
    {
        isSlowed = val;
    }

    public void SetCanMove(bool val)
    {
        canMove = val;
        if (!canMove)
        {
            SetCurrentState(IdleState);
        }
    }
}
