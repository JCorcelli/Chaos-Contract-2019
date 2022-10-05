using UnityEngine;
using System.Collections;

namespace TestProject.Cameras
{

		
	public class GetFidelityToFront : MonoBehaviour {
		// This contains methods that look at an object on camera as though it were 2D and determine whether regions are intersecting the target.
		public bool atCenter = false;
		public Transform target;
		
		private Camera cam;
		private RectTransform center;
		// Update is called once per frame
		
		void Awake(){
			cam = gameObject.GetComponentInChildren<Camera>();
			
			Transform t = transform.Find("Canvas/Center");
			if (t != null)
				center = t.GetComponent<RectTransform>();
			
				
		}
		
		public Vector3 WorldToCameraViewport(Vector3 point) {
			// in order to be True the canvas needs to be visible as well
			
			return cam.WorldToViewportPoint(point);
			
		}
		
		public bool Contains(Vector3 point) {
			// in order to be True the canvas needs to be visible as well
			
			Vector3 screenPoint = cam.WorldToViewportPoint(point);
			bool check = 
					screenPoint.z < cam.farClipPlane
				&& screenPoint.z > cam.nearClipPlane
				&& 0 < screenPoint.x && screenPoint.x < 1
				&& 0 < screenPoint.y && screenPoint.y < 1;
			return check;
			
		}
		
		
		public bool Contains(Vector3 point, RectTransform region) {
			// in order to be True the canvas needs to be visible as well
			
			Vector3 screenPoint = cam.WorldToScreenPoint(point);
			bool check = 
					screenPoint.z < cam.farClipPlane
				&& screenPoint.z > cam.nearClipPlane
				&& RectTransformUtility.RectangleContainsScreenPoint(region, screenPoint, cam);
			return check;
			
		}
		
		

	 
		private void Update () {
			if (target == null) 
			{
				
				enabled = false;
				return;
			}
			
			
			
			atCenter = Contains(target.position, center);
			
		}
	}
}