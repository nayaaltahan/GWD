using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected float horizontalMoveMultiplier = 1.0f;
    public abstract void EnterState(PlayerStateController player);
    public virtual void FixedUpdate(PlayerStateController player)
    {
        player.Rigidbody.velocity = (player.InputController.MoveDirection * player.speed).WithY(player.Rigidbody.velocity.y + player.downVelocity) * horizontalMoveMultiplier;
        player.Animations.SetFloat("Blend", Math.Abs(player.Rigidbody.velocity.x));
    }
    public abstract void OnCollisionEnter(PlayerStateController player, Collision collision);

}
