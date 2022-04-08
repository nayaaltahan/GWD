using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.PLAYER))
        {
            Debug.Log("Setting new checkpoint");
            CheckpointManager.instance.SetNewCheckpoint(transform);
        }
    }
}
