using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{

    public FMODUnity.EventReference stepEvent;


    // Start is called before the first frame update
    public void Step()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(stepEvent.Guid, gameObject);
        Debug.Log("TookStep", gameObject);
    }

}
