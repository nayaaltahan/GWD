using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MovementController : RaycastController
{
    float maxClimbAngle = 80;
	float maxDescendAngle = 80;
	
	public CollisionInfo collisions;

	public LayerMask wallCollisionMask;

	float springboardMinSpeed = 0.5f;



	public override void Start() {
		base.Start ();
		collisions.faceDir = 1;
	}

	public void Move(Vector3 velocity, bool standingOnPlatform = false) {
		UpdateRaycastOrigins ();
		collisions.Reset ();
		collisions.velocityOld = velocity;
		
		if (velocity.x != 0) {
			collisions.faceDir = (int)Mathf.Sign(velocity.x);
		}

		if (velocity.y < 0) {
			DescendSlope(ref velocity);
		}
		if (velocity.x != 0) {
			HorizontalCollisions (ref velocity);
			WallCollisions(ref velocity);
		}
		if (velocity.y != 0) {
			VerticalCollisions (ref velocity);
		}

		transform.Translate (velocity);
		
		if (standingOnPlatform) {
			collisions.below = true;
		}

		Debug.DrawRay(transform.position, Vector3.down * 0.5f, Color.blue);
		if (Physics.Raycast(transform.position, Vector3.down, out var hit, 0.5f, collisionMask, QueryTriggerInteraction.Ignore))
			transform.parent = null;
		else
			transform.parent = null;

	}

	void HorizontalCollisions(ref Vector3 velocity) {
		float directionX = collisions.faceDir;
		float rayLength = Mathf.Abs (velocity.x) + skinWidth;
		
		if (Mathf.Abs(velocity.x) < skinWidth) {
			rayLength = 2*skinWidth;
		}
		
		for (int i = 0; i < horizontalRayCount; i ++) {
			Vector3 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
			rayOrigin += Vector3.up * (horizontalRaySpacing * i);

			Debug.DrawRay(rayOrigin, Vector3.right * directionX * rayLength,Color.red);

			if (Physics.Raycast(rayOrigin, Vector3.right * directionX, out var hit, rayLength, collisionMask, QueryTriggerInteraction.Ignore)) {
				Debug.DrawLine(transform.position, hit.point, Color.cyan, 5);

				if (hit.distance == 0) {
					continue;
				}
				
				float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

				if (i == 0 && slopeAngle <= maxClimbAngle) {
					if (collisions.descendingSlope) {
						collisions.descendingSlope = false;
						velocity = collisions.velocityOld;
					}
					float distanceToSlopeStart = 0;
					if (slopeAngle != collisions.slopeAngleOld) {
						distanceToSlopeStart = hit.distance-skinWidth;
						velocity.x -= distanceToSlopeStart * directionX;
					}
					ClimbSlope(ref velocity, slopeAngle);
					velocity.x += distanceToSlopeStart * directionX;
				}

				if (!collisions.climbingSlope || slopeAngle > maxClimbAngle) {
					velocity.x = (hit.distance - skinWidth) * directionX;
					rayLength = hit.distance;

					if (collisions.climbingSlope) {
						velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
					}

					collisions.left = directionX == -1;
					collisions.right = directionX == 1;
				}
			}
		}
	}
	
	void WallCollisions(ref Vector3 velocity) {
		float directionX = collisions.faceDir;
		float rayLength = Mathf.Abs (velocity.x) + skinWidth;
		
		if (Mathf.Abs(velocity.x) < skinWidth) {
			rayLength = 2*skinWidth;
		}
		
		for (int i = 0; i < horizontalRayCount; i ++) {
			Vector3 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
			rayOrigin += Vector3.up * (horizontalRaySpacing * i);

			Debug.DrawRay(rayOrigin, Vector3.right * directionX * rayLength,Color.red);

			if (Physics.Raycast(rayOrigin, Vector3.right * directionX, out var hit, rayLength, wallCollisionMask, QueryTriggerInteraction.Ignore)) {
				Debug.DrawLine(transform.position, hit.point, Color.blue, 5);

				if (hit.distance == 0) {
					continue;
				}
				
				float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

				if (!collisions.climbingSlope || slopeAngle > maxClimbAngle) {
					collisions.leftWall = directionX == -1;
					collisions.rightWall = directionX == 1;
				}
			}
		}
	}

	void VerticalCollisions(ref Vector3 velocity) {
		float directionY = Mathf.Sign (velocity.y);
		float rayLength = Mathf.Abs (velocity.y) + skinWidth;
		Vector3 springVelocity = Vector3.zero;
		for (int i = 0; i < verticalRayCount; i ++) {
			Vector3 rayOrigin = (directionY == -1)?raycastOrigins.bottomLeft:raycastOrigins.topLeft;
			rayOrigin += Vector3.right * (verticalRaySpacing * i + velocity.x);

			Debug.DrawRay(rayOrigin, Vector3.up * directionY * rayLength,Color.red);

			if (Physics.Raycast(rayOrigin, Vector3.up * directionY, out var hit, rayLength, collisionMask, QueryTriggerInteraction.Ignore))
			{
				Debug.DrawLine(transform.position, hit.point, Color.magenta, 5);
				velocity.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;

				if (collisions.climbingSlope)
				{
					velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
				}

				collisions.below = directionY == -1;
				collisions.above = directionY == 1;

				if (hit.collider.gameObject.GetComponent<PuzzleInteractible>())
				{
					if (puzzleInteractible != hit.collider.gameObject.GetComponent<PuzzleInteractible>())
					{
						puzzleInteractible = hit.collider.gameObject.GetComponent<PuzzleInteractible>();
						puzzleInteractible.Pressed = true;
					}

					if (puzzleInteractible == hit.collider.gameObject.GetComponent<PuzzleInteractible>())
						puzzleInteractible.Interact();
				}
				else
                {
					if (puzzleInteractible)
					{
						puzzleInteractible.Pressed = false;
						puzzleInteractible = null;
					}
                }

				if (hit.collider.gameObject.CompareTag(Constants.SPRINGBOARD))
				{
					Debug.Log("SPRINGBOARD" + hit.collider.GetComponent<Springboard>().GetVelocity());
					springVelocity = hit.collider.GetComponent<Springboard>().GetVelocity();
					PlayerStateController playerState = GetComponent<PlayerStateController>();

					if (playerState.velocity.y < -springboardMinSpeed)
						playerState.Springboard(springVelocity);

					return;
				}
			}
		}

		if (collisions.climbingSlope) {
			float directionX = Mathf.Sign(velocity.x);
			rayLength = Mathf.Abs(velocity.x) + skinWidth;
			Vector3 rayOrigin = ((directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight) + Vector3.up * velocity.y;

			if (Physics.Raycast(rayOrigin,Vector3.right * directionX, out var hit, rayLength,collisionMask, QueryTriggerInteraction.Ignore)) {
				float slopeAngle = Vector3.Angle(hit.normal,Vector3.up);
				if (slopeAngle != collisions.slopeAngle) {
					velocity.x = (hit.distance - skinWidth) * directionX;
					collisions.slopeAngle = slopeAngle;
				}
			}
		}
	}

	void ClimbSlope(ref Vector3 velocity, float slopeAngle) {
		float moveDistance = Mathf.Abs (velocity.x);
		float climbVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;

		if (velocity.y <= climbVelocityY) {
			velocity.y = climbVelocityY;
			velocity.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (velocity.x);
			collisions.below = true;
			collisions.climbingSlope = true;
			collisions.slopeAngle = slopeAngle;
		}
	}

	void DescendSlope(ref Vector3 velocity) {
		float directionX = Mathf.Sign (velocity.x);
		Vector3 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;

		if (Physics.Raycast (rayOrigin, -Vector3.up, out var hit, Mathf.Infinity, collisionMask)) {
			float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
			if (slopeAngle != 0 && slopeAngle <= maxDescendAngle) {
				if (Mathf.Sign(hit.normal.x) == directionX) {
					if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x)) {
						float moveDistance = Mathf.Abs(velocity.x);
						float descendVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;
						velocity.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (velocity.x);
						velocity.y -= descendVelocityY;

						collisions.slopeAngle = slopeAngle;
						collisions.descendingSlope = true;
						collisions.below = true;
					}
				}
			}
		}
	}



	public struct CollisionInfo {
		public bool above, below;
		public bool left, right;
		public bool leftWall, rightWall;

		public bool climbingSlope;
		public bool descendingSlope;
		public float slopeAngle, slopeAngleOld;
		public Vector3 velocityOld;
		public int faceDir;

		public void Reset() {
			above = below = false;
			left = right = false;
			climbingSlope = false;
			descendingSlope = false;

			slopeAngleOld = slopeAngle;
			slopeAngle = 0;
		}
	}
}


