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
    public float fastSpeed = 6.0f;

    public float slowSpeed = 3.0f;

    public float speed => isSlowed ? slowSpeed : fastSpeed;


    [Header("Model Settings")]
    [SerializeField]
    private Transform modelTransform;

    private Animator animator;

    public PlayerInputController InputController { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    private GroundedChecker GroundedChecker { get; set; }
    public Animator Animations => animator;

    public bool IsGrounded => GroundedChecker.IsGrounded;

    private bool isFacingRight;
    private bool isSlowed = false;
    private bool canMove = true;



    // Start is called before the first frame update
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        GroundedChecker = GetComponent<GroundedChecker>();
        animator = modelTransform.GetComponent<Animator>();
        InputController = GetComponent<PlayerInputController>();
        SetCurrentState(IdleState);
        Debug.Log(currentState.GetType());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!canMove)
            return;

        if (InputController.MoveDirection.x > 0 && !isFacingRight)
        {
            modelTransform.eulerAngles = new Vector3(0, 90, 0);
            isFacingRight = true;
        }
        else if (InputController.MoveDirection.x < 0 && isFacingRight)
        {
            modelTransform.eulerAngles = new Vector3(0, -90, 0);
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
            Rigidbody.velocity = Vector3.zero;
            Animations.SetFloat("Blend", Math.Abs(Rigidbody.velocity.x));
        }
    }
}
