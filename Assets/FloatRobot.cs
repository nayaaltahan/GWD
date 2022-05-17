using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatRobot : MonoBehaviour
{
    [SerializeField] float size;
    [SerializeField] float sizeRandom;
    [SerializeField] float speed;
    [SerializeField] float speedRandom;
    Vector3 startPos;

    private void Start()
    {
        speed += Random.Range(-speedRandom, +speedRandom);
        size += Random.Range(-sizeRandom, sizeRandom);
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, startPos.y+ (Mathf.Sin(Time.time * speed) * size), transform.position.z);
    }
}
