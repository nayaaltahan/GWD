using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public bool IsJumping { get; private set; } = false;
    
    public void Jump(InputAction.CallbackContext value)
    {
        IsJumping = value.ReadValueAsButton();
        Debug.Log(IsJumping);
    }
    
}
