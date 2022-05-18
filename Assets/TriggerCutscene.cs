using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TriggerCutscene : MonoBehaviour
{
    public bool endScene = false;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!endScene)
                GameManager.instance.StartRobotsCutscene();
            else
                GameManager.instance.ShowEndScreen();
        }
    }
}
