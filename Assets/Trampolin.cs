using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampolin : MonoBehaviour
{
    [SerializeField] private float force;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag(Constants.PLAYER))
        {
            ApplyForce(collision.rigidbody);
        }
    }

    private void ApplyForce(Rigidbody rb)
    {
        Debug.Log("Mushroom Force");
        rb.AddForce(rb.transform.up * force, ForceMode.Impulse);
    }
}
