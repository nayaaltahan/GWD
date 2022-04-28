using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleInteractible : MonoBehaviour
{
    bool pressed = false;
    int interactedCount = 0;
    [SerializeField] float yEnd;
    [SerializeField] float yStart;
    [SerializeField] float moveSpeed;
    private PuzzleMovingPlatform puzzleObject;
    private PuzzleInteractible otherPuzzleConnection;

    public bool Pressed { get => pressed; set => pressed = value; }
    public PuzzleMovingPlatform PuzzleObject { get => puzzleObject; set => puzzleObject = value; }

    private void Start()
    {
        for (int i = 0; i < puzzleObject.puzzleInteractables.Count; i++)
        {
            if(puzzleObject.puzzleInteractables[i] != this)
            {
                otherPuzzleConnection = puzzleObject.puzzleInteractables[i];
            }
        }
    }

    public void Interact()
    {
        if(!otherPuzzleConnection.Pressed && puzzleObject.Moving == false)
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

    private void OnTriggerStay(Collider other)
    {
 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" )
            return;

        interactedCount++;
        pressed = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
            return;

        interactedCount--;

        if (interactedCount == 0)
        {
            pressed = false;
            if(!otherPuzzleConnection.pressed)
            {
                puzzleObject.OnPlateRelease();
            }
        }
    }
}
