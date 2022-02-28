using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    private PlayerState currentState;
    
    public readonly IdleState IdleState = new IdleState();
    public readonly JumpState JumpState = new JumpState();
    public readonly WalkState WalkState = new WalkState();
    public readonly FallState FallState = new FallState();
    
    
    // Start is called before the first frame update
    void Start()
    {
        SetCurrentState(IdleState);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.Update(this);
    }

    private void OnCollisionEnter(Collision other)
    {
        currentState.OnCollisionEnter(this, other);
    }

    public void SetCurrentState(PlayerState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
}
