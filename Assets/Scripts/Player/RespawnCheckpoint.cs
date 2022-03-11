using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCheckpoint : MonoBehaviour
{ 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerRespawn>();
            if (player == null)
                player = other.transform.root.GetComponent<PlayerRespawn>();
            //player.UpdateCheckpoint(this);
        }
    }
}
