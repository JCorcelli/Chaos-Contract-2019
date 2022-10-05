using UnityEngine;
using System.Collections;

namespace TestProject.Cameras
{
	public class PredictCollision : MonoBehaviour {
		
		public float proximityRadius = 1f;
		public float castRadius = 0.1f;
		public float responseTime = 2f;
		public float timeToCollision = 100f;
		public LayerMask activeLayer = 1 << 12 | 1 << 0; // ground & default
		public bool likelyCollision = false;
		
		private Vector3 deltaMovement;
		private Vector3 lastPosition;
		private Ray ray;
		private RaycastHit hit;
		
		
		void Start () {
			lastPosition = transform.position;
		}
		
		public RaycastHit GetHit() { return hit; }
		
		/// <summary>
		/// Predicts if it's about to hit something in the given responseTime
		/// </summary>
		protected void PredictCollisionInternal() {
			// check if you're about to hit something or close enough to something
			deltaMovement = transform.position - lastPosition;
			lastPosition = transform.position;
			
			ray = new Ray(transform.position, deltaMovement);
			if (Physics.CheckSphere(ray.origin, proximityRadius, activeLayer))
				likelyCollision = true;
			else if (Physics.SphereCast(ray, castRadius, out hit, Mathf.Infinity, activeLayer)) {
				
				timeToCollision = (hit.point - transform.position).magnitude / deltaMovement.magnitude * Time.deltaTime;
				
				if (timeToCollision < responseTime) likelyCollision = true;
				else 
					likelyCollision = false;
			}
			else 
				likelyCollision = false;
		}
		/// <summary>
		/// Returns the amount of time in seconds it would take to reach a single point given the current speed.
		/// </summary>
		
		public float ComparePoint (Vector3 vec){
			// returns float based on speed last frame, magnitude of point if invalid, 0 if near
			ray = new Ray(transform.position, vec - transform.position);
			
			if (Physics.CheckSphere(ray.origin, proximityRadius, activeLayer))
				return 0f;
			else if (Physics.SphereCast(ray, castRadius, out hit, vec.magnitude, activeLayer)) {
				
				timeToCollision = (hit.point - transform.position).magnitude / deltaMovement.magnitude * Time.deltaTime;
				
				return timeToCollision;
			}
			else
			{
				timeToCollision = vec.magnitude / deltaMovement.magnitude * Time.deltaTime;
				return timeToCollision;
			}
			
		}
				
		void FixedUpdate () {
				PredictCollisionInternal();
		}
	}
}
