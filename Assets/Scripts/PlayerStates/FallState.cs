using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : PlayerState
{
    [SerializeField]
    private float downVelocity = -10.0f;
    
    [SerializeField]
    private float maxDownVelocity = 50.0f;
    
    public override void EnterState(PlayerStateController player)
    {
        
    }

    public override void FixedUpdate(PlayerStateController player)
    {
        if (!player.IsGrounded && player.Rigidbody.velocity.y < 0) // falling
        {
            var clamped = Mathf.Clamp(player.Rigidbody.velocity.y + downVelocity, maxDownVelocity, 100);
            player.Rigidbody.velocity = player.Rigidbody.velocity.WithY(clamped);
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
        throw new System.NotImplementedException();
    }

    public override void ExitState(PlayerStateController player)
    {
        throw new System.NotImplementedException();
    }
}
