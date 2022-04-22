using Ink.Runtime;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    [SerializeField]
    private TextAsset inkyStory;

    [SerializeField]
    [Dropdown("GetStoryKnots")]
    [ShowIf("showKnotName")]
    protected StoryKnots knotName;

    protected bool activated;

    // we need this to hide the variable in inherited classes
    protected virtual bool showKnotName => true;

    protected StoryKnots[] GetStoryKnots()
    {
        return (StoryKnots[])Enum.GetValues(typeof(StoryKnots));
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!activated && other.gameObject.CompareTag("Player") && !GameManager.instance.allowSinglePlayer)
        {
            Debug.Log("player triggered me!", gameObject);
            activated = true;
            DialogueManager.instance.StartStory(inkyStory.text, knotName.ToString());
        }
    }

    protected virtual void StartStory(string knotName)
    {
        DialogueManager.instance.StartStory(inkyStory.text, knotName);
    }
}
