using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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


    private IEnumerator PlayDialogue(Color subtitleColor)
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
            subtitleColor = ChooseSubtitleColor(currentText);

            // Skip narrations
            var lower = currentText.ToLower();
            if (!lower.Contains("onwell:") && !lower.Contains("rani:"))
                continue;
            else
                subtitle.GetComponent<SubtitleController>().CreateSubtitle(currentText, subtitleColor, 5.0f, subtitleContainer);
            Debug.Log(currentText);
            // TODO: Parse for audio and delayAmount
            var audioInfo = currentStory.currentTags.FirstOrDefault()?.Split(' ');
            if (audioInfo != null)
            {
                // Get audio path and length
                // string is structured like so:  "event:/Voice/Rani/Hello 4.5"
                // First element is path to file in fmod
                // Second element is the length the audio takes to play, or the time we wait for the next diaogue choice
                var audioPath = audioInfo[0];
                var audioLength = audioInfo.ElementAtOrDefault(1);
                Debug.Log($"Playing audio: {audioPath} for {audioLength} seconds.");
                // TODO: Delete Try catch when audio is in
                // TODO: Do this better
                var pos = subtitleColorFrog == subtitleColor ? frogInputController.transform.position : robotInputController.transform.position;
                AudioManager.instance.Play3DOneShot(audioPath, pos);
                if (audioLength == null)
                {
                    FMODUnity.RuntimeManager.GetEventDescription(audioPath).getLength(out var length);
                    Debug.Log(length);

                }
                else
                    yield return new WaitForSeconds(float.Parse(audioInfo[1]));

                Debug.Log($"Finished playing audio, waiting for {delayBetweenSpeechBubbles} seconds.");
                yield return new WaitForSeconds(delayBetweenSpeechBubbles);
            }
            else
            {
                Debug.Log("Audio Info Null: ");
                foreach (var tag in currentStory.currentTags)
                {

                    Debug.Log(tag);
                }
                Debug.Log(audioInfo);
                Debug.Log(currentText);

                yield return new WaitForSeconds(defaultAudioDelay);
                yield return new WaitForSeconds(delayBetweenSpeechBubbles);
            }
        }

        hasMoreDialogue = true;
        yield return null;
    }

    private Color ChooseSubtitleColor(string currentText)
    {
        if (currentText.StartsWith("Onwell:"))
            return subtitleColorRobot;
        else if (currentText.StartsWith("Rani:"))
            return subtitleColorFrog;
        else
            return Color.white;
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
