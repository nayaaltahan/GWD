using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStates { MAINMENU, CHARACTERSELECT, PAUSEMENU, PLAYGAME }
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameStates state;

    //TODO: Create a UI manager, instead of having variables being managed in GameManager
    public GameObject canvas;

    private GameObject playerOne;
    private GameObject playerTwo;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Debug.LogError("More than one Game Manager in the scene");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchState(GameStates state)
    {
        switch (state)
        {
            case GameStates.MAINMENU:
                break;
            case GameStates.CHARACTERSELECT:
                break;
            case GameStates.PAUSEMENU:
                break;
            case GameStates.PLAYGAME:
                //Turn off any U.I. or objects that don't belong to PlayGame State
                break;
            default:
                break;
        }
    }
}
