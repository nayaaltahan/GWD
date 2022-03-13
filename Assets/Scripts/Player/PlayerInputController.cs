using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public Vector3 MoveDirection { get; private set; }
    public bool IsJumping { get; private set; } = false;

    public bool HasMadeChoice => pressedButton1 || pressedButton2 || pressedButton3;

    public int SelectedChoice { get; private set; } = -1;

    private bool pressedButton1, pressedButton2, pressedButton3;


    public void Jump(InputAction.CallbackContext value)
    {
        IsJumping = value.ReadValueAsButton();
    }

    public void SetMove(InputAction.CallbackContext value)
    {
        Vector2 tempVector = value.ReadValue<Vector2>();
        MoveDirection = new Vector3(tempVector.x, 0, 0);
    }

    public void PressedChoice1(InputAction.CallbackContext context)
    {
        PressedButton(context, ref pressedButton1, 0);

    }
    public void PressedChoice2(InputAction.CallbackContext context)
    {
        PressedButton(context, ref pressedButton2, 1);

    }
    public void PressedChoice3(InputAction.CallbackContext context)
    {
        PressedButton(context, ref pressedButton3, 2);
    }

    private void PressedButton(InputAction.CallbackContext context, ref bool value, int setChoice = -1)
    {
        if (context.performed)
        {
            value = true;
            SelectedChoice = setChoice;
        }
        if (context.canceled)
        {
            value = false;
            SelectedChoice = -1;
        }
    }
}
