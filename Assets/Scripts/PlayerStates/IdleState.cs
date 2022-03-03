using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerState
{
    public override void EnterState(PlayerStateController player)
    {
        throw new System.NotImplementedException();
    }

    public override void FixedUpdate(PlayerStateController player)
    {
        if (player.InputController.IsJumping)
        {
            player.SetCurrentState(player.JumpState);
        }

        else if(player.InputController.MoveDirection != Vector3.zero)
        {
            player.SetCurrentState(player.WalkState);
        }

        else if (player.Rigidbody.velocity.y < 0)
        {
            player.SetCurrentState(player.FallState);
        }
    }

    public override void OnCollisionEnter(PlayerStateController player, Collision collision)
    {
        
    }
}
