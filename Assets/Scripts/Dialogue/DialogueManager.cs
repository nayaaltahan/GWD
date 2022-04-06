using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    private Story currentStory;


    [SerializeField]
    private PlayerInputController robotInputController;

    [SerializeField]
    private PlayerInputController frogInputController;

    [Header("Chat bubble settings")]
    [SerializeField]
    private GameObject[] robotSpeechBubbles;

    [SerializeField]
    private GameObject[] frogSpeechBubbles;

    [SerializeField]
    private GameObject frogSpeechIndicator;

    [SerializeField]
    private GameObject robotSpeechIndicator;

    [SerializeField]
    private GameObject frogOptionalChatIndicator;

    [SerializeField]
    private GameObject robotOptionalChatIndicator;

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

    [SerializeField] 
    private float playerChoiceTimeLimit = 6.0f;

    

    /// Used to know when the player is making a choice so we can select the story knot
    private bool frogIsMakingChoice = false;
    /// Used to know when the player is making a choice so we can select the story knot
    private bool robotIsMakingChoice = false;
    
    private bool frogIsSpeaking = false;
    private bool robotIsSpeaking = false;
    /// used to see when we should stop the conversation while making choices
    private bool hasMoreDialogue = false;
    /// Current spoken dialogue text
    private string currentText;

    private float timeSpentMakingChoice = 0.0f;

    /// Current playing audio clip
    private FMOD.Studio.EventInstance currentAudioClip;

    private PlayerStateController frogPlayerController;
    private PlayerStateController robotPlayerController;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            frogPlayerController = frogInputController.GetComponent<PlayerStateController>();
            robotPlayerController = robotInputController.GetComponent<PlayerStateController>();
        }
        else
            Debug.LogError("More than one Dialogue Manager in the scene");
    }

    private void Update()
    {
        if (!robotIsMakingChoice && !frogIsMakingChoice)
            return;

        timeSpentMakingChoice += Time.deltaTime;
        
        // Select third choice if timer runs out
        if (timeSpentMakingChoice >= playerChoiceTimeLimit)
        {
            // TODO: Telemetry play didn't select a choice
            // Random choice from current story current choices
            var randomChoice = new Random().Next(0, currentStory.currentChoices.Count);
            StartCoroutine(SelectChoice(randomChoice));
        }


        if (robotIsMakingChoice && robotInputController.HasMadeChoice)
            StartCoroutine(SelectChoice(robotInputController.SelectedChoice));
        else if (frogIsMakingChoice && frogInputController.HasMadeChoice)
            StartCoroutine(SelectChoice(frogInputController.SelectedChoice));

    }

    public void StartStory(string story, string knotName)
    {
        // Stop coroutines in case any dialogues are still playing
        StopAllCoroutines();
        
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
        while (currentStory.currentChoices.Count == 0)
        {
            if (!currentStory.canContinue)
            {
                hasMoreDialogue = false;
                yield return new WaitForSeconds(delayBetweenSpeechBubbles);
                SlowPlayers(false);
                FreezePlayers(false);
                yield break;
            }

            currentText = currentStory.Continue();

            ParseDialogueType();

            if (currentStory.currentTags.Count > 0)
                Debug.Log(currentStory.currentTags[0]);

            bool shouldSkip = DisplaySubtitles(currentText);
            if (shouldSkip)
                continue;

            yield return StartCoroutine(ParseAudio());
        }

        hasMoreDialogue = true;
        yield return null;
    }

    private void FreezePlayers(bool shouldFreeze)
    {
        robotPlayerController.SetCanMove(!shouldFreeze);
        frogPlayerController.SetCanMove(!shouldFreeze);
    }

    private void SlowPlayers(bool shouldSlowDown)
    {
        robotPlayerController.SetIsWalkingSlow(shouldSlowDown);
        frogPlayerController.SetIsWalkingSlow(shouldSlowDown);
    }

    private IEnumerator ParseAudio()
    {

        var audioInfo = currentStory.currentTags.FirstOrDefault()?.Split(' ');
        if (audioInfo != null)
        {
            // Get audio path and length
            // string is structured like so:  "event:/Voice/Rani/Hello slowdown"
            // First element is path to file in fmod
            // Second element tells us if the player needs to be slowed down, or if we are showing a cutscene
            var audioPath = audioInfo[0];

            // TODO: Do this better
            int audioLengthMillis = PlayAudio(audioPath);

            ActivateSpeechIndicator();
            yield return new WaitForSeconds(audioLengthMillis / 1000);

            if (robotIsSpeaking)
                robotSpeechIndicator.SetActive(false);
            else
                frogSpeechIndicator.SetActive(false);
            yield return new WaitForSeconds(delayBetweenSpeechBubbles);
        }
        else
        {

            yield return new WaitForSeconds(defaultAudioDelay);
            yield return new WaitForSeconds(delayBetweenSpeechBubbles);
            frogIsSpeaking = false;
            robotIsSpeaking = false;
        }
    }

    private int PlayAudio(string audioPath)
    {
        var speakerGameObject = frogIsSpeaking ? frogInputController.gameObject : robotInputController.gameObject;
        if(currentAudioClip.isValid())
            currentAudioClip.stop(STOP_MODE.ALLOWFADEOUT);
        currentAudioClip = AudioManager.instance.Create3DEventInstance(audioPath, speakerGameObject);
        currentAudioClip.start();
        currentAudioClip.getDescription(out var description);
        description.getLength(out var audioLengthMillis);
        return audioLengthMillis;
    }

    private void ActivateSpeechIndicator()
    {
        if (robotIsSpeaking)
            robotSpeechIndicator.SetActive(true);
        else
            frogSpeechIndicator.SetActive(true);
    }

    private void ParseDialogueType()
    {
        if (currentStory.currentTags.Contains("slowdown"))
        {
            SlowPlayers(true);
        }
        else if (currentStory.currentTags.Contains("cutscene"))
        {
            FreezePlayers(true);
        }
        else
        {
            // Do nothing
            SlowPlayers(false);
            FreezePlayers(false);
        }
    }

    private bool DisplaySubtitles(string currentText)
    {
        var lower = currentText.ToLower();

        var subtitle = Instantiate(subtitlePrefab);
        subtitle.SetActive(true);
        var subtitleColor = ChooseSubtitleColor(lower);

        // Skip narrations
        if (!lower.Contains("onwell:") && !lower.Contains("rani:"))
            return true;
        else
        {
            subtitle.GetComponent<SubtitleController>().CreateSubtitle(currentText, subtitleColor, 5.0f, subtitleContainer);
            return false;

        }
    }

    private Color ChooseSubtitleColor(string currentText)
    {
        frogIsSpeaking = false;
        robotIsSpeaking = false;
        if (currentText.StartsWith("onwell:"))
        {

            robotIsSpeaking = true;
            return subtitleColorRobot;
        }
        else if (currentText.StartsWith("rani:"))
        {
            frogIsSpeaking = true;
            return subtitleColorFrog;
        }
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
        // Reset timer
        timeSpentMakingChoice = 0.0f;
        
        var selectedChoice = currentStory.currentChoices[index].text;
        currentStory.ChooseChoiceIndex(index);
        // TODO: Play Effect audio?
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

    public void ActivateOptionalDialogueIndicator(string name)
    {
        if (name.Equals("frog"))
        {
            frogOptionalChatIndicator.SetActive(true);
        }
        else if (name.Equals("robot"))
            robotOptionalChatIndicator.SetActive(true);
        else
            Debug.LogWarning("no optional dialogue indicators were set active");
    }
    public void DisableOptionalDialogueIndicator(string name)
    {
        if (name.Equals("frog"))
        {
            frogOptionalChatIndicator.SetActive(false);
        }
        else if (name.Equals("robot"))
            robotOptionalChatIndicator.SetActive(false);
        else
            Debug.LogWarning("no optional dialogue indicators were set inactive");
    }

    private void OnGUI()
    {
        if(Debug.isDebugBuild)
            GUI.Box (new Rect (Screen.width - 100,20,100,50), timeSpentMakingChoice.ToString("F2"));
    }

    public void SetChoiceTimeLimit(float newLimit)
    {
        playerChoiceTimeLimit = newLimit;
    }
    
}
