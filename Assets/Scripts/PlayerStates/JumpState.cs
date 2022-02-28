using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : PlayerState
{
    [SerializeField]
    private float jumpHoldTimer = 0.3f;

    [SerializeField]
    private float initialVelocity = 20.0f;
    
    [SerializeField]
    private float upVelocity = 10.0f;

    private float jumpTimer;
    
    public override void EnterState(PlayerStateController player)
    {
        // Initial velocity on enter jump
        // TODO add property for velocity
        player.Rigidbody.velocity = player.Rigidbody.velocity.WithY(player.Rigidbody.velocity.y + initialVelocity);
    }

    public override void FixedUpdate(PlayerStateController player)
    {
        if (player.InputController.IsJumping && jumpTimer < jumpHoldTimer) // jumped and is holding
        {
            jumpTimer += Time.deltaTime;
            player.Rigidbody.velocity = player.Rigidbody.velocity.WithY(player.Rigidbody.velocity.y + upVelocity);
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
        throw new System.NotImplementedException();
    }

    public override void ExitState(PlayerStateController player)
    {
        throw new System.NotImplementedException();
    }
}
