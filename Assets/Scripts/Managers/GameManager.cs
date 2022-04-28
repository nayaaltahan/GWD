using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Analytics;

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
    
    [Header("Debug")]
    // TODO: Spawn only 1 player when this is true
    [SerializeField] public bool allowSinglePlayer = false;
    [SerializeField] private PlayerType playerType = PlayerType.frog;

    // Start is called before the first frame update
    async void Start()
    {
        if (instance == null)
            instance = this;
        else
            Debug.LogError("More than one Game Manager in the scene");
        
        var options = new InitializationOptions();
        options.SetEnvironmentName(Debug.isDebugBuild ? "development" : "production");

        await UnityServices.InitializeAsync(options);
        
        DialogueTracking.SendTrackingEvent(DialogueTrackingEvent.SessionStarted);
        
        await Events.CheckForRequiredConsents();

        if (allowSinglePlayer)
        {
            if (playerType == PlayerType.robot)
            {
                playerOne.GetComponent<PlayerController>().SetUpPlayer(true);
                CameraManager.instance.AddPlayerToTargetGroup(playerOne);
                playerTwo.SetActive(false);
            }
            else if(playerType == PlayerType.frog)
            {
                playerTwo.GetComponent<PlayerController>().SetUpPlayer(true);
                CameraManager.instance.AddPlayerToTargetGroup(playerTwo);
                playerOne.SetActive(false);
            }
            SwitchState(GameStates.SINGLEPLAYGAME);
        }
        else
        {
            SwitchState(GameStates.CHARACTERSELECT);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
                //Turn off any U.I. or objects that don't belong to PlayGame State
                break;
            case GameStates.SINGLEPLAYGAME:
                this.state = GameStates.SINGLEPLAYGAME;
                UIManager.instance.TurnCharSelectUIOn(false);
                //Turn off any U.I. or objects that don't belong to PlayGame State
                break;
            default:
                break;
        }
    }
}
