using UnityEngine;
using System.Collections;



namespace SelectionSystem
{
	
	public class ContinuousIndicatorSignal : UpdateBehaviour {

		/*
			The mouse pointer, basically. intended to make a tool point at it
		*/
		
		public Transform target;
		public Transform planeTarget;
		protected Ray ray;
		public float defaultDepth = 100.0f;
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
		
		public string _planeTargetName = "PlayerTool";
		public string _planeTargetTag = "PlayerRig";
		
		protected void Start() {
			planeTarget = gameObject.FindNameXTag(_planeTargetName, _planeTargetTag).transform;
			
			if (planeTarget == null) Debug.Log("The guy who i'm following isn't here so this isn't working.", gameObject);
		}
		protected override void OnUpdate ()
		{
			base.OnUpdate();
			
			
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			plane.SetNormalAndPosition(planeTarget.up, planeTarget.position); // normal could be planeTarget's up
			float dist;
			
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, activeLayers))
	        {
				
				target.position = hit.point + hit.normal*surfaceOffset;
				
				if (rotatedTarget != null) rotatedTarget.up = hit.normal;
				
				
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