using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : RaycastController
{
    public LayerMask passengerMask;
	public Vector3 move;
	
	public override void Start () {
		base.Start ();
	}

	void Update () {

		UpdateRaycastOrigins ();

		Vector3 velocity = move * Time.deltaTime;

		MovePassengers(velocity);
		transform.Translate (velocity);
	
	}

	void MovePassengers(Vector3 velocity) {
		HashSet<Transform> movedPassengers = new HashSet<Transform> ();

		float directionX = Mathf.Sign (velocity.x);
		float directionY = Mathf.Sign (velocity.y);

		// Vertically moving platform
		if (velocity.y != 0) {
			float rayLength = Mathf.Abs (velocity.y) + skinWidth;
			
			for (int i = 0; i < verticalRayCount; i ++) {
				Vector3 rayOrigin = (directionY == -1)?raycastOrigins.bottomLeft:raycastOrigins.topLeft;
				rayOrigin += Vector3.right * (verticalRaySpacing * i);

				if (Physics.Raycast(rayOrigin, Vector3.up * directionY, out var hit, rayLength, passengerMask)) {
					if (!movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add(hit.transform);
						float pushX = (directionY == 1)?velocity.x:0;
						float pushY = velocity.y - (hit.distance - skinWidth) * directionY;

						hit.transform.Translate(new Vector3(pushX,pushY));
					}
				}
			}
		}

		// Horizontally moving platform
		if (velocity.x != 0) {
			float rayLength = Mathf.Abs (velocity.x) + skinWidth;
			
			for (int i = 0; i < horizontalRayCount; i ++) {
				Vector3 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
				rayOrigin += Vector3.up * (horizontalRaySpacing * i);

				if (Physics.Raycast(rayOrigin, Vector3.right * directionX, out var hit,  rayLength, passengerMask)) {
					if (!movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add(hit.transform);
						float pushX = velocity.x - (hit.distance - skinWidth) * directionX;
						float pushY = 0;
						
						hit.transform.Translate(new Vector3(pushX,pushY));
					}
				}
			}
		}

		// Passenger on top of a horizontally or downward moving platform
		if (directionY == -1 || velocity.y == 0 && velocity.x != 0) {
			float rayLength = skinWidth * 2;
			
			for (int i = 0; i < verticalRayCount; i ++) {
				Vector3 rayOrigin = raycastOrigins.topLeft + Vector3.right * (verticalRaySpacing * i);
				
				if (Physics.Raycast(rayOrigin, Vector3.up, out var hit, rayLength, passengerMask)) {
					if (!movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add(hit.transform);
						float pushX = velocity.x;
						float pushY = velocity.y;
						
						hit.transform.Translate(new Vector3(pushX,pushY));
					}
				}
			}
		}
	}
}
