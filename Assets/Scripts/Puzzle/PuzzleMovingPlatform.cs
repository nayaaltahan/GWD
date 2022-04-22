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

    Vector3 direction;
    Vector3 velocity = new Vector3(0.0f, 3.0f, 0.0f);
    private bool moving = false;

	Dictionary<Transform, bool> passengers = new Dictionary<Transform, bool>();
    
    private void Start()
    {
        direction = target.position - transform.position;
        startPosition = transform.position;
    }

    //private void Update()
    //{
    //    if(moving)
    //    {
    //        transform.position += direction * pressedDuration; 
    //    }    
    //}

    public override void Interact()
    {
        moving = true;
        StartCoroutine(MovePlatform(target.position, pressedDuration));
    }

    public override void OnPlateRelease() 
    {
        StopAllCoroutines();
        StartCoroutine(MovePlatform(startPosition, pressedDuration));
        moving = false;
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

        Debug.Log("Exit");
        collision.gameObject.transform.parent = null;
    }

    IEnumerator MovePlatform(Vector3 _target, float velocity)
    {
        Vector3 startPosition = transform.position;
        float time = 0f;

        while (transform.position != _target)
        {
            transform.position = Vector3.Lerp(startPosition, _target, (time / Vector3.Distance(startPosition, _target)) * velocity);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
