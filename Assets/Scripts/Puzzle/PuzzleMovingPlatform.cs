using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PuzzleMovingPlatform : PuzzleObject
{
	private  Vector3 startPosition;
    [SerializeField] private Vector3 distance;
    [SerializeField] private Transform target;
    [SerializeField] private float pressedDuration;
    [SerializeField] private float returningDuration;

    Vector3 velocity = new Vector3(0.0f, 3.0f, 0.0f);
    private bool moving = false;
    Sequence sequence;

	Dictionary<Transform, MovementController> passengerDictionary = new Dictionary<Transform, MovementController>();

	Dictionary<Transform, bool> passengers = new Dictionary<Transform, bool>();
    
    private void Start()
    {
        sequence = DOTween.Sequence();
        startPosition = transform.position;
    }

    public override void Interact()
    {
        sequence.Kill();

        if (target)
            sequence.Append(transform.DOMove(target.position, pressedDuration).SetEase(Ease.InSine));
        else
            sequence.Append(transform.DOMove(transform.position + distance, returningDuration).SetEase(Ease.InSine));

        moving = true;
    }

    public override void OnPlateRelease() 
    {
        moving = false;
        sequence.Append(transform.DOMove(startPosition, returningDuration).SetEase(Ease.InSine));
        sequence.Kill();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != Constants.PLAYER)
            return;

        collision.gameObject.transform.parent = transform;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag != Constants.PLAYER)
            return;

        collision.gameObject.transform.parent = null;
    }
}
