using UnityEngine;
using System.Collections;

namespace CameraSystem
{

		
	public class RegionCheckUtil 
	{
		// This contains methods that look at an object on camera as though it were 2D and determine whether regions are intersecting the target.
		public bool isSafe = false;
		public Transform target;
		
		public Camera cam;
		public RectTransform region;
		
		// Update is called once per frame
		
		
		public Vector3 WorldToCameraViewport(Vector3 point) {
			// in order to be True the canvas needs to be visible as well
			
			return cam.WorldToViewportPoint(point);
			
		}
		
		
		public bool InViewFloat(Vector3 point){
			point = WorldToCameraViewport(point);
			bool check = point.x > 0 &&  point.x < Screen.width && point.y > 0 && point.y < Screen.height;
			
			return check;
			
		}
		public bool Contains(Vector3 point) {
			// in order to be True the canvas needs to be visible as well
			
			Vector3 screenPoint = cam.WorldToScreenPoint(point);
			bool check = 
					screenPoint.z < cam.farClipPlane
				&& screenPoint.z > cam.nearClipPlane
				&& RectTransformUtility.RectangleContainsScreenPoint(region, screenPoint, cam);
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
		
		

	 
		public bool GetSafe () {
			if (target == null) 
			{
				isSafe = false;
				return false;
			}
			
			
			
			isSafe = Contains(target.position, region);
			return isSafe;
			
		}
		
	}
}