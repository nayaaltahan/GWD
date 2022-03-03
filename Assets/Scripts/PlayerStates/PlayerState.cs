using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    public abstract void EnterState(PlayerStateController player);
    public abstract void FixedUpdate(PlayerStateController player);
    public abstract void OnCollisionEnter(PlayerStateController player, Collision collision);
}
