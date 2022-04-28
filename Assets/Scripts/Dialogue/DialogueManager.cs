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

    public FMODUnity.EventReference dialogueSnapshot;
    FMOD.Studio.EventInstance dialogueSnapshotInstance;

    [SerializeField]
    private PlayerInputController robotInputController;

    [SerializeField]
    private PlayerInputController frogInputController;

    [Header("Chat bubble settings")] [SerializeField]
    private float timeBetweenChatbubbles = 0.3f;
    
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

    public float PlayerChoiceTimeLimit => playerChoiceTimeLimit;

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
            dialogueSnapshotInstance = FMODUnity.RuntimeManager.CreateInstance(dialogueSnapshot.Guid);
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

        dialogueSnapshotInstance.start();
        
        // Skip to first choice
        StartCoroutine(StartStoryCoroutine());
    }

    private IEnumerator StartStoryCoroutine()
    {
        var color = robotIsMakingChoice ? subtitleColorRobot : subtitleColorFrog;
        yield return StartCoroutine(PlayDialogue(color));
        DisplayChoices();
    }

    private void DisplayChoices(int index = -1)
    {
        var player = currentStory.currentChoices[0].text.ToLower().Contains("onwell:");

        if (player)
        {
            robotIsMakingChoice = true;
            frogIsMakingChoice = false;
            // HideSpeechBubbles(frogSpeechBubbles, index);
            StartCoroutine(ActivateSpeechBubbles(robotSpeechBubbles));
        }
        else
        {
            robotIsMakingChoice = false;
            frogIsMakingChoice = true;
            // HideSpeechBubbles(robotSpeechBubbles, index);
            StartCoroutine(ActivateSpeechBubbles(frogSpeechBubbles));
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
            float audioLengthSeconds = audioLengthMillis / 1000.0f;

            ActivateSpeechIndicator();
            yield return new WaitForSeconds(audioLengthSeconds);

            if (robotIsSpeaking)
                StartCoroutine(robotSpeechIndicator.GetComponent<ChatBubbleController>().DisableSpeechBubble(0.2f, shouldShrink: true));
            else
                StartCoroutine(frogSpeechIndicator.GetComponent<ChatBubbleController>().DisableSpeechBubble(0.2f, shouldShrink: true));
            yield return new WaitForSeconds(delayBetweenSpeechBubbles);
        }
        else
        {
            ActivateSpeechIndicator();
            yield return new WaitForSeconds(defaultAudioDelay);
            if (robotIsSpeaking)
                StartCoroutine(robotSpeechIndicator.GetComponent<ChatBubbleController>().DisableSpeechBubble(0.2f, shouldShrink: true));
            else
                StartCoroutine(frogSpeechIndicator.GetComponent<ChatBubbleController>().DisableSpeechBubble(0.2f, shouldShrink: true));
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
        {
            robotSpeechIndicator.SetActive(true);
            StartCoroutine(robotSpeechIndicator.GetComponent<ChatBubbleController>().ActivateSpeechBubble(0.5f));
        }
        else
        {
            frogSpeechIndicator.SetActive(true);
            StartCoroutine(frogSpeechIndicator.GetComponent<ChatBubbleController>().ActivateSpeechBubble(0.5f));
        }
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

    private IEnumerator ActivateSpeechBubbles(GameObject[] speechBubbles)
    {
        var index = 0;
        foreach (var choice in currentStory.currentChoices)
        {
            var current = speechBubbles[index];
            current.SetActive(true);                                               // TODO: Don't do this
            current.GetComponentInChildren<TextMeshProUGUI>().text = choice.text.Replace("Onwell: ", "").Replace("Rani: ", "");
            StartCoroutine(current.GetComponent<ChatBubbleController>().OnEnableCoroutine());

            index++;
            yield return new WaitForSeconds(timeBetweenChatbubbles);
        }
    }

    private void HideSpeechBubbles(GameObject[] speechBubbles, int index = -1)
    {
        var idx = 0;
        //Debug.Log("Index: " + index);
        foreach (var speechBubble in speechBubbles)
        {
            if(idx == index && index != -1)
                StartCoroutine(speechBubble.GetComponent<ChatBubbleController>().DisableSpeechBubble(1.0f, 1.0f));
            else
                StartCoroutine(speechBubble.GetComponent<ChatBubbleController>().DisableSpeechBubble(1.2f));
            idx++;

        }
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

        if(frogIsMakingChoice)
            HideSpeechBubbles(frogSpeechBubbles, index);
        else if (robotIsMakingChoice)
            HideSpeechBubbles(robotSpeechBubbles, index);
        // TODO: Play Effect audio?
        // HideAllSpeechBubbles();
        var color = robotIsMakingChoice ? subtitleColorRobot : subtitleColorFrog;
        robotIsMakingChoice = false;
        frogIsMakingChoice = false;


        yield return StartCoroutine(PlayDialogue(color));

        if (hasMoreDialogue)
        {
            DisplayChoices(index);
        }
        else
        {
            HideAllSpeechBubbles();
            robotIsMakingChoice = false;
            frogIsMakingChoice = false;
            dialogueSnapshotInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

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
        // if(Debug.isDebugBuild)
        //     GUI.Box (new Rect (Screen.width - 100,20,100,50), timeSpentMakingChoice.ToString("F2"));
    }

    public void SetChoiceTimeLimit(float newLimit)
    {
        playerChoiceTimeLimit = newLimit;
    }
    
}
