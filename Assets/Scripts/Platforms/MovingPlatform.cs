using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How far will the platform move from the original position")]
    private Vector3 movementOffset = Vector3.zero;

    [SerializeField]
    [Tooltip("How long does it take the platform to move to its destination (in seconds)")]
    private float moveTime = 2f;

    [SerializeField]
    [Tooltip("How long does it take the platform to move again once it arrives at the destination (seconds)")]
    private float moveDelay = 0.5f;

    private Vector3 initialPosition;


    private void MovePlatform(Vector3 targetPos)
    {
        transform.DOMove(targetPos, moveTime).SetEase(Ease.InSine);
    }

    private IEnumerator StartPlatform()
    {
        while (true)
        {
            var target = transform.position == initialPosition ? transform.position + movementOffset : initialPosition;
            MovePlatform(target);
            yield return new WaitForSeconds(moveTime + moveDelay);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        StartCoroutine(StartPlatform());
    }
}
