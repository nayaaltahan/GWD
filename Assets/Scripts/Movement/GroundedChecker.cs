using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GroundedChecker : MonoBehaviour
{

    public bool IsGrounded { get; private set; } = false;

    [SerializeField] 
    private Vector3 offset;

    [SerializeField] 
    private Vector3 halfExtents;

    [SerializeField] 
    private LayerMask collidedLayers;

    [SerializeField] 
    private QueryTriggerInteraction queryTriggerInteraction;

    private readonly Collider[] hit = new Collider[1];

    private int hitAmount;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hitAmount = Physics.OverlapBoxNonAlloc(transform.position + offset, halfExtents, hit, Quaternion.identity, collidedLayers,
            queryTriggerInteraction);
        IsGrounded = hitAmount > 1;
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Gizmos.DrawWireCube(transform.position  + offset, halfExtents * 2);
    }
}
