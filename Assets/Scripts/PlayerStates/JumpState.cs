using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : PlayerState
{

    public override void EnterState(PlayerStateController player)
    {
        // TODO Initial velocity on enter jump
        player.Animations.SetTrigger("Jumping");
    }

    public override void FixedUpdate(PlayerStateController player)
    {
        base.FixedUpdate(player);

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
        

    }

    public override void OnCollisionEnter(PlayerStateController player, Collision collision)
    {

    }

}
