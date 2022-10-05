using UnityEngine;
using System.Collections;

namespace TestProject.Cameras
{
	public class CAUtilityMethods : CameraAssistant
	{
		
		
		public float moveSpeed = 1f;
		public float moveBoosterThreshold = 0f; 
		public float moveBooster = 1f; 
		
		public float limitDistance = 10f;
		public float limitProximity = 10f;
		
		
		public float lookSpeed = 1f;
		public float lookBoosterThresholdAngles = 0f; 
		public float lookBooster = 1f; 
		
		
		// inherits public Transform target;
		
		public Transform secondTarget;
		
		public float occlusionThreshold = 1f;
		public float birdEyeHeight = 5f;
		
		// additional utility options
        public float sphereCastRadius = 0.1f;
		public LayerMask avoidLayers = 1 << 12 | 1 << 0;
		
		
		
		// Basic movement
		protected virtual void  LookAt () {
			transform.LookAt(secondTarget);
			
			
		}
		
		
		protected virtual float BoostThis(float speed, float compare, float threshold, float boost ) {
			float currentSpeed;
			if (compare < threshold)
				currentSpeed = speed;
			else
				currentSpeed = lookBooster * (1 + (compare  - threshold) ) * speed;
			
			
			return currentSpeed;
		}
		protected virtual void  LookAt (bool dampen) {
			Quaternion rotate = Quaternion.LookRotation(secondTarget.position - transform.position);
			
			float currentSpeed = BoostThis(lookSpeed, rotate.eulerAngles.magnitude / 180f, lookBoosterThresholdAngles / 180f, lookBooster);
			
			transform.rotation = Quaternion.Slerp	(transform.rotation, rotate, Time.deltaTime * currentSpeed);
			
			
		}
		
		protected virtual void  Move () {
			transform.position = target.position;
		}
		protected virtual void  Move (bool dampen) {
			Vector3 difference = target.position - transform.position;
			
			float currentSpeed = BoostThis(moveSpeed, difference.magnitude, moveBoosterThreshold, moveBooster);
			
			Vector3 adjusted = Vector3.Lerp	(Vector3.zero, difference, Time.deltaTime * currentSpeed);
			
			transform.position = transform.position + adjusted;
		}
		protected virtual void  Move (bool dampen, bool proxy) {
			Vector3 difference = target.position - transform.position;
			float distance = difference.magnitude;
			
			if (distance < limitDistance && distance > 
			limitProximity) return;

			difference = MoveNear ( true ) - transform.position;
			
			float currentSpeed = BoostThis(moveSpeed, difference.magnitude, moveBoosterThreshold, moveBooster);
			
			Vector3 adjusted = Vector3.Lerp	(Vector3.zero, difference, Time.deltaTime * currentSpeed);
			
			transform.position = transform.position + adjusted;
		}
		protected virtual void  MoveNear () {
			// this assumes the camera is not on a pivot and attempts to maintain distance?
			Vector3 difference = target.position - transform.position;
			
			Vector3 direction = difference.normalized;
			transform.position = target.position - direction * limitProximity;
		}
		protected virtual Vector3  MoveNear (bool proxy = false) {
			// this assumes the camera is not on a pivot and attempts to maintain distance?
			Vector3 difference = target.position - transform.position;
			Vector3 direction = difference.normalized;
			Vector3 position = target.position - direction * limitDistance;
			if (!proxy)
				transform.position = position;
			return position;
		}
		
		// Checks angles and distance for cinematic shot
		protected virtual void  DecideAngle() {
			// default it tries to rotate away from the wall when it bumps a wall
			// going to try using a normal spherecollider
			
		}
		
		
		// vars for zoomtofit
		private float boundSize;
		
		public float temp_zoomThreshold = 0.5f;
		public float temp_zoomSpeed = 5f;
		
		
		protected virtual void  ZoomToFit() {
			// default it tries to find the minimum distance two objects fit
			
			
			// calculate bounds?
			
			// center of two objects
			Vector3 between = (target.position + secondTarget.position) /2f;
			Bounds bounds = new Bounds(between, new Vector3(1,1,1));
			bounds.Encapsulate(target.position);
			bounds.Encapsulate(secondTarget.position);
			
			// eh
			boundSize = Mathf.Max(bounds.extents.x, bounds.extents.y, bounds.extents.z);
			
			
			// get FOV (from this position)?
			// distance from this to the center of two
			float dist = (between - transform.position).magnitude;
			
            float requiredFOV = Mathf.Atan2(boundSize, dist)*Mathf.Rad2Deg; // * zoomMultiplier

			
			float fovDiff = Mathf.Abs(requiredFOV - Camera.main.fieldOfView);
			if (fovDiff > temp_zoomThreshold)
				transform.position -= transform.forward * temp_zoomSpeed;
			else
				transform.position += transform.forward * temp_zoomSpeed;
				
			// get FOV? 
			// edit: actually, I want to move the camera to a new position and avoid changing the FOV.
            // Camera.main.fieldOfView = Mathf.SmoothDamp(Camera.main.fieldOfView, requiredFOV, ref m_FovAdjustVelocity, m_FovAdjustTime);
			
		}
		protected virtual void  RotateToFit() {
			// default rotates around target until it sees second target
			// I could also have a default zoom distance
			// public float sphereCastRadius = 0.1f;
			// avoidLayers
		}
		protected virtual void  BirdEye() {
			// default seeks a height and pans to center target
			// public float birdEyeHeight = 5f;
			
		}
		
		// Predict Occluder
		protected virtual void PredictOccluder() {
			// raycasts ahead of itself and targets several frames
			// public float occlusionThreshold = 1f;
			// avoidLayers
			// target velocity?
		}
		
		// Player input
		protected virtual void AdjustToInput() {
			// makes minor adjustments based on input. e.g. zoom distance ++
			
		}
		
	}
}