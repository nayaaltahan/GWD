using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MovementController : MonoBehaviour
{
    public LayerMask collisionMask;
    
    private const float skinWidth = .015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    private float horizontalRaySpacing;
    private float verticalRaySpacing;

    private CapsuleCollider collider;
    private RaycastOrigins raycastOrigins;
    
    public CollisionInfo collisions;

    private void Start()
    {
        collider = GetComponentInChildren<CapsuleCollider>();
        CalculateRaySpacing();
    }
    
    public void Move(Vector3 velocity) {
        UpdateRaycastOrigins ();
        collisions.Reset();
        if (velocity.x != 0) {
            HorizontalCollisions (ref velocity);
        }
        if (velocity.y != 0) {
            VerticalCollisions (ref velocity);
        }

        transform.Translate (velocity);
    }
    
    void HorizontalCollisions(ref Vector3 velocity) {
        float directionX = Mathf.Sign (velocity.x);
        float rayLength = Mathf.Abs (velocity.x) + skinWidth;
		
        for (int i = 0; i < horizontalRayCount; i ++) {
            Vector3 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
            rayOrigin += Vector3.up * (horizontalRaySpacing * i);

            Debug.DrawRay(rayOrigin, Vector3.right * directionX * rayLength,Color.red);

            if (Physics.Raycast(rayOrigin, Vector3.right * directionX, out var hit, rayLength, collisionMask)) {
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;
                
                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
            }
        }
    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;
        
        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector3 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector3.right * (verticalRaySpacing * i + velocity.x);
            Debug.DrawRay(raycastOrigins.bottomLeft + Vector3.right * verticalRaySpacing * i, Vector3.up * -2, Color.red);
            
            if (Physics.Raycast(rayOrigin, Vector3.up * directionY, out var hit,  rayLength, collisionMask)) {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;
                
                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);
        
        raycastOrigins.bottomLeft = new Vector3(bounds.min.x, bounds.min.y, bounds.center.z);
        raycastOrigins.bottomRight = new Vector3(bounds.max.x, bounds.min.y, bounds.center.z);
        raycastOrigins.topLeft = new Vector3(bounds.min.x, bounds.max.y, bounds.center.z);
        raycastOrigins.topRight = new Vector3(bounds.max.x, bounds.max.y, bounds.center.z);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    struct RaycastOrigins 
    { 
        public Vector3 topLeft, topRight;
        public Vector3 bottomLeft, bottomRight;
    }
    
    public struct CollisionInfo {
        public bool above, below;
        public bool left, right;

        public void Reset() {
            above = below = false;
            left = right = false;
        }
    }
}


