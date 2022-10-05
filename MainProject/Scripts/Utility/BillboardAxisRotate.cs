using UnityEngine;
using System.Collections;

namespace Utility
{
	public class BillboardAxisRotate : UpdateBehaviour {


		protected Transform target;
		protected Transform body;
		
		
		// needs a body/this.transform relationship, I can add as many things as I like below
		protected void Awake() {
			
			target = Camera.main.transform;

			body = transform.parent;
		}
		protected override void OnLateUpdate () {
			
			
			var distanceToPlane = Vector3.Dot(body.up, target.position - transform.position);
			var planePoint = target.position - body.up * distanceToPlane;
			transform.LookAt(planePoint, body.up);
			
	 
		}
		
	}

}