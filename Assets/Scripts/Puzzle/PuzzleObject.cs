using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PuzzleObject : MonoBehaviour 
{
    public abstract void Interact();
    public abstract void OnPlateRelease();
}