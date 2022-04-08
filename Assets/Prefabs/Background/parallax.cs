using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour
{
    public Transform camera;
    public float speedCoefficient;
    Vector3 lastPos;

    void Start()
    {
        lastPos = camera.position;
    }

    void Update()
    {
        transform.position -= ((lastPos - camera.position) * speedCoefficient);
        lastPos = camera.position;
    }
}
