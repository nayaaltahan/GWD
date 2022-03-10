using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : PlayerState
{
    public override void EnterState(PlayerStateController player)
    {
        
    }

    public override void FixedUpdate(PlayerStateController player)
    {
        if (!player.IsGrounded) // falling
        {
            var clamped = Mathf.Clamp(player.Rigidbody.velocity.y + player.downVelocity, player.maxDownVelocity, 100);
            player.Rigidbody.velocity = (player.InputController.MoveDirection * (player.speed )).WithY(clamped);
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
    }

    public override void OnCollisionEnter(PlayerStateController player, Collision collision)
    {
        
    }
    
}
