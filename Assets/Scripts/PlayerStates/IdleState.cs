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
            player.SetCurrentState(new JumpState());
        }

        else if(player.InputController.MoveDirection != Vector3.zero)
        {
            player.SetCurrentState(new WalkState());
        }

        else if (player.Rigidbody.velocity.y < 0)
        {
            player.SetCurrentState(new FallState());
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
