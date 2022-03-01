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
        multiplayerES = GetComponent<MultiplayerEventSystem>();
        if (GameManager.instance.state == GameStates.CHARACTERSELECT)
        {
            GameObject midButton = CharSelectManager.instance.p1MidButton;
            if (GetComponent<PlayerController>().playerNumber == 2)
            {
                midButton = CharSelectManager.instance.p2MidButton;
                Debug.Log("MAGIC");
            }

            multiplayerES.firstSelectedGameObject = midButton;
            multiplayerES.playerRoot = GameManager.instance.canvas;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
