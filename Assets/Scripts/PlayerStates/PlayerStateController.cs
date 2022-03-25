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
    public float jumpHoldTimer = 0.3f;

    public float initialVelocity = 15.0f;

    public float upVelocity = 0.2f;

    [Header("Fall Settings")]
    public float downVelocity = -0.2f;

    public float maxDownVelocity = -100.0f;

    [Header("Walk Settings")]
    [Tooltip("Walking speed of the player.")]
    public float moveSpeed = 6;
    public float gravity = -20;
    public Vector3 velocity = new Vector3(3,0,0);

    [Header("Components")]
    [SerializeField]
    private Animator animator;

    [Header("Model Settings")]
    [SerializeField]
    private Transform modelTransform;


    public PlayerInputController InputController { get; private set; }
    private MovementController MovementController { get; set; }
    public Animator Animations => animator;


    // Start is called before the first frame update
    void Start()
    {
        MovementController = GetComponent<MovementController>();
        InputController = GetComponent<PlayerInputController>();
        SetCurrentState(IdleState);
        Debug.Log(currentState.GetType());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (MovementController.collisions.above || MovementController.collisions.below) {
            velocity.y = 0;
        }

        var input = InputController.MoveDirection;

        velocity.x = input.x * moveSpeed * Time.fixedDeltaTime;
        velocity.y += gravity * Time.fixedDeltaTime;
        MovementController.Move (velocity);
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
