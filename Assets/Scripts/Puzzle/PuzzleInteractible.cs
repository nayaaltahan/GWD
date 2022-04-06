using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleInteractible : MonoBehaviour
{
    [SerializeField] private PuzzleObject puzzleObject;
    [SerializeField] float yStart;
    [SerializeField] float yEnd;
    [SerializeField] float speed;
    bool pressed;
    bool interacted;

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
            if (transform.localPosition.y > yStart)
                transform.localPosition -= Vector3.up * speed * Time.deltaTime;
        }
        else
            if (transform.localPosition.y < yEnd)
            transform.localPosition += Vector3.up * speed * Time.deltaTime;

    }
}
