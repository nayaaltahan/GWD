using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PuzzleCamTrigger : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera fromCam, toCam;
    [SerializeField] private bool isExitTrigger = false;
    private bool onwellTriggered, raniTriggered;
    private bool exitTriggered;



    private void Start()
    {
        Debug.Log("Subscribing");
        CheckpointManager.instance.OnRespawnPlayers += Reset;

    }

    private void OnDisable()
    {
        CheckpointManager.instance.OnRespawnPlayers -= Reset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
          
            
            if (other.name == "Rani")
                raniTriggered = !raniTriggered;
            
            else if (other.name == "Onwell")
                onwellTriggered = !onwellTriggered;
            
            if (isExitTrigger && exitTriggered && fromCam.Priority != 11)
                return;

            if (raniTriggered && onwellTriggered)
            {
                if (isExitTrigger)
                    exitTriggered = true;
                
                toCam.gameObject.SetActive(true);
                fromCam.Priority = 10;
                toCam.Priority = 11;
            }
            else
            {
                toCam.gameObject.SetActive(false);
                toCam.Priority = 10;
                fromCam.Priority = 11;
            }
        }
    }

    public void Reset()
    {
        Debug.LogWarning("RESET");
        raniTriggered = false;
        onwellTriggered = false;
        if (isExitTrigger)
        {
            exitTriggered = false;
            fromCam?.gameObject?.SetActive(false);            
        }
        else
        {
            toCam.Priority = 10;
            fromCam.Priority = 11;
            fromCam.gameObject.SetActive(true);
            toCam.gameObject.SetActive(false);
        }

    
    }
}
