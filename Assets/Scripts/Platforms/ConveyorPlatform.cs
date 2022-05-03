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
            var playerStateController = collisionInfo.transform.GetComponent<PlayerStateController>();
            playerStateController.standingOnConveyorBelt = true;
        }
    }
    void OnTriggerStay(Collider collisionInfo)
    {
        if (collisionInfo.transform.CompareTag("Player"))
        {
            var movementController = collisionInfo.transform.GetComponent<MovementController>();
            movementController.Move((Vector3.right * speed * Time.fixedDeltaTime), standingOnConveyor:true);
        }
    }
    
    void OnTriggerExit(Collider collisionInfo)
    {
        if (collisionInfo.transform.CompareTag("Player"))
        {
            var playerStateController = collisionInfo.transform.GetComponent<PlayerStateController>();
            playerStateController.standingOnConveyorBelt = false;
        }
    }
}
