using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] childrenToActivate;

    private static int amountPlayers;
    public int playerNumber;

    // Start is called before the first frame update
    void Awake()
    {
        amountPlayers += 1;
        playerNumber = amountPlayers;
    }

    //Activate Children
    private void PlayGame()
    {
        foreach (var child in childrenToActivate)
        {
            child.SetActive(true);
        }
    }

    //TODO: Implement this
    public void SetUpPlayer(bool isPlayerOne)
    {
        PlayGame();
        //TODO: Set this as spawn point
        GetComponent<Rigidbody>().position = Vector3.zero;
        //Add differences here for players
    }
}
