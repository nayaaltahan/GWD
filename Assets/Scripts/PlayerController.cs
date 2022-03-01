using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static int amountPlayers;
    public int playerNumber;
    // Start is called before the first frame update
    void Start()
    {
        amountPlayers += 1;
        playerNumber = amountPlayers;
    }

}
