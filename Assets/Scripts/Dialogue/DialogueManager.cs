using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    private Story currentStory;

    [SerializeField]
    private GameObject[] robotSpeechBubbles;

    [SerializeField]
    private GameObject[] frogSpeechBubbles;

    [SerializeField]
    private PlayerInputController robotInputController;

    [SerializeField]
    private PlayerInputController frogInputController;

    private bool frogIsMakingChoice = false;
    private bool robotIsMakingChoice = false;

    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Debug.LogError("More than one Dialogue Manager in the scene");
    }

    private void Update()
    {
        if (!robotIsMakingChoice && !frogIsMakingChoice)
            return;

        if (robotIsMakingChoice && robotInputController.HasMadeChoice)
            SelectChoice(robotInputController.SelectedChoice);
        else if (frogIsMakingChoice && frogInputController.HasMadeChoice)
            SelectChoice(frogInputController.SelectedChoice);

    }

    public void StartStory(TextAsset inkJson)
    {
        currentStory = new Story(inkJson.text);
        // Skip to first choice
        GetNextChoice();
        DisplayChoices();
    }

    private void DisplayChoices()
    {
        var player = currentStory.variablesState["player"].ToString();
        if (player.Equals("Robot"))
        {
            robotIsMakingChoice = true;
            frogIsMakingChoice = false;
            HideSpeechBubbles(frogSpeechBubbles);
            ActivateSpeechBubbles(robotSpeechBubbles);
        }
        else
        {
            robotIsMakingChoice = false;
            frogIsMakingChoice = true;
            HideSpeechBubbles(robotSpeechBubbles);
            ActivateSpeechBubbles(frogSpeechBubbles);
        }
    }

    private bool GetNextChoice()
    {
        while (currentStory.currentChoices.Count == 0)
        {
            if (!currentStory.canContinue)
            {
                return false;
                break;
            }
            var text = currentStory.Continue();
            Debug.Log(text);
        }
        return true;
    }

    private void ActivateSpeechBubbles(GameObject[] speechBubbles)
    {
        var index = 0;
        foreach (var choice in currentStory.currentChoices)
        {
            Debug.Log(choice.text);
            speechBubbles[index].SetActive(true);
            speechBubbles[index].GetComponentInChildren<TextMeshProUGUI>().text = choice.text;
            index++;
        }
    }

    private void HideSpeechBubbles(GameObject[] speechBubbles)
    {
        foreach (var speechBubble in speechBubbles)
            speechBubble.SetActive(false);
    }

    private void HideAllSpeechBubbles()
    {
        foreach (var speechBubble in robotSpeechBubbles)
            speechBubble.SetActive(false);
        foreach (var speechBubble in frogSpeechBubbles)
            speechBubble.SetActive(false);
    }

    public void SelectChoice(int index)
    {
        if (currentStory.currentChoices.Count <= index)
            return;

        currentStory.ChooseChoiceIndex(index);
        var moreChoices = GetNextChoice();
        if (moreChoices)
            DisplayChoices();
        else
        {
            HideAllSpeechBubbles();
            robotIsMakingChoice = false;
            frogIsMakingChoice = false;
        }
    }
}
