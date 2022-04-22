using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : MonoBehaviour
{
    [SerializeField] private float force;
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(Constants.PLAYER))
            return;

        if (other.GetComponent<PlayerStateController>().InputController.MoveDirection.x != 0)
            return;
        Debug.Log("PUSHING");

        if (other.GetComponent<PlayerStateController>().InputController.MoveDirection.x != 0)
        {
            other.GetComponent<PlayerStateController>().Animations.SetBool(Constants.PUSHING, true);
        }
        else
        {
            other.GetComponent<PlayerStateController>().Animations.SetBool(Constants.PUSHING, false);
        }

        Vector3 dir = other.GetComponent<PlayerStateController>().InputController.MoveDirection;

        var f = other.GetComponent<PlayerStateController>().InputController.MoveDirection * force;
        Debug.Log($"Pushing object {other.gameObject.name} with force {f}");
        GetComponentInParent<Rigidbody>()?.AddForce(f);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(Constants.PLAYER))
            return;

        other.GetComponent<PlayerStateController>().Animations.SetBool(Constants.PUSHING, false);
    }
}
