using UnityEngine;
using System.Collections;

namespace TestProject 
{
	public class RotateTowardsRing2 : MonoBehaviour {
		public Transform target;
		
		public float speed = 3f;
		
		public float accel = 1f; 
		public float decel = 2f; 
		public float stopDistance = 10f;
		
		public float maxSpeed = 10f;
		
		protected float currentVelocity = 0f;
		[SerializeField] protected Transform mainNode;
		[SerializeField] protected Transform axis;
		protected Vector3 saved;

		// Use this for initialization
		void Start () {
			mainNode = gameObject.GetComponentInChildren<Camera>().transform.parent;
			axis = transform.parent;
			
		}
		
		// Update is called once per frame
		void Update() {
			Vector3 direction = target.position - axis.position;
			direction.y = 0; // this makes the camera angle perfectly flattened
			
			// not distance but angle difference?
			Quaternion tqat = Quaternion.LookRotation(direction, Vector3.up);
			/*
			float distance = Vector3.Distance(mainNode.position, targetVector);
			
			if (distance < stopDistance)
				speed -= decel * Time.deltaTime;
			else
				speed += accel * Time.deltaTime;
			if (speed < 0) { speed = 0; return; }
			if (speed > maxSpeed) speed = maxSpeed; 
			float step = speed * Time.deltaTime;
			*/
			axis.rotation = tqat;
			
		}
	}
}