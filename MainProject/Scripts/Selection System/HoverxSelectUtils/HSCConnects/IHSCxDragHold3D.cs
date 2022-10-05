using UnityEngine;
using System.Collections;


namespace SelectionSystem.IHSCx
{
	
	
	public class IHSCxDragHold3D : IHSCxConnect {
		
		
		public Transform heldThing;
		protected Vector3 hitOffset; // anchor
		protected float cameraOffset;
		protected Vector3 lastMousePosition = Vector3.zero;
		public float deltaSpeed = 5f;
		public LayerMask activeLayers = -1;
		public bool saveObjectOffset = false;
		
		protected override void OnEnable(){
			Connect();
			if (ih == null) return;
			ih.doWhilePressed += Hold;
			ih.onPress += Press;
		}
		protected override void OnDisable(){
			
			if (ih == null) return;
			ih.doWhilePressed -= Hold;
			ih.onPress -= Press;
		}
		
		protected void Press(HSCxController caller) {
			lastMousePosition = Input.mousePosition;
			cameraOffset = Vector3.Distance(ih.hit.point, ih.ray.origin); // from view to hit point
			
			hitOffset =   heldThing.position - ih.hit.point; // from its center, my offset
			
					
			
		}
		
		protected void Hold(HSCxController caller) {
			
			//float scaleFactor = Screen.width / 800f;
			Vector3 currentPosition = Input.mousePosition;
			Vector3 delta = currentPosition-lastMousePosition;
			//delta /= scaleFactor;
			if (delta.magnitude > deltaSpeed) 
				lastMousePosition += delta.normalized * deltaSpeed; // * scaleFactor;
			else
				lastMousePosition = currentPosition;
			
			Vector3 anchoredPosition = lastMousePosition;
			
			
			Ray ray = Camera.main.ScreenPointToRay(anchoredPosition);
			RaycastHit hit;
			
			
			if (!saveObjectOffset)
				cameraOffset = Vector3.Distance(heldThing.position - hitOffset, ray.origin);
			
			if (Physics.Raycast(ray, out hit, cameraOffset, activeLayers))
	        {
				heldThing.position = hit.point + hitOffset ; // maybe an object radius = + hit.normal*surfaceOffset;
				
	        }
			else
			{
				// I'd actually like to push it to the farthest side of my selection radius somehow, like + Camera.forward * pushspeed * Time.deltaTime
				heldThing.position = ray.GetPoint(cameraOffset) + hitOffset ;
				
				//cameraOffset += Camera.forward * pushSpeed * Time.deltaTime;
			}
			
			
		}
		 
	}
}