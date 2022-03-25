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
    [ShowIf(EConditionOperator.Or, "showBoth", "showFrog")]
    private string frogKnot;

    [SerializeField]
    [ShowIf(EConditionOperator.Or, "showBoth", "showRobot")]
    private string robotKnot;

    # region editorSettings
    private bool showBoth => interactableBy == InteractableBy.Both;
    private bool showFrog => interactableBy == InteractableBy.Frog;
    private bool showRobot => interactableBy == InteractableBy.Robot;
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
                StartStory(frogKnot);
            }
            else if (interactableBy == InteractableBy.Robot && robotInputController.HasMadeChoice)
            {
                StartStory(robotKnot);
            }
        }
    }



    protected override void StartStory(string knotName)
    {
        base.StartStory(knotName);
        activated = true;
    }
}
