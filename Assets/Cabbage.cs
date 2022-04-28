using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabbage : MonoBehaviour
{

    public float maxActiveTime = 30f;
    Rigidbody rb;
    bool onBelt = false;
    Vector3 beltVelocity = new Vector3();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        StartCoroutine(Deactivate());
    }


    private void Update()
    {
        if (onBelt)
            rb.velocity = beltVelocity;
    }
    IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(maxActiveTime);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void Respawn(Vector3 position, Vector3 velocity)
    {
        gameObject.SetActive(true);
        rb.velocity = velocity;
        transform.position = position;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CabbageDestroyer"))
        {
            StopAllCoroutines();
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Springboard"))
        {
            Springboard board = collision.collider.gameObject.GetComponent<Springboard>();
            rb.velocity = board.GetVelocity();
        }

        if (collision.collider.CompareTag("Conveyor"))
        {
            onBelt = true;
            beltVelocity = collision.collider.gameObject.GetComponent<Conveyor>().GetVelocity();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Conveyor"))
        {
            onBelt = false;
        }
    }
}
