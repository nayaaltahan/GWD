using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float speedX = 10.0f;
    [SerializeField] private float speedY = 10.0f;
    [SerializeField] private float speedZ = 10.0f;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(speedX, speedY, speedZ) * Time.deltaTime);
    }
}
