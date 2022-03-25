using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    [SerializeField]
    private TextAsset inkyStory;

    [SerializeField]
    private string knotName;

    protected bool activated;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!activated && other.gameObject.CompareTag("Player"))
        {
            Debug.Log("player triggered me!");
            activated = true;
            DialogueManager.instance.StartStory(inkyStory.text, knotName);
        }
    }

    protected virtual void StartStory(string knotName)
    {
        DialogueManager.instance.StartStory(inkyStory.text, knotName);
    }
}
