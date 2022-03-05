using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;

public class PlayerUIController : MonoBehaviour
{
    // Start is called before the first frame update
    MultiplayerEventSystem multiplayerES;

    void Start()
    {
        SetSelections();
    }

    private void SetSelections()
    {
        //multiplayerES = GetComponent<MultiplayerEventSystem>();
        //if (GameManager.instance.state == GameStates.CHARACTERSELECT)
        //{
        //    GameObject midButton = CharSelectManager.instance.p1MidButton;
        //    GameObject root = CharSelectManager.instance.p1Root;

        //    if (GetComponent<PlayerController>().playerNumber == 2)
        //    {
        //        midButton = CharSelectManager.instance.p2MidButton;
        //        root = CharSelectManager.instance.p2Root;
        //    }

        //    multiplayerES.firstSelectedGameObject = midButton;
        //    multiplayerES.SetSelectedGameObject(midButton);
        //    multiplayerES.playerRoot = root;
        //}
    }
}
