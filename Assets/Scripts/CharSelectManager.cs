using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CharSelectManager : MonoBehaviour
{
    public static CharSelectManager instance;

    public GameObject p1MidButton;
    public GameObject p2MidButton;

    [SerializeField] private PlayerInputManager playerInputManager;

    int playersReady = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Debug.LogError("More than one Character Selection Manager in the scene");
    }

    public void Play()
    {
       
    }

  public void Ready()
    {
        playersReady += 1;

        if(playersReady > 1)
        {
            GameManager.instance.SwitchState(GameStates.PLAYGAME);
        }
    }
}
