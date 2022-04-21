using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerState
{
    public override void EnterState(PlayerStateController player)
    {
        player.Animations.SetBool(Constants.FALLING, false);
        player.Animations.SetBool(Constants.JUMPING, false);
        Debug.Log("IDLE STATE");

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

        // Add fall statement
        else if (false)
        {
            player.SetCurrentState(player.FallState);
        }
    }

    public override void OnCollisionEnter(PlayerStateController player, Collision collision)
    {
        
    }
}
