using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] 
    private DialogueManager dialogueManager;

    [SerializeField]
    private GameObject CharacterSelectUI;

    private bool showMainMenu = true;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.SwitchState(GameStates.MAINMENU);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevelWithTimeLimit()
    {
        Debug.Log("WIth time limit");
        dialogueManager.SetChoiceTimeLimit(2.0f);

        CharacterSelectUI.SetActive(true);
        if (!SceneManager.GetSceneByName("Swamp").isLoaded)
            SceneManager.LoadScene("Swamp", LoadSceneMode.Additive);
    }

    public void LoadLevelWithoutTimeLimit()
    {
        Debug.Log("WIthout time limit");

        CharacterSelectUI.SetActive(true);
        if (!SceneManager.GetSceneByName("Swamp").isLoaded)
            SceneManager.LoadScene("Swamp", LoadSceneMode.Additive);
    }

    private void OnGUI()
    {
        if (showMainMenu)
        {
            if(GUI.Button(new Rect(Screen.width / 2.0f, Screen.height / 2 - 100, 200, 100), "Play A"))
            {
                GameManager.instance.SwitchState(GameStates.CHARACTERSELECT);
                LoadLevelWithTimeLimit();
                showMainMenu = false;
            }
            else if (GUI.Button(new Rect(Screen.width / 2.0f, Screen.height / 2 + 200, 200, 100), "Play B"))
            {
                GameManager.instance.SwitchState(GameStates.CHARACTERSELECT);
                LoadLevelWithoutTimeLimit();
                showMainMenu = false;
            }
        }
    }

}
