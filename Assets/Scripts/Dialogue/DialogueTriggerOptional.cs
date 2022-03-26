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
    new protected StoryKnots knotName;

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
            if (interactableBy == InteractableBy.Frog && frogInputController.HasMadeChoice)
            {
                StartStory(frogKnot.ToString());
            }
            else if (interactableBy == InteractableBy.Robot && robotInputController.HasMadeChoice)
            {
                StartStory(robotKnot.ToString());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // TODO: Show dialogue indicator

    }

    private void OnTriggerExit(Collider other)
    {
        // TODO: Hide dialogue indicator
    }

    protected override void StartStory(string knotName)
    {
        base.StartStory(knotName);
        activated = true;
    }
}
