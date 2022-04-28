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
    public List<PuzzleInteractible> puzzleInteractables;

    Vector3 direction;
    Vector3 velocity = new Vector3(0.0f, 3.0f, 0.0f);
    private bool moving = false;
    private Coroutine coroutine;

	Dictionary<Transform, bool> passengers = new Dictionary<Transform, bool>();

    public bool Moving { get => moving; set => moving = value; }

    private void Awake()
    {
        direction = target.position - transform.position;
        startPosition = transform.position;

        for (int i = 0; i < puzzleInteractables.Count; i++)
        {
            puzzleInteractables[i].PuzzleObject = this;
        }
    }


    public override void Interact()
    {
        //StopAllCoroutines();
        if(coroutine != null)
            StopCoroutine(coroutine);

        Moving = true;
        coroutine = StartCoroutine(MovePlatform(target.position, pressedDuration));
    }

    public override void OnPlateRelease() 
    {
        StopAllCoroutines();
        coroutine = StartCoroutine(MovePlatform(startPosition, returningDuration));
        Moving = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != Constants.PLAYER)
            return;

        other.gameObject.transform.parent = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ENTER");
        if (other.gameObject.tag != Constants.PLAYER)
            return;

       other.gameObject.transform.parent = transform;
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
