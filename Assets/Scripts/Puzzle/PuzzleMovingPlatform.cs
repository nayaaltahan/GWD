using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PuzzleMovingPlatform : PuzzleObject
{
    private  Vector3 startPosition;
    [SerializeField] private Vector3 distance;
    [SerializeField] private Transform target;
    [SerializeField] private float pressedSpeed;
    [SerializeField] private float returningSpeed;

    private bool moving = false;
    Sequence sequence;

    private void Start()
    {
        sequence = DOTween.Sequence();
        startPosition = transform.position;
    }

    public override void Interact()
    {
        if (moving)
            return;

        sequence.Kill();

        if (target)
            sequence.Append(transform.DOMove(target.position, pressedSpeed).SetEase(Ease.InSine));
        else
            sequence.Append(transform.DOMove(transform.position + distance, pressedSpeed).SetEase(Ease.InSine)); 

        moving = true;
    }

    public override void OnPlateRelease() 
    {
        moving = false;
        sequence.Append(transform.DOMove(startPosition, returningSpeed).SetEase(Ease.InSine));
        sequence.Kill();
    }
}
