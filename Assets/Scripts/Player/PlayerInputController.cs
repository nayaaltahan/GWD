using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public Vector3 MoveDirection {get; private set; }
    public bool IsJumping { get; private set; } = false;
    
    public void Jump(InputAction.CallbackContext value)
    {
        IsJumping = value.ReadValueAsButton();
    }
    
    public void SetMove(InputAction.CallbackContext value)
    {
        Vector2 tempVector = value.ReadValue<Vector2>();
        MoveDirection = new Vector3(tempVector.x, 0, 0);
    }
    
}
