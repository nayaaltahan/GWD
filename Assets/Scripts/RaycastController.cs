using UnityEngine;
using System.Collections;

public class RaycastController : MonoBehaviour {

    public LayerMask collisionMask;
	
    public const float skinWidth = .015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    [HideInInspector]
    public float horizontalRaySpacing;
    [HideInInspector]
    public float verticalRaySpacing;

    [HideInInspector]
    public Collider collider;
    public RaycastOrigins raycastOrigins;

    public virtual void Start() {
        collider = GetComponentInChildren<Collider> ();
        CalculateRaySpacing ();
    }

    public void UpdateRaycastOrigins() {
        Bounds bounds = collider.bounds;
        bounds.Expand (skinWidth * -2);
		
        raycastOrigins.bottomLeft = new Vector3 (bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector3 (bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector3 (bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector3 (bounds.max.x, bounds.max.y);
    }
	
    public void CalculateRaySpacing() {
        Bounds bounds = collider.bounds;
        bounds.Expand (skinWidth * -2);
		
        horizontalRayCount = Mathf.Clamp (horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp (verticalRayCount, 2, int.MaxValue);
		
        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }
	
    public struct RaycastOrigins {
        public Vector3 topLeft, topRight;
        public Vector3 bottomLeft, bottomRight;
    }
}