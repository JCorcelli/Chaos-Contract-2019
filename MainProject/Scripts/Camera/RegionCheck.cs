using UnityEngine;
using System.Collections;

namespace CameraSystem
{

		
	public class RegionCheck : MonoBehaviour {
		// This contains methods that look at an object on camera as though it were 2D and determine whether regions are intersecting the target.
		public bool isSafe = false;
		public Transform target;
		
		
		public Camera cam;
		protected Camera defaultCam;
		public RectTransform center;
		// Update is called once per frame
		
		
		void Awake(){
			if (cam == null)
				cam = Camera.main;
			
			defaultCam = cam;
			
			if (center == null)
			{
				Transform t = transform.Find("Canvas/Center");
				if (t != null)
					center = t.GetComponent<RectTransform>();
			}
			
			if (target == null)
				target = CameraHolder.instance.targetA;
		}
		
		public Vector3 WorldToCameraViewport(Vector3 point) {
			// in order to be True the canvas needs to be visible as well
			
			return cam.WorldToViewportPoint(point);
			
		}
		
		public bool Contains(Vector3 point) {
			// in order to be True the canvas needs to be visible as well
			
			Vector3 screenPoint = cam.WorldToScreenPoint(point);
			bool check = 
					screenPoint.z < cam.farClipPlane
				&& screenPoint.z > cam.nearClipPlane;
				
				// but is it within rectangle
			// idk
			float scaleFactor = center.lossyScale.y;
			float screenScaler = (float)(Screen.width / Screen.height);
			float pad = 100 * .5f / screenScaler * scaleFactor;
			
			
			check = (check && screenPoint.y < Screen.height - pad && screenPoint.y > pad && screenPoint.x < Screen.width - pad && screenPoint.x > pad);
			
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
		
		

	 

		
		public bool GetSafe (Camera newCam) {
			cam = newCam;
			if (target == null) 
			{
				isSafe = false;
				enabled = false;
				return false;
			}
			
			
			
			isSafe = Contains(target.position);
			return isSafe;
			
		}
		
		public bool GetSafeRegion (Camera newCam) {
			cam = newCam;
			if (target == null) 
			{
				isSafe = false;
				enabled = false;
				return false;
			}
			
			
			
			isSafe = Contains(target.position, center);
			return isSafe;
			
		}
		
		public bool GetSafe () {
			cam = defaultCam;
			if (target == null) 
			{
				isSafe = false;
				enabled = false;
				return false;
			}
			
			
			
			isSafe = Contains(target.position, center);
			return isSafe;
			
		}
		
	}
}