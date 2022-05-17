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

    [SerializeField] private PlayerInputManager playerInputManager;


    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Debug.LogError("More than one Character Selection Manager in the scene");

        p1MidButton = p1Buttons[1].gameObject;
        p2MidButton = p2Buttons[1].gameObject;

        p1multiplayerES = GameManager.instance.playerOne.GetComponent<MultiplayerEventSystem>();
        p2multiplayerES = GameManager.instance.playerTwo.GetComponent<MultiplayerEventSystem>();
        
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

    private void OnGUI()
    {
        if (GameManager.instance.state == GameStates.CHARACTERSELECT)
        {
            var startX = Screen.width / 2;
            var startY = Screen.height - 250;
            var gamepads = Gamepad.all;

            var playerInput1 = p1multiplayerES.GetComponent<PlayerInput>();
            var playerInput2 = p2multiplayerES.GetComponent<PlayerInput>();
            if (GUI.Button(new Rect(startX - 50, startY, 100, 50), "Use controllers"))
            {
                playerInput1.SwitchCurrentControlScheme("Gamepad", gamepads[0]);
                playerInput2.SwitchCurrentControlScheme("Gamepad", gamepads[1]);
            }

            if (!playerInput1.currentControlScheme.Equals("Gamepad") ||
                !playerInput2.currentControlScheme.Equals("Gamepad"))
            {
                var style = new GUIStyle();
                style.fontSize = 30;
                style.fontStyle = FontStyle.Bold;
                style.normal.textColor = Color.red;
                GUI.Label(new Rect(startX - 270, startY + 80, 200, 50), "MOUSE&KEYBOARD DETECTED", style);
            }
            else
            {
                GUI.Box(new Rect(startX - 100, startY + 80, 200, 50), "Connected controllers: " + gamepads.Count);

            }
        }
    }

}
