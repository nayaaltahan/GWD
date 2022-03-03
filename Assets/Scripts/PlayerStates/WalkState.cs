using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : PlayerState
{
    [Tooltip("Walking speed of the player.")]
    [SerializeField]
    private float speed = 20f;
    
    public override void EnterState(PlayerStateController player)
    {
        
    }

    public override void FixedUpdate(PlayerStateController player)
    {
        //TODO change movedirection to property
        Vector3 velocity = player.InputController.MoveDirection * speed;
        velocity.y = player.Rigidbody.velocity.y;
        player.Rigidbody.velocity = velocity;

        if (player.InputController.IsJumping)
        {
            player.SetCurrentState(new JumpState());
        }

        else if(player.InputController.MoveDirection == Vector3.zero)
        {
            player.SetCurrentState(new IdleState());
        }

        else if (player.Rigidbody.velocity.y < 0)
        {
            player.SetCurrentState(new FallState());
        }
    }

    public override void OnCollisionEnter(PlayerStateController player, Collision collision)
    {
        // TODO check if collision is pushable/draggable
    }
    
    //TODO add onTriggerEnter base function for implementing pushable/draggable

}
