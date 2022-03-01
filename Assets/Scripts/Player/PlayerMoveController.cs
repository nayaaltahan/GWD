using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveController : MonoBehaviour
{
    private Rigidbody rb;

    [Tooltip("Walking speed of the player.")]
    [SerializeField]
    private float speed = 20f;
    private Vector3 moveDirection;
    private bool isFacingRight;

    
    [HideInInspector]
    public bool canTurnLeft { get; set; }   //Variable is used when we want to make sure the player can only walk forward

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    public void SetMove(InputAction.CallbackContext value)
    {
        Vector2 tempVector = value.ReadValue<Vector2>();
        moveDirection = new Vector3(tempVector.x, 0, 0);

        if (canTurnLeft == false && moveDirection.x < 0)
            moveDirection.x = 0;
    }

    public void Move()
    {
        Vector3 velocity = moveDirection * speed;

        velocity.y = rb.velocity.y;
        rb.velocity = velocity;

        isFacingRight = false;
        if (moveDirection.x < 0)
            isFacingRight = true;

        if (moveDirection.x > 0 && !isFacingRight)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            isFacingRight = true;
        }
        else if (moveDirection.x < 0 && isFacingRight)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            isFacingRight = false;
        }
    }
}
