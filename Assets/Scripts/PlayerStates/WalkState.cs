using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : PlayerState
{
    public override void EnterState(PlayerStateController player)
    {
        
    }

    public override void FixedUpdate(PlayerStateController player)
    {
        //TODO change movedirection to property
        player.Rigidbody.velocity = (player.InputController.MoveDirection * player.speed).WithY(player.Rigidbody.velocity.y + player.downVelocity);

        if (player.InputController.IsJumping && player.IsGrounded)
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
