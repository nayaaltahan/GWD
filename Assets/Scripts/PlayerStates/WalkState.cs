using System;
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
        base.FixedUpdate(player);

        if (player.InputController.IsJumping )// todo add isgrounded
        {
            player.SetCurrentState(new JumpState());
        }

        else if (player.InputController.MoveDirection == Vector3.zero)
        {
            player.SetCurrentState(new IdleState());
        }

        // Add fall state
        else if (false)
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
