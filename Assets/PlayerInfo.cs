using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Just a container to pass data for when we select a character
public class PlayerInfo : MonoBehaviour
{
    public PlayerInputController FrogInputController, RobotInputController;

    public GameObject[] FrogSpeechBubbles, RobotSpeechBubbles;

    public GameObject FrogSpeechIndicator;

    public GameObject RobotSpeechIndicator;

    public GameObject FrogOptionalSpeechIndicator;
    public GameObject RobotOptionalSpeechIndicator;

    public GameObject RobotModel;
    public GameObject RaniModel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
