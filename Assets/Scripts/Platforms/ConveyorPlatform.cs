using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorPlatform : MonoBehaviour
{
    public float speed = 1f;
    
    void OnTriggerEnter(Collider collisionInfo)
    {
        if (collisionInfo.transform.CompareTag("Player"))
        {
            var movementController = collisionInfo.transform.GetComponent<MovementController>();
            movementController.Move((Vector3.right * speed * Time.fixedDeltaTime));
        }
    }
    
    void OnTriggerStay(Collider collisionInfo)
    {
        if (collisionInfo.transform.CompareTag("Player"))
        {
            var movementController = collisionInfo.transform.GetComponent<MovementController>();
            var controller = collisionInfo.transform.GetComponent<PlayerStateController>();
            movementController.Move((Vector3.right * speed * Time.fixedDeltaTime));
        }
    }
}
