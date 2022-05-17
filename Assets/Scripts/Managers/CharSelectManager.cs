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
            p1multiplayerES = GameManager.instance.playerOne.GetComponent<MultiplayerEventSystem>();
            p1MidButton = p1Buttons[1].gameObject;
            ActivatePlayerSelectionUI(ref p1multiplayerES, p1Buttons);

        };

        OnPlayerTwoJoined += () =>
        {
            foreach (var button in p2Buttons)
                button.gameObject.SetActive(true);
            p2multiplayerES = GameManager.instance.playerTwo.GetComponent<MultiplayerEventSystem>();
            p2MidButton = p2Buttons[1].gameObject;
            ActivatePlayerSelectionUI(ref p2multiplayerES, p2Buttons);
            
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
        if (isP1Ready == false || isP2Ready == false)
            return;

        GameManager.instance.playerOne.GetComponent<PlayerController>().SetUpPlayer(false);
        GameManager.instance.playerTwo.GetComponent<PlayerController>().SetUpPlayer(false);
        GameManager.instance.SwitchState(GameStates.PLAYGAME);
        Debug.Log("IS P1 LEFT: " + isP1Left);
        Debug.Log("IS P2 LEFT: " + isP2Left);
    }

    //Update images to make sure only selected one shows for player one
    private void UpdatePlayerOneSelection()
    {
        Debug.Log("p1 selection");
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
        Debug.Log("p2 selection");

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

    private void OnGUI()
    {
            var startX = Screen.width / 2;
            var startY = Screen.height - 250;
            var gamepads = Gamepad.all;
            GUI.Box(new Rect(startX - 100, startY + 80, 200, 50), "Connected controllers: " + gamepads.Count);
    }
}