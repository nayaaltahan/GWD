using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.PLAYER))
        {
            Debug.Log("Killing sexy frog or UwU roboto");
            CheckpointManager.instance.RespawnPlayers();
        }
    }
}
