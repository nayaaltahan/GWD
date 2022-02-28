using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Debug.LogError("More than one Dialogue Manager in the scene");
    }

    /// <summary>
    /// Uses Bubble dialogue system
    /// </summary>
    public void EnterDialogueModeBubbles()
    {

    }

    /// <summary>
    /// Uses narrator style dialogueSystem
    /// </summary>
    public void EnterDialogueMode()
    {

    }
}
