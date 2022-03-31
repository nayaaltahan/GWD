using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerOptional : DialogueTrigger
{
    private enum InteractableBy
    {
        Both,
        Frog,
        Robot,
    };


    [SerializeField]
    private InteractableBy interactableBy = InteractableBy.Frog;

    [SerializeField]
    private PlayerInputController robotInputController;

    [SerializeField]
    private PlayerInputController frogInputController;


    [SerializeField]
    [Dropdown("GetStoryKnots")]
    [ShowIf(EConditionOperator.Or, "showBoth", "showFrog")]
    private StoryKnots frogKnot;

    [SerializeField]
    [Dropdown("GetStoryKnots")]
    [ShowIf(EConditionOperator.Or, "showBoth", "showRobot")]
    private StoryKnots robotKnot;



    # region editorSettings
    private bool showBoth => interactableBy == InteractableBy.Both;
    private bool showFrog => interactableBy == InteractableBy.Frog;
    private bool showRobot => interactableBy == InteractableBy.Robot;
    protected override bool showKnotName => false;


    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Interactable by: " + interactableBy.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if (!activated)
        {                                                   // TODO: Specific input?
            if ((interactableBy == InteractableBy.Frog || interactableBy == InteractableBy.Both) && frogInputController.PressedButton1)
            {
                StartStory(frogKnot.ToString());
                DialogueManager.instance.DisableOptionalDialogueIndicator("frog");
                DialogueManager.instance.DisableOptionalDialogueIndicator("robot");

            }
            else if ((interactableBy == InteractableBy.Robot || interactableBy == InteractableBy.Both) && robotInputController.PressedButton1)
            {
                StartStory(robotKnot.ToString());
                DialogueManager.instance.DisableOptionalDialogueIndicator("frog");
                DialogueManager.instance.DisableOptionalDialogueIndicator("robot");

            }
        }
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (activated)
            return;
        // TODO: Show dialogue indicator
        if (other.transform.root.name.Equals("PlayerOne"))
        {
            DialogueManager.instance.ActivateOptionalDialogueIndicator("robot");
        }
        else if (other.transform.root.name.Equals("PlayerTwo"))
        {
            DialogueManager.instance.ActivateOptionalDialogueIndicator("frog");
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (activated)
            return;
        // TODO: Change names
        if (other.transform.root.name == "PlayerOne")
        {
            DialogueManager.instance.DisableOptionalDialogueIndicator("robot");
        }
        else if (other.transform.root.name == "PlayerTwo")
        {
            DialogueManager.instance.DisableOptionalDialogueIndicator("frog");
        }
    }

    protected override void StartStory(string knotName)
    {
        base.StartStory(knotName);
        activated = true;
    }
}
