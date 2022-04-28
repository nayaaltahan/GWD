using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Boat : MonoBehaviour
{
    [SerializeField] private Transform targetPos;
    [SerializeField] private float time;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Constants.PLAYER))
        {
            startTravel();
        }
    }

    private void startTravel()
    {
        Debug.Log("MOVE BOAT");
        transform.DOMove(targetPos.position, time).SetEase(Ease.InSine);
    }
}
