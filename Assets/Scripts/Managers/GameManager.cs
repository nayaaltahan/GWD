using Cinemachine;
using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStates { MAINMENU, CHARACTERSELECT, PAUSEMENU, PLAYGAME, SINGLEPLAYGAME }
internal enum PlayerType
{
    robot, frog
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameStates state;

    //TODO: Create a UI manager, instead of having variables being managed in GameManager
    public GameObject canvas;

    public GameObject playerOne;
    public GameObject playerTwo;

    public bool playCutscene;

    public GameObject robotsFlyingCutscene;

    [Header("Debug")]
    // TODO: Spawn only 1 player when this is true
    public bool allowSinglePlayer = false;
    public bool turnOnDialogue = true;
    [SerializeField] private PlayerType playerType = PlayerType.frog;
    [SerializeField] private GameObject CineMachineCamera;
    [SerializeField] private GameObject CutsceneObj;

    // Start is called before the first frame update
    async void Start()
    {
        if (instance == null)
            instance = this;
        else
            Debug.LogError("More than one Game Manager in the scene");
        
        var options = new InitializationOptions();
        options.SetEnvironmentName(Debug.isDebugBuild ? "dev" : "live");
        
        await UnityServices.InitializeAsync(options);

        DialogueTracking.SendTrackingEvent(DialogueTrackingEvent.SessionStarted);
        
        await Events.CheckForRequiredConsents();

        if (SceneManager.GetActiveScene().name.Equals("MainABTest"))
        {
            DialogueTracking.SendTrackingEvent(DialogueTrackingEvent.PlayTestSession);
        }

        if (allowSinglePlayer)
        {
            if (playerType == PlayerType.robot)
            {
                playerOne.GetComponent<PlayerController>().SetUpPlayer(true);
                CameraManager.instance.AddPlayerToTargetGroup(playerOne);
                playerTwo.GetComponent<PlayerController>().enabled = false;
            }
            else if(playerType == PlayerType.frog)
            {
                playerTwo.GetComponent<PlayerController>().SetUpPlayer(true);
                CameraManager.instance.AddPlayerToTargetGroup(playerTwo);
                playerOne.GetComponent<PlayerController>().enabled = false;
            }
            SwitchState(GameStates.SINGLEPLAYGAME);
        }
        else
        {
            SwitchState(GameStates.CHARACTERSELECT);
        }


        PuzzleInteractible[] temp = FindObjectsOfType<PuzzleInteractible>();

        ActivateDialogue();
    }

    //TODO: Make this function work
    void ActivateDialogue()
    {
        DialogueTrigger[] triggers = FindObjectsOfType<DialogueTrigger>();

        for (int i = 0; i < triggers.Length; i++)
        {
            triggers[i].gameObject.SetActive(turnOnDialogue);
        }
    }

    public void StartRobotsCutscene()
    {
        robotsFlyingCutscene.SetActive(true);
    }

public void SwitchState(GameStates state)
    {
        Debug.Log(state.ToString());
        switch (state)
        {
            case GameStates.MAINMENU:
                this.state = GameStates.MAINMENU;
                break;
            case GameStates.CHARACTERSELECT:
                this.state = GameStates.CHARACTERSELECT;
                break;
            case GameStates.PAUSEMENU:
                this.state = GameStates.PAUSEMENU;
                break;
            case GameStates.PLAYGAME:
                this.state = GameStates.PLAYGAME;
                UIManager.instance.TurnCharSelectUIOn(false);
                CameraManager.instance.AddPlayerToTargetGroup(playerOne);
                CameraManager.instance.AddPlayerToTargetGroup(playerTwo);

                if (playCutscene)
                {
                    CutsceneObj.SetActive(true);
                }
                else
                {
                    CutsceneObj.SetActive(false);
                    CineMachineCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
                    Debug.Log("Don't play cutscene");
                }

                if(!turnOnDialogue || !playCutscene)
                {
                    playerOne.GetComponent<PlayerController>().SetUpPlayer(true);
                    playerTwo.GetComponent<PlayerController>().SetUpPlayer(true);
                }

                CineMachineCamera.SetActive(true);
                //Turn off any U.I. or objects that don't belong to PlayGame State
                break;
            case GameStates.SINGLEPLAYGAME:
                this.state = GameStates.SINGLEPLAYGAME;
                UIManager.instance.TurnCharSelectUIOn(false);
                CutsceneObj.SetActive(true);
                CineMachineCamera.SetActive(true);
                //Turn off any U.I. or objects that don't belong to PlayGame State
                break;
            default:
                break;
        }
    }
}
