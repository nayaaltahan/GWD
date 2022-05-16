using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDialogue : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(!GameManager.instance.turnOnDialogue)
        {
            gameObject.SetActive(false);
            Debug.Log("TURN OFF DIALOGUE");
        }
    }
}
