using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BreakablePlatform : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How long the platform takes to break in seconds")]
    private float breakingTime = 2f;

    [SerializeField]
    [Tooltip("How much shake is applied to each axis")]
    private Vector3 shakeStrength = Vector3.zero;

    [SerializeField]
    [Tooltip("How long the platform takes to respawn")]
    private float respawnTime = 2f;

  

    private bool isBreaking = false;
    private Vector3 initialPosition;
    private MeshRenderer mr;
    private Collider collider;
    private void Start()
    {
        mr = GetComponent<MeshRenderer>();
        collider = GetComponent<Collider>();
        initialPosition = transform.position;
    }

    public IEnumerator StartBreak()
    {
        // TODO: SOUND add breaking sound
        isBreaking = true;
        transform.DOShakePosition(breakingTime, shakeStrength, 10, 90, false, false);
        yield return new WaitForSeconds(breakingTime);

        // TODO: Maybe network this enable / disable
        // Destroy
        collider.enabled = false;
        mr.enabled = false;
        yield return new WaitForSeconds(respawnTime);
        // Respawn
        transform.position = initialPosition;
        isBreaking = false;
        mr.enabled = true;
        collider.enabled = true;

    }
    private void OnCollisionEnter(Collision collision)
    {
        // TODO: If big character, start breaking
        if (!isBreaking)
        {
            StartCoroutine(StartBreak());
        }
    }
}
