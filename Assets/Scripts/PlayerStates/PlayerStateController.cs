using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    public bool CanMove => canMove;
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
    public bool isForceAdded = false;

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



    public bool movingToPoint = false;
    GameObject[] movementTargets;
    Vector3 movementTarget = Vector3.zero;
    bool faceRightAfterMovement = false;
    [HideInInspector] public bool standingOnConveyorBelt;



    private bool isFacingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        MovementController = GetComponent<MovementController>();
        animator = modelTransform.GetComponent<Animator>();
        InputController = GetComponentInParent<PlayerInputController>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        SetCurrentState(IdleState);
        movementTargets = GameObject.FindGameObjectsWithTag("MovementTarget");

        if(GameManager.instance.playCutscene && GameManager.instance.turnOnDialogue)
            SetCanMove(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

        var input = canMove ? InputController.MoveDirection : Vector3.zero;

        int wallDirX = (MovementController.collisions.leftWall) ? -1 : 1;

        float targetVelocityX = 0.0f;

        if (!isForceAdded)
        {
            targetVelocityX = input.x * moveSpeed;

        }
        else
        {
            targetVelocityX = (velocity.x * 0.95f) + (input.x * moveSpeed);

            Debug.Log("Target:" + targetVelocityX);


            if (Mathf.Sign(springVelocity.x) > 0f)
            {

                if (targetVelocityX > springVelocity.x + moveSpeed)
                    targetVelocityX = springVelocity.x + moveSpeed;
                else if (targetVelocityX < -moveSpeed)
                    targetVelocityX = -moveSpeed;
            }
            else if (Mathf.Sign(springVelocity.x) < 0f)
            {
                if (targetVelocityX < springVelocity.x - moveSpeed)
                    targetVelocityX = springVelocity.x - moveSpeed;
                else if (targetVelocityX > moveSpeed)
                    targetVelocityX = moveSpeed;
            }




            Debug.Log("Actual:" + targetVelocityX);

        }

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (MovementController.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        bool wallSliding = false;

        if ((MovementController.collisions.left || MovementController.collisions.right) && (!MovementController.collisions.below || MovementController.collisions.slopeAngle > MovementController.maxClimbAngle) && velocity.y < 0)
        {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if (input.x != wallDirX && input.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }

        }

        if (MovementController.collisions.above || (MovementController.collisions.below && !isForceAdded && MovementController.collisions.slopeAngle < MovementController.maxClimbAngle) || standingOnConveyorBelt)
        {
            velocity.y = 0;
        }

        if (coyoteTimer > 0f)
            coyoteTimer -= Time.fixedDeltaTime;

        if (MovementController.collisions.below && MovementController.collisions.slopeAngle < MovementController.maxClimbAngle || standingOnConveyorBelt)
        {
            coyoteTimer = 0.3f;

            if (velocity.y < 0f)
            {
                isForceAdded = false;
            }
        }

        if (InputController.IsJumping && canMove)
        {
            if ((MovementController.collisions.leftWall || MovementController.collisions.rightWall) && !MovementController.collisions.below && velocity.y < 0)
            {
                if (wallDirX * input.x > 0)
                {
                    Debug.Log("Wall Climb");
                    velocity.x = -wallDirX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                }
                else if (input.x == 0)
                {
                    Debug.Log("Wall Jump Off");
                    velocity.x = -wallDirX * wallJumpOff.x;
                    velocity.y = wallJumpOff.y;
                }
                else if (wallDirX * input.x < 0)
                {
                    Debug.Log("Wall Leap");
                    velocity.x = -wallDirX * wallLeap.x;
                    velocity.y = wallLeap.y;
                }
            }
            if (coyoteTimer > 0 && !isForceAdded)
            {
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
        
        if (velocity.y > 0)
        {
            Animations.SetBool(Constants.JUMPING, true);
        }

        // animations
        if (!MovementController.collisions.below && velocity.y < 0)
        {
            Animations.SetBool(Constants.FALLING, true);
        }

        if (!InputController.IsJumping)
        {
            Animations.SetBool(Constants.JUMPING, false);
        }

        if (MovementController.collisions.below)
        {
            Animations.SetBool(Constants.FALLING, false);
            Animations.SetBool("IsGrounded", true);
            if (!InputController.IsJumping)
            {
                Animations.SetFloat("Blend", Math.Abs(velocity.x));
            }
            else if(canMove) // Prevent jumping during cutscene dialogue
            {
                Animations.SetBool(Constants.JUMPING, true);
            }
        }
        else
        {
            Animations.SetBool("IsGrounded", false);
        }

        if (!canMove)
        {
            Animations.SetBool(Constants.JUMPING, false);
            Animations.SetBool(Constants.FALLING, false);
            Animations.SetFloat("Blend", Math.Abs(velocity.x));
        }

        if (movingToPoint)
        {
            float difference = movementTarget.x - transform.position.x;

            if (Mathf.Abs(difference) < 0.1)
            {
                movingToPoint = false;
                modelTransform.eulerAngles = faceRightAfterMovement ? new Vector3(0, 90, 0) : new Vector3(0, -90, 0);
                isFacingRight = faceRightAfterMovement;
            }
            else
            {
                if (Mathf.Abs(difference) > moveSpeed * Time.fixedDeltaTime)
                {
                    velocity.x = moveSpeed * Mathf.Sign(movementTarget.x - transform.position.x);
                    MovementController.Move(velocity * Time.fixedDeltaTime);
                }
                else
                {
                    velocity.x = difference;
                    MovementController.Move(velocity * Time.fixedDeltaTime);

                }


                if (movementTarget.x - transform.position.x < 0f)
                {
                    if (isFacingRight)
                    {
                        modelTransform.eulerAngles = new Vector3(0, -90, 0);
                        isFacingRight = false;
                    }
                }
                else
                {
                    if (!isFacingRight)
                    {
                        modelTransform.eulerAngles = new Vector3(0, 90, 0);
                        isFacingRight = true;
                    }
                }
            }

        }

        currentState.FixedUpdate(this);
    }

    public void Springboard(Vector3 springVelocity)
    {
        velocity = springVelocity;
        isForceAdded = true;
        Debug.Log("Springboard");
        SetCurrentState(JumpState);
        coyoteTimer = -1f;
        SpringVelocity = springVelocity;
    }

    public void MoveToPoint(Vector3 point, bool faceRightAfter)
    {
        movingToPoint = true;
        movementTarget = point;
        faceRightAfterMovement = faceRightAfter;
    }

    public void MoveToPoint(float relative, bool faceRightAfter)
    {
        movingToPoint = true;
        movementTarget = new Vector3(transform.position.x + relative, transform.position.y, transform.position.z);
        faceRightAfterMovement = faceRightAfter;

    }

    public void MoveToPoint(String point, bool faceRightAfter)
    {

        foreach (GameObject target in movementTargets)
        {
            if (target.name == point)
            {
                movementTarget = target.transform.position;
                movingToPoint = true;
                faceRightAfterMovement = faceRightAfter;


                return;
            }
        }

        Debug.LogError("Failed to move point - point was not found");
    }

    public void FaceOtherPlayerWhileTalking()
    {
        Debug.Log($"transform.name is {transform.name}" );
        var otherPlayerName = transform.name == "Onwell" ? "Rani" : "Onwell";
        var otherPlayer = GameObject.Find(otherPlayerName).transform;
        //if (1 <= x && x <= 100)   //true
        var diff = Mathf.Abs(transform.position.x - otherPlayer.position.x);
        if (transform.position.x > otherPlayer.position.x)
        {
            // face left
            modelTransform.eulerAngles = new Vector3(0, -90, 0);
            isFacingRight = false;
            if (diff < 1.5)
            {
                transform.GetComponent<MovementController>().Move(Vector3.right);
                otherPlayer.GetComponent<MovementController>().Move(Vector3.left);
            }
        }
        if (transform.position.x < otherPlayer.position.x)
        {
            // face right
            modelTransform.eulerAngles = new Vector3(0, 90, 0);
            isFacingRight = true;
            if (diff < 1.5)
            {
                transform.GetComponent<MovementController>().Move(Vector3.left);
                otherPlayer.GetComponent<MovementController>().Move(Vector3.right);
            }
        }
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
