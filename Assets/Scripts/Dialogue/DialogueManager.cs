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

    [Header("Audio Settings")]

    [SerializeField]
    [Tooltip("How long is the default value between speech bubbles if no audio is found")]
    private float defaultAudioDelay = 2.0f;

    [SerializeField]
    [Tooltip("How long after the audio stops playing do we wait to show the next speech bubbles")]
    private float delayBetweenSpeechBubbles = 0.5f;

    [Header("Subtitle Settings")]
    [SerializeField]
    private Transform subtitleContainer;

    [SerializeField]
    private GameObject subtitlePrefab;

    [SerializeField]
    private Color subtitleColorFrog = Color.cyan;

    [SerializeField]
    private Color subtitleColorRobot = Color.white;




    private bool frogIsMakingChoice = false;
    private bool robotIsMakingChoice = false;
    // used to see when we should stop the conversation while making choices
    private bool hasMoreDialogue = false;

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
            StartCoroutine(SelectChoice(robotInputController.SelectedChoice));
        else if (frogIsMakingChoice && frogInputController.HasMadeChoice)
            StartCoroutine(SelectChoice(frogInputController.SelectedChoice));

    }

    public void StartStory(string story, string knotName)
    {
        currentStory = new Story(story);
        if (!string.IsNullOrEmpty(knotName))
            currentStory.ChoosePathString(knotName);
        // Skip to first choice
        StartCoroutine(StartStoryCoroutine());
    }

    private IEnumerator StartStoryCoroutine()
    {
        var color = robotIsMakingChoice ? subtitleColorRobot : subtitleColorFrog;
        yield return StartCoroutine(PlayDialogue(color));
        DisplayChoices();
    }

    private void DisplayChoices()
    {
        var player = currentStory.currentChoices[0].text.ToLower().Contains("onwell:");
        if (player)
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


    private IEnumerator PlayDialogue(Color color)
    {

        //if (currentStory.currentChoices.Count > 0 && currentStory.canContinue)
        //{
        //    var text = currentStory.Continue();
        //    Debug.LogWarning("Can continue");
        //    Debug.Log(text);
        //}
        var currentText = currentStory.currentText;
        //if (!string.IsNullOrEmpty(currentText))
        //{
        //    Debug.LogWarning(currentText);

        //}

        while (currentStory.currentChoices.Count == 0)
        {
            if (!currentStory.canContinue)
            {
                hasMoreDialogue = false;
                yield return new WaitForSeconds(delayBetweenSpeechBubbles);
                yield break;
            }
            // TODO: Play audio from text

            // TODO: Display speaking speech bubble
            currentText = currentStory.Continue();
            var subtitle = Instantiate(subtitlePrefab);
            subtitle.SetActive(true);

            subtitle.GetComponent<SubtitleController>().CreateSubtitle(currentText, color, 5.0f, subtitleContainer);
            Debug.Log(currentText);
            // TODO: Parse for audio and delayAmount
            yield return new WaitForSeconds(defaultAudioDelay);
            yield return new WaitForSeconds(delayBetweenSpeechBubbles);
        }

        hasMoreDialogue = true;
        yield return null;
    }

    private void ActivateSpeechBubbles(GameObject[] speechBubbles)
    {
        var index = 0;
        foreach (var choice in currentStory.currentChoices)
        {
            speechBubbles[index].SetActive(true);                                               // TODO: Don't do this
            speechBubbles[index].GetComponentInChildren<TextMeshProUGUI>().text = choice.text.Replace("Onwell: ", "").Replace("Rani: ", "");
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

    public IEnumerator SelectChoice(int index)
    {
        if (currentStory.currentChoices.Count <= index)
            yield return null;
        var selectedChoice = currentStory.currentChoices[index].text;
        currentStory.ChooseChoiceIndex(index);
        // TODO: Play Effect audio?
        // If no audio is found, wait the default amount
        HideAllSpeechBubbles();
        var color = robotIsMakingChoice ? subtitleColorRobot : subtitleColorFrog;
        robotIsMakingChoice = false;
        frogIsMakingChoice = false;


        yield return StartCoroutine(PlayDialogue(color));

        if (hasMoreDialogue)
        {
            DisplayChoices();
        }
        else
        {
            HideAllSpeechBubbles();
            robotIsMakingChoice = false;
            frogIsMakingChoice = false;
        }
    }
}
