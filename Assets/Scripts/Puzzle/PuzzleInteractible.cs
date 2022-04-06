using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleInteractible : MonoBehaviour
{
    bool pressed = false;
    bool released = false;
    bool interacted = false;
    [SerializeField] float yEnd;
    [SerializeField] float yStart;
    [SerializeField] float moveSpeed;
    [SerializeField] private PuzzleObject puzzleObject;

    public bool Pressed { get => pressed; set => pressed = value; }
    public bool Interacted { get => interacted; set => interacted = value; }

    public void Interact()
    {
        puzzleObject.Interact();
    }

    private void Update()
    {
        PressurePlate();
    }

    public void PressurePlate()
    {
        if (pressed)
        {
            if (released) 
                released = false;
            if (transform.localPosition.y > yStart)
                transform.localPosition -= Vector3.up * moveSpeed * Time.deltaTime;
        }
        else
        {
            if (!released)
            {
                released = true;
                puzzleObject.OnPlateRelease();
            }
            if (transform.localPosition.y < yEnd)
                transform.localPosition += Vector3.up * moveSpeed * Time.deltaTime;
        }
    }
}
