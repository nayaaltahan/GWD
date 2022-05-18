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
   
   
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.position.x > transform.position.x)
            {
                if (other.name == "Rani")
                    raniTriggered = true;
            
                if (other.name == "Onwell")
                    onwellTriggered = true;
            
                if (isExitTrigger && exitTriggered && fromCam.Priority != 11)
                    return;

                if (raniTriggered && onwellTriggered)
                {
                    toCam.gameObject.SetActive(true);
                    fromCam.Priority = 10;
                    toCam.Priority = 11;
                    if (isExitTrigger)
                    {
                        fromCam.gameObject.SetActive(false);
                        exitTriggered = true;
                    }
                }
            }
            else if (isExitTrigger)
            {
                if (other.name == "Rani")
                    raniTriggered = false;
            
                if (other.name == "Onwell")
                    onwellTriggered = false;
            }
            else if(!isExitTrigger)
            {
                if (other.name == "Rani")
                    raniTriggered = false;
            
                if (other.name == "Onwell")
                    onwellTriggered = false;
                
                toCam.gameObject.SetActive(false);
                toCam.Priority = 10;
                fromCam.Priority = 11;
            }
        }
    }

    public void Reset()
    {
        raniTriggered = false;
        onwellTriggered = false;
        if (isExitTrigger)
        {
            toCam.Priority = 11;
            fromCam.Priority = 10;
            exitTriggered = false;
            fromCam.gameObject.SetActive(false);            
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
