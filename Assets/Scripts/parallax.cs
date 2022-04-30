using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour
{
    private Transform camera;
    public float speedCoefficient;
    Vector3 lastPos;

    void Start()
    {
        camera = Camera.main.transform;
        lastPos = camera.position;
    }

    void Update()
    {
        transform.position -= ((lastPos - camera.position) * speedCoefficient);
        lastPos = camera.position;
    }
}
