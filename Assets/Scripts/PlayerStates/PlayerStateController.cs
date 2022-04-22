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
    public float maxJumpHeight = 5;
    public float minJumpHeight = 3;
    public float timeToJumpApex = .4f;
    public float coyoteTime = 0.3f;
    float coyoteTimer = 0f;
    
    [Header("Turn Around Settings")]
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;

    [Header("Wall Jump")] 
    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    float timeToWallUnstick;

    [Header("Walk Settings")]
    [Tooltip("Walking speed of the player.")]
    public float fastSpeed = 6.0f;

    public float slowSpeed = 3.0f;
    private bool isForceAdded = false;

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

    private CapsuleCollider capsuleCollider;

    public Vector3 SpringVelocity { get => springVelocity; set => springVelocity = value; }

    
    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    [HideInInspector]
    public Vector3 velocity;
    float velocityXSmoothing;
    
    private bool isSlowed = false;
    private bool canMove = true;
    private Vector3 springVelocity = Vector3.zero;

    private bool isFacingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        MovementController = GetComponent<MovementController>();
        animator = modelTransform.GetComponent<Animator>();
        InputController = GetComponent<PlayerInputController>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        SetCurrentState(IdleState);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

        var input = canMove ? InputController.MoveDirection : Vector3.zero;
        
        int wallDirX = (MovementController.collisions.left) ? -1 : 1;

        float targetVelocityX = 0.0f;

        if (!isForceAdded)
            targetVelocityX = input.x * moveSpeed;
        else
            targetVelocityX = springVelocity.x + (input.x * moveSpeed);
        
        velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, 
            (MovementController.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);

        bool wallSliding = false;
        if ((MovementController.collisions.left || MovementController.collisions.right) && !MovementController.collisions.below && velocity.y < 0) {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax) {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0) {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if (input.x != wallDirX && input.x != 0) {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else {
                timeToWallUnstick = wallStickTime;
            }

        }
        
        if (MovementController.collisions.above || MovementController.collisions.below)
        {
            velocity.y = 0;
        }

        if (coyoteTimer > 0f)
            coyoteTimer -= Time.fixedDeltaTime;

        if (MovementController.collisions.below)
        {
            coyoteTimer = 0.3f;
            isForceAdded = false;
        }

        if (InputController.IsJumping && canMove)
        {
            if ((MovementController.collisions.leftWall || MovementController.collisions.rightWall) && !MovementController.collisions.below && velocity.y < 0) {
                if (wallDirX == input.x) {
                    velocity.x = -wallDirX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                }
                else if (input.x == 0) {
                    velocity.x = -wallDirX * wallJumpOff.x;
                    velocity.y = wallJumpOff.y;
                }
                else {
                    velocity.x = -wallDirX * wallLeap.x;
                    velocity.y = wallLeap.y;
                }
            }
            if (coyoteTimer > 0) {
                velocity.y = maxJumpVelocity;
                coyoteTimer = -1f;

            }
        }

        if (InputController.ReleasedJump && !isForceAdded)
        {
            if (velocity.y > minJumpVelocity)
            {
                velocity.y = minJumpVelocity;
            }
        }

        if (InputController.MoveDirection.x > 0 && !isFacingRight && canMove)
        {
            modelTransform.eulerAngles = new Vector3(0, 90, 0);
            isFacingRight = true;
        }
        else if (InputController.MoveDirection.x < 0 && isFacingRight && canMove)
        {
            modelTransform.eulerAngles = new Vector3(0, -90, 0);
            isFacingRight = false;
        }
        
        velocity.y += gravity * Time.fixedDeltaTime;
        MovementController.Move(velocity * Time.fixedDeltaTime);
        
        if (velocity.x != 0 && (MovementController.collisions.above || MovementController.collisions.below))
        {
            SetCurrentState(WalkState);
        }
        else if (velocity.y < 0)
        {
            SetCurrentState(FallState);
        }
        else if (velocity.y > 0)
        {
            SetCurrentState(JumpState);

        }
        else if (velocity.y == 0 && velocity.x == 0)
        {
            SetCurrentState(IdleState);
        }
        
        // animations
        if (!MovementController.collisions.below)
        {
            Animations.SetBool(Constants.FALLING, true);
        }
        
        if (MovementController.collisions.below)
        {
            //Debug.Log("Collisions below, finish falling/jumping animations");
            //Animations.SetLayerWeight(1,1);
            Animations.SetBool(Constants.FALLING, false);
            Animations.SetBool(Constants.JUMPING, false);
            //Animations.SetLayerWeight(1,0);

        }
        currentState.FixedUpdate(this);
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.transform.CompareTag(Constants.PUSHABLE))
    //    {
    //        if (InputController.MoveDirection.x != 0)
    //        {
    //            Animations.SetBool(Constants.PUSHING, true);

    //        }
    //        else
    //        {
    //            Animations.SetBool(Constants.PUSHING, false);

    //        }
    //        var force = InputController.MoveDirection * 1000;
    //        Debug.Log($"Pushing object {other.gameObject.name} with force {force}");
    //        other.GetComponentInParent<Rigidbody>()?.AddForce(force);
            
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.transform.CompareTag(Constants.PUSHABLE))
    //    {
    //        Animations.SetBool(Constants.PUSHING, false);
    //    }
    //}

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
