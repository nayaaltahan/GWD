using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopTube : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Tube")
        {
            Debug.Log("TUBE");
            other.GetComponent<Rigidbody>().mass = 100000.0f;
        }
    }
}
