using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicParticleController : MonoBehaviour
{

    GameObject magicThing;
    [SerializeField] float playDistance = 15f;
    ParticleSystem system;
    bool playing = false;
    Animator magicThingAnim;

    // Start is called before the first frame update
    void Start()
    {
        magicThing = GameObject.FindGameObjectWithTag("MagicRobotThing");
        magicThingAnim = magicThing.GetComponent<Animator>();
        system = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (magicThingAnim.isActiveAndEnabled)
        {
            if (Vector3.Distance(transform.position, magicThing.transform.position) < playDistance && !playing)
            {
                Debug.Log((Vector3.Distance(transform.position, magicThing.transform.position)));
                playing = true;
                system.Play(true);
            }
            else if (Vector3.Distance(transform.position, magicThing.transform.position) > playDistance && playing)
            {
                system.Stop(false, ParticleSystemStopBehavior.StopEmitting);
                playing = false;
            }
        }
        
    }
}
