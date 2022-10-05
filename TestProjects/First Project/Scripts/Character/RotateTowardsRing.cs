using UnityEngine;
using System.Collections;

namespace TestProject 
{
	public class RotateTowardsRing : MonoBehaviour {
		public Transform target;
		public float speed = 3f;
		
		protected float currentVelocity = 0f;
		[SerializeField] protected Transform mainNode;
		[SerializeField] protected Transform axis;
		protected Vector3 saved;
		public float accel = 1f; 
		public float decel = 2f; 
		public float stopDistance = 10f;
		
		public float maxSpeed = 10f;

		// Use this for initialization
		void Start () {
			mainNode = gameObject.GetComponentInChildren<Camera>().transform.parent;
			axis = transform.parent;
		}
		
		// Update is called once per frame
		void Update() {
			float distance;
			float savedDist = Vector3.Distance(target.position, mainNode.position);
			Vector3 saved =  mainNode.position;
			foreach (Transform child in transform){
				distance = Vector3.Distance(target.position, child.position);
				if (distance < savedDist) 
				{
					savedDist = distance;
					saved = child.position;
				}
			} 
			distance = Vector3.Distance(mainNode.position, saved);
			
			if (saved == mainNode.position || distance < stopDistance)
				speed -= decel * Time.deltaTime;
			else
				speed += accel * Time.deltaTime;
			if (speed < 0) { speed = 0; return; }
			if (speed > maxSpeed) speed = maxSpeed; 
			float step = speed * Time.deltaTime;
			
			
			Vector3 p = mainNode.position - transform.position;
			Vector3 vp = Vector3.RotateTowards(p, saved - transform.position, step, 0f);
			mainNode.position = vp + transform.position;
			mainNode.LookAt(axis);
		}
	}
}