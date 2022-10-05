using UnityEngine;
using System.Collections;



namespace SelectionSystem
{
	[RequireComponent (typeof(PlaceIndicatorMagnet))]
	public class IndicatorSignalLook : MonoBehaviour {

		/*
			Moves a image around
		*/
		
		public Transform target;
		public Transform avatarTransform;
		public RectTransform screenTarget;
		protected Ray ray;
		public float safeDepth = 5.0f;
		public float defaultDepth = 5.0f;
		public float height = 0.0f;
		
		public bool savePrevious = true;
		public bool requireContact = false;
		public bool planeCast = true;
		public bool rotateToNormal = true;
		public Transform rotatedTarget;
		
		
		public float surfaceOffset = 0.0f;
		public LayerMask activeLayers;
		protected Plane plane = new Plane(Vector3.up, Vector3.zero);
		protected RaycastHit hit;
		void Start () {
			GetComponent<PlaceIndicatorMagnet>().onDrag += OnDrag;
		}
		
		
		void OnDrag ()
		{
			
			
			ray = Camera.main.ScreenPointToRay(screenTarget.anchoredPosition);
			plane.SetNormalAndPosition(Vector3.up, Vector3.up * height);
			float dist;
			
			height = avatarTransform.position.y;
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, activeLayers))
	        {
				
				target.position = hit.point + hit.normal*surfaceOffset;
				
				if (rotatedTarget != null) rotatedTarget.up = hit.normal;
				
				height = target.position.y;
				if (savePrevious)
				{
					defaultDepth = Vector3.Distance(target.position, ray.origin);
				}
				
	        }
			else if (planeCast && plane.Raycast(ray, out dist) && ray.GetPoint(dist).y < Camera.main.transform.position.y)
			{
					
				//Get the point that is clicked
				
				//Move your cube GameObject to the point where you clicked
				
				target.position = ray.GetPoint(dist);
			
				if (rotatedTarget != null) rotatedTarget.up = Vector3.up;
					
				if (savePrevious)
				{
					defaultDepth = Vector3.Distance(target.position, ray.origin);
					
				}
				
				// this currently drops the point in space to the previuos height
				
				
			}
			else if (!requireContact)
			{
				if (rotatedTarget != null) rotatedTarget.up = Vector3.up;
				
				target.position = ray.GetPoint(defaultDepth);
				
				//fix
			}
			Vector3 newPosition = target.position;
			//newPosition.y = avatarTransform.position.y;
			newPosition = Vector3.MoveTowards(avatarTransform.position, newPosition, safeDepth); 
			
			target.position = newPosition;
		}
		
	}
}