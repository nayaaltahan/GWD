using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : PuzzleObject
{
    [SerializeField] private Vector3 axis;
    [SerializeField] private float rotateSpeed;

    public override void Interact()
    {
        transform.Rotate(axis, rotateSpeed * Time.deltaTime);
    }
}
