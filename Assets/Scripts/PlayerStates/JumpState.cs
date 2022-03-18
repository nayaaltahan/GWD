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
        player.Animations.SetTrigger("Jumping");
        player.Rigidbody.velocity = player.Rigidbody.velocity.WithY(player.Rigidbody.velocity.y + player.initialVelocity);
        jumpTimer = 0;
    }

    public override void FixedUpdate(PlayerStateController player)
    {
        base.FixedUpdate(player);

        if (player.InputController.IsJumping && jumpTimer < player.jumpHoldTimer) // jumped and is holding
        {
            jumpTimer += Time.deltaTime;
            float temp = player.Rigidbody.velocity.x;
            player.Rigidbody.velocity = (player.InputController.MoveDirection * player.speed).WithY(player.Rigidbody.velocity.y + player.upVelocity + player.downVelocity);

        }
        else if ((player.Rigidbody.velocity.y < 0) || (player.InputController.IsJumping && jumpTimer < player.jumpHoldTimer)) // falling
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
        
        //forgiveness mechanics
        // maybe rework the jump? 
        // Climbing
        // Sliding 
        // jumping would be the opposite way
        // grabbing
        // pushing works physics
        // two lanes
        // moving platforms? moving platforms should move players
        // shaky platform should not shake players
        // weighted platforms - consider spring in unity 
        
        // TODO do we need this? can we add breakpoints and see when we reach this condition?
        else if (player.Rigidbody.velocity.y >= 0) // released jump but still jumping
        {
            jumpTimer = 1;
            //player.Rigidbody.velocity = (player.InputController.MoveDirection * (player.speed * 0.5f)).WithY(player.Rigidbody.velocity.y * 0.8f);
        }

    }

    public override void OnCollisionEnter(PlayerStateController player, Collision collision)
    {

    }

}
