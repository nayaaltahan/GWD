using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private Transform playerSpawn;

    private static int amountPlayers;
    public int playerNumber;

    // Start is called before the first frame update
    void Awake()
    {
        amountPlayers += 1;
        playerNumber = amountPlayers;
    }

    //Activate Children
    private void PlayGame(bool activateModel)
    {
        transform.GetChild(0).gameObject.SetActive(activateModel);
    }

    //TODO: Implement this
    public void SetUpPlayer(bool activateModel)
    {
        PlayGame(activateModel);
        //TODO: Set this as spawn point
        transform.position = playerSpawn.position;
        //Add differences here for players
    }
}
