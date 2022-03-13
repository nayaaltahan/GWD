using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using TMPro;
using DG.Tweening;

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
    [Header("Debug")]
    // TODO: Spawn only 1 player when this is true
    [SerializeField] private bool allowSinglePlayer = false;

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
                    tempString = "Ready";
            }
            else
                p1Buttons[i].enabled = !isP1Ready;

            p1Buttons[i].transform.GetChild(0).GetComponent<TMP_Text>().text = tempString;
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
                    tempString = "Ready";
            }
            else
                p2Buttons[i].enabled = !isP2Ready;

            p2Buttons[i].transform.GetChild(0).GetComponent<TMP_Text>().text = tempString;
        }

        Ready();
    }

    //Check if game is Ready to play, start PlayState and setup players
    public void Ready()
    {
        if ((isP1Ready == false || isP2Ready == false))
            return;

        GameManager.instance.SwitchState(GameStates.PLAYGAME);
        GameManager.instance.playerOne.GetComponent<PlayerController>().SetUpPlayer(isP1Left);
        GameManager.instance.playerTwo.GetComponent<PlayerController>().SetUpPlayer(isP2Left);
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
            p1Buttons[i].transform.GetChild(0).gameObject.SetActive(temp);
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
            p2Buttons[i].transform.GetChild(0).gameObject.SetActive(temp);
        }
    }
}
