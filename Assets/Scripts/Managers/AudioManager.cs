using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Destroying duplicate Audio Manager", gameObject);
            Destroy(this);
        }
    }


    public void PlayOneShot(string audioEvent)
    {
        FMODUnity.RuntimeManager.PlayOneShot(audioEvent);
    }

    public void Play3DOneShot(string audioEvent, Vector3 position)
    {
        FMODUnity.RuntimeManager.PlayOneShot(audioEvent, position);
    }

    public void StopOneShot(string audioEvent)
    {

    }
    public FMOD.Studio.EventInstance CreateEventInstance(string audioEvent)
    {
        return FMODUnity.RuntimeManager.CreateInstance(audioEvent);
    }

    public FMOD.Studio.EventInstance Create3DEventInstance(string audioEvent, GameObject gameObject)
    {
        var instance = FMODUnity.RuntimeManager.CreateInstance(audioEvent);
        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        return instance;
    }
}
