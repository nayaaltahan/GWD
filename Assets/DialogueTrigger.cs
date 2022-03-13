using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    [SerializeField]
    private TextAsset inkyStory;

    private bool activated;
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
            DialogueManager.instance.StartStory(inkyStory);
        }
    }
}
