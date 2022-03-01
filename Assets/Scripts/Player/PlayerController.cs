using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static int amountPlayers;
    public int playerNumber;
    // Start is called before the first frame update
    void Awake()
    {
        amountPlayers += 1;
        playerNumber = amountPlayers;

        if (amountPlayers == 1)
        {
            GameManager.instance.playerOne = gameObject;
            CharSelectManager.instance.SetP1MES();
        }
        else
        {
            GameManager.instance.playerTwo = gameObject;
            CharSelectManager.instance.SetP2MES();
        }
    }

    private void Start()
    {
        if (GameManager.instance.state == GameStates.PLAYGAME)
            PlayGame();
    }

    public void PlayGame()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
