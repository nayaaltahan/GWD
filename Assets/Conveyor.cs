using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] float speed;

    public Vector3 GetVelocity()
    {
        return Vector3.right * speed;
    }
}
