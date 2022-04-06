using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Springboard : MonoBehaviour
{
    [SerializeField] private float velocity;

    public Vector3 GetVelocity()
    {
        return transform.up * velocity;
    }
}
