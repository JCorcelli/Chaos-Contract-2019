using UnityEngine;
using System.Collections;

namespace SelectionSystem.Magnets
{
	public delegate void MagnetViewDelegate();
	public class MagnetView {

		
		// should calculate all relevant objects on screen positions
		// this would cost a lot if done frequently
		// I want this to run before other things that would need the info this frame.
		
		public static MagnetViewDelegate onSetWorld;
		public static MagnetViewDelegate onSetScreen;
		
		public static Transform worldTransform;
		public static Transform screenTransform;
		
		// virual, should be altered outside
		public static Vector3 worldPoint;
		public static Vector3 screenPoint;
		
		public static void SetTransforms(Transform w, Transform s) {
			worldTransform = w;
			screenTransform = s;
		}
		public static void SetScreenPoint()
		{
			// sets the world transform based on screen transform
			
			screenPoint = screenTransform.position;
			GetWorldPoint(screenPoint);
			if (MagnetView.onSetScreen != null) 
			{
				MagnetView.onSetScreen();
			}
			
		}
		public static void SetWorldPoint()
		{
			// sets the screen transform based on world transform
			
			worldPoint = worldTransform.position;
			GetScreenPoint(worldPoint);
			if (MagnetView.onSetWorld != null) 
			{
				MagnetView.onSetWorld();
			}
			
		}
		
		// this doesn't work. I need world to viewport point or screenpoint instead.
		public static Vector3 GetScreenPoint (Vector3 w3) {
			Vector3 s3 = Camera.main.WorldToScreenPoint(w3);
			
			return screenPoint = s3;
		}
		
		public static Ray ray;
		public static RaycastHit hit;
		public static Vector3 GetWorldPoint (Vector3 screenPoint) {
			ray = Camera.main.ScreenPointToRay(screenPoint);
			Physics.Raycast(ray,  out hit, Mathf.Infinity, Camera.main.eventMask);
			Vector3 w3 = hit.point;
			
			return worldPoint = w3;
		}
		
		
		
	}
}