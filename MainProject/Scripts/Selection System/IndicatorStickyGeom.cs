using UnityEngine;
using System.Collections;



namespace SelectionSystem
{
	[RequireComponent (typeof(PlaceIndicatorMagnet))]
	public class IndicatorStickyGeom : MonoBehaviour {

		/*
			Moves a image around
		*/
		public Collider currentCollider;
		public Transform target;
		public RectTransform screenTarget;
		protected Ray ray;
		public float defaultDepth = 100.0f;
		public float height = 0.0f;
		public bool savePrevious = true;
		public bool requireContact = false;
		public bool planeCast = true;
		public bool rotateToNormal = true;
		public bool topmostFirst = true;
		public Transform rotatedTarget;
		
		
		public float surfaceOffset = 0.0f;
		public LayerMask activeLayers;
		protected Plane plane = new Plane(Vector3.up, Vector3.zero);
		protected RaycastHit hit;
		protected RaycastHit[] hits;
		void Start () {
			GetComponent<PlaceIndicatorMagnet>().onDrag += OnDrag;
		}
		
		
		void OnDrag ()
		{
			
			
			ray = Camera.main.ScreenPointToRay(screenTarget.anchoredPosition);
			plane.SetNormalAndPosition(Vector3.up, Vector3.up * height);
			float dist;
			
			if (topmostFirst || Input.GetButtonDown("mouse 1") || currentCollider == null )
			{
				currentCollider = null;
				if (Physics.Raycast(ray, out hit, Mathf.Infinity, activeLayers))
				
					currentCollider = hit.collider;
			}
			
		
			hits = Physics.RaycastAll(ray, Mathf.Infinity, activeLayers);
			bool doHit = false;
			foreach (RaycastHit hitx in hits)
			{
				hit = hitx;
				if (hit.collider == currentCollider )
				{
					doHit = true;
					break;
				}
			}
			
			if (doHit)
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
			}
	        
			
		}
		
	}
}