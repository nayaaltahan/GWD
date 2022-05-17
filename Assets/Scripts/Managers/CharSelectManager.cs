using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using TMPro;
using DG.Tweening;
using MonKey.Extensions;
using UnityEngine.EventSystems;
using UnityEngine.XR;
using InputDevice = UnityEngine.XR.InputDevice;

//TODO: This shouldn't be a singleton
public class CharSelectManager : MonoBehaviour
{
    public static CharSelectManager instance;

    [SerializeField] private List<Button> p1Buttons;
    [SerializeField] private List<Button> p2Buttons;

    [SerializeField] private GameObject p1JoinButton, p2JoinButton;
    
    [HideInInspector] public GameObject p1MidButton;
    [HideInInspector] public GameObject p2MidButton;

    private MultiplayerEventSystem p1multiplayerES;
    private MultiplayerEventSystem p2multiplayerES;

    public GameObject p1Root;
    public GameObject p2Root;

    private bool isP1Ready;
    private bool isP2Ready;

    private bool isP1Left;
    private bool isP2Left;

    public delegate void OnPlayersConnectedDelegate();

    public OnPlayersConnectedDelegate OnPlayersConnected; // TODO: MAKE ACTION PLAYER WHEN WE START THE GAME

    public delegate void OnPlayerJoined();

    public OnPlayerJoined OnPlayerOneJoined;
    public OnPlayerJoined OnPlayerTwoJoined;

    public GameObject FrogPlayer => isP1Left ? GameManager.instance.playerOne : GameManager.instance.playerTwo;
    public GameObject RobotPlayer => isP1Left ? GameManager.instance.playerTwo : GameManager.instance.playerOne;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Debug.LogError("More than one Character Selection Manager in the scene");


        OnPlayerOneJoined += () =>
        {
            foreach (var button in p1Buttons)
                button.gameObject.SetActive(true);
            p1JoinButton.SetActive(false);
            p1multiplayerES = GameManager.instance.playerOne.GetComponent<MultiplayerEventSystem>();
            p1MidButton = p1Buttons[1].gameObject;
            ActivatePlayerSelectionUI(ref p1multiplayerES, p1Buttons);
        };

        OnPlayerTwoJoined += () =>
        {
            foreach (var button in p2Buttons)
                button.gameObject.SetActive(true);
            p2JoinButton.SetActive(false);
            p2multiplayerES = GameManager.instance.playerTwo.GetComponent<MultiplayerEventSystem>();
            p2MidButton = p2Buttons[1].gameObject;
            ActivatePlayerSelectionUI(ref p2multiplayerES, p2Buttons);
            FindObjectOfType<PlayerInputManager>().DisableJoining();
        };
    }

    private void ActivatePlayerSelectionUI(ref MultiplayerEventSystem mes, List<Button> buttons)
    {
        mes.SetSelectedGameObject(buttons[1].gameObject);
        mes.firstSelectedGameObject = buttons[1].gameObject;
        mes.playerRoot = buttons[0].transform.parent.parent.gameObject;
    }


    private void Update()
    {
        if (p1multiplayerES)
            UpdatePlayerOneSelection();

        if (p2multiplayerES)
            UpdatePlayerTwoSelection();
    }

    public void ReadyP1(bool isLeft)
    {
        if (isP2Ready && isP2Left == isLeft)
            return;

        isP1Left = isLeft;
        isP1Ready = !isP1Ready;

        for (int i = 0; i < p1Buttons.Count; i++)
        {
            string tempString = "P1";

            if (p1Buttons[i].gameObject == p1multiplayerES.currentSelectedGameObject)
            {
                if (isP1Ready)
                    foreach (var arrow in p1Buttons[i].transform.GetChildren())
                    {
                        arrow.gameObject.SetActive(false);
                    }
            }
            else
                p1Buttons[i].enabled = !isP1Ready;

            //p1Buttons[i].transform.GetChild(0).GetComponent<TMP_Text>().text = tempString;
        }

        Ready();
    }

    public void ReadyP2(bool isLeft)
    {
        if (isP1Ready && isP1Left == isLeft)
            return;

        isP2Left = isLeft;
        isP2Ready = !isP2Ready;

        for (int i = 0; i < p2Buttons.Count; i++)
        {
            string tempString = "P1";

            if (p2Buttons[i].gameObject == p2multiplayerES.currentSelectedGameObject)
            {
                if (isP2Ready)
                    foreach (var arrow in p2Buttons[i].transform.GetChildren())
                    {
                        arrow.gameObject.SetActive(false);
                    }
            }
            else
                p2Buttons[i].enabled = !isP2Ready;

            //p2Buttons[i].transform.GetChild(0).GetComponent<TMP_Text>().text = tempString;
        }

        Ready();
    }

    //Check if game is Ready to play, start PlayState and setup players
    public void Ready()
    {
        if ((isP1Ready == false || isP2Ready == false) && !GameManager.instance.allowSinglePlayer)
            return;
        Debug.Log("IS P1 LEFT: " + isP1Left);
        Debug.Log("IS P2 LEFT: " + isP2Left);
        if (GameManager.instance.allowSinglePlayer)
        {
            var p1 = p1multiplayerES.transform.GetChild(0).gameObject;
            p1.SetActive(true);
            OnPlayersConnected?.Invoke();
            p1.GetComponent<PlayerController>().SetUpPlayer(false);
            GameManager.instance.SwitchState(GameStates.PLAYGAME);
            p1multiplayerES.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
            return;

        }
        else if (isP1Left)
        {
            var p1 = p1multiplayerES.transform.GetChild(0).gameObject;
            var p2 = p2multiplayerES.transform.GetChild(1).gameObject;
            p1.SetActive(true);
            p2.SetActive(true);
            OnPlayersConnected?.Invoke();
            p1.GetComponent<PlayerController>().SetUpPlayer(false);
            p2.GetComponent<PlayerController>().SetUpPlayer(false);
        }
        else
        {
            var p1 = p1multiplayerES.transform.GetChild(1).gameObject;
            var p2 = p2multiplayerES.transform.GetChild(0).gameObject;
            p1.SetActive(true);
            p2.SetActive(true);
            OnPlayersConnected?.Invoke();
            p1.GetComponent<PlayerController>().SetUpPlayer(false);
            p2.GetComponent<PlayerController>().SetUpPlayer(false);
        }
        FrogPlayer.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        RobotPlayer.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        GameManager.instance.SwitchState(GameStates.PLAYGAME);
    }

    //Update images to make sure only selected one shows for player one
    private void UpdatePlayerOneSelection()
    {
        bool temp = false;
        for (int i = 0; i < p1Buttons.Count; i++)
        {
            temp = false;

            if (p1Buttons[i].gameObject == p1multiplayerES.currentSelectedGameObject)
                temp = true;

            p1Buttons[i].GetComponent<Image>().enabled = temp;
            if (!isP1Ready)
            {
                foreach (var arrow in p1Buttons[i].transform.GetChildren())
                {
                    arrow.gameObject.SetActive(temp);
                }
            }
        }
    }

    //Update images to make sure only selected one shows for player Two
    private void UpdatePlayerTwoSelection()
    {
        bool temp = false;
        for (int i = 0; i < p2Buttons.Count; i++)
        {
            temp = false;

            if (p2Buttons[i].gameObject == p2multiplayerES.currentSelectedGameObject)
                temp = true;

            p2Buttons[i].GetComponent<Image>().enabled = temp;
            if (!isP2Ready)
            {
                foreach (var arrow in p2Buttons[i].transform.GetChildren())
                {
                    arrow.gameObject.SetActive(temp);
                }
            }
        }
    }
}