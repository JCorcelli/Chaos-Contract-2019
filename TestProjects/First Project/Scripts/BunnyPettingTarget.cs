using UnityEngine;
using System.Collections;

namespace TestProject
{
	public class BunnyPettingTarget : MonoBehaviour {

		// Use this for initialization
		
		public int clicks = 0;
		
		public GameObject colliders;
		private Transform back;
		private Transform front;
		private bool pressing;
		private RaycastHit hit;
		private Animator animator;
		
		void Awake() {
			colliders = transform.Find("PettableColliders").gameObject;
			back = transform.Find("Back");
			front = transform.Find("Front");
			
			animator = transform.parent.gameObject.GetComponent<Animator>();
			
			if (colliders == null) Debug.Log("need 'PettableColliders' set as child");
			if (back == null) Debug.Log("need 'back' set as child");
			if (front == null) Debug.Log("need 'front' set as child");
		}
		/*
		void OnEnable() {
			colliders.SetActive(true);
		}
		void OnDisable() {
			colliders.SetActive(false);
		}*/
		public void hitPosition(Collider c) {
			
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			c.Raycast(ray, out hit, Mathf.Infinity);
			
			
			Vector3 point = closestPointOnLineSegment(front.position, back.position, hit.point);
			
			float d = Vector3.Distance(front.position, back.position);
			float d2 = Vector3.Distance(front.position, point);
			// 1 back, 0 is front
			float petDistFront = d2/d;
			// set animation paramter distanceToFront
			animator.SetFloat("PetFront", petDistFront);
			
			
		}
		private Vector3 closestPointOnLineSegment(Vector3 vA, Vector3 vB, Vector3 vPoint) {

			Vector3 vVector1 = vPoint - vA;
			Vector3 vVector2 = (vB - vA).normalized;

			float d = Vector3.Distance(vA, vB);
			float t = Vector3.Dot(vVector2, vVector1);

			if (t <= 0)
				return vA;

			if (t >= d)
				return vB;

			Vector3 vVector3 = vVector2 * t;

			Vector3 vClosestPoint = vA + vVector3;

			return vClosestPoint;
	
		}
	}
}