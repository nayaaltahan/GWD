using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleInteractible : MonoBehaviour
{
    bool pressed = false;
    bool interacted = true;
    int interactedCount = 0;
    [SerializeField] float yEnd;
    [SerializeField] float yStart;
    [SerializeField] float moveSpeed;
    [SerializeField] private PuzzleObject puzzleObject;

    public bool Pressed { get => pressed; set => pressed = value; }

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
                transform.localPosition -= Vector3.up * moveSpeed * Time.deltaTime;

            Interact();
        }
        else
        {
            if (transform.localPosition.y < yEnd)
                transform.localPosition += Vector3.up * moveSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TEST");
        if (other.tag != "Player")
            return;
        Debug.Log("PLAYER");

        interactedCount++;
        interacted = true;
        pressed = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
            return;

        interactedCount--;

        if (interactedCount == 0)
        {
            interacted = false;
            pressed = false;
            puzzleObject.OnPlateRelease();
        }
    }
}
