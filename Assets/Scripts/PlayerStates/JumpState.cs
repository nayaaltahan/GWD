using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : PlayerState
{
    private float jumpTimer;
    
    public override void EnterState(PlayerStateController player)
    {
        // Initial velocity on enter jump
        // TODO add property for velocity
        player.Rigidbody.velocity = player.Rigidbody.velocity.WithY(player.Rigidbody.velocity.y + player.initialVelocity);
        jumpTimer = 0;
    }

    public override void FixedUpdate(PlayerStateController player)
    {
        if (player.InputController.IsJumping && jumpTimer < player.jumpHoldTimer) // jumped and is holding
        {
            jumpTimer += Time.deltaTime;
            player.Rigidbody.velocity = (player.InputController.MoveDirection * player.speed).WithY(player.Rigidbody.velocity.y);

        }
        else if (player.Rigidbody.velocity.y < 0) // falling
        {
            // enter falling state
            player.SetCurrentState(new FallState());
        }
        else if (player.IsGrounded) // landed
        {
            if (player.InputController.MoveDirection == Vector3.zero)
            {
                player.SetCurrentState(new IdleState());
            }
            else
            {
                player.SetCurrentState(new WalkState());
            }
        }
        else if (!player.IsGrounded) // released jump but still jumping
        {
            jumpTimer = 1;
            player.Rigidbody.velocity = player.Rigidbody.velocity.WithY(0);
        }    
    }

    public override void OnCollisionEnter(PlayerStateController player, Collision collision)
    {
        
    }
    
}
