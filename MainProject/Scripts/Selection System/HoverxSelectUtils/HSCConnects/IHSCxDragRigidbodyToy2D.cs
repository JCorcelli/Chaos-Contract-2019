using UnityEngine;
using System.Collections;


namespace SelectionSystem.IHSCx
{
	
	
	public class IHSCxDragRigidbodyToy2D : IHSCxConnect {
		
		
		public Rigidbody2D heldThing;
		protected Vector3 hitOffset; // anchor
		//protected Vector3 viewOffset;
		protected Vector3 lastMousePosition = Vector3.zero;
		public float bodySpeed = 5f;
		public float deltaSpeed = 5f;
		public float flingSpeed = 10f;
		public float powerMultiply = 10f;
		
		protected Vector3 storedForce = Vector3.zero;
		
		
		public bool moving = true;
		public bool flingOnSwipe = true;
		public bool slippery = true;
		public float slipForce = 100f;
		public float slipDelta = 30f;
		
		public bool slingshot = true;
		public float maxSlingshotDistance = 50f;
		public float minSlingshotDistance = 1f;
		public float maxSlingshotPower = 200f;
		
		protected Canvas canvas;
		
		void Awake () {
			canvas = GetComponentInParent<Canvas>();
			
		}
		
		protected override void OnEnable(){
			Connect();
			if (ih == null) return;
			ih.doWhilePressed += Hold;
			//ih.doWhileHovered += Press;
			ih.onPress += Press;
			
			ih.onRelease += Release;
		}
		protected override void OnDisable(){
			
			if (ih == null) return;
			ih.doWhilePressed -= Hold;
			ih.onPress -= Press;
			
			ih.onRelease -= Release;
		}
		
		protected void Press(HSCxController caller) {
			canvas = GetComponentInParent<Canvas>();
			
			lastMousePosition = Input.mousePosition;
			
			
			hitOffset =   heldThing.GetPoint(lastMousePosition); // from its center, my offset
			
			heldThing.centerOfMass = hitOffset;
			
			
			storedForce = Vector3.zero;
			
			heldThing.velocity = Vector2.zero;
			heldThing.angularVelocity = 0f;
			startPosition = lastMousePosition;
			
		}
		
		protected Vector3 startPosition; // so maybe i can average the throw
		protected Vector3 currentPosition; // so maybe i can average the throw
		
		
		protected void Hold(HSCxController caller) {
			canvas = GetComponentInParent<Canvas>();
			
			hitOffset =   heldThing.position - heldThing.worldCenterOfMass;
			
			currentPosition = Input.mousePosition;
			
			// Clamp to screen

			currentPosition.x = Mathf.Clamp(currentPosition.x, 0, Screen.width);
			currentPosition.y = Mathf.Clamp(currentPosition.y, 0, Screen.height);
				
			Vector3 delta = currentPosition-lastMousePosition;
			delta /=  transform.lossyScale.y;
			if (delta.magnitude > deltaSpeed) 
			{
				lastMousePosition += delta.normalized * deltaSpeed * transform.lossyScale.y;
				
			}
			else
			{
				lastMousePosition = currentPosition;
			}
			
			
			if (moving)
			{
				heldThing.position = lastMousePosition + hitOffset ;
				//stay on screen
			}
			
				
				
			if (flingOnSwipe)
			{
				
				if (delta.magnitude > flingSpeed)
				{
					Vector2 cm = heldThing.centerOfMass;
					heldThing.centerOfMass = Vector2.zero;
					heldThing.AddForceAtPosition(delta, lastMousePosition, ForceMode2D.Impulse);
					storedForce += delta * powerMultiply;
					heldThing.centerOfMass = cm;
				}
				else
				{
					Vector2 cm = heldThing.centerOfMass;
					heldThing.centerOfMass = Vector2.zero;
					heldThing.AddForceAtPosition(storedForce * .5f, heldThing.position, ForceMode2D.Impulse);
					storedForce *= .5f;
					heldThing.centerOfMass = cm;
				}
			}
			if (heldThing.velocity.magnitude > bodySpeed) 
				heldThing.velocity = heldThing.velocity.normalized * bodySpeed;
			
			if (slippery && (storedForce.magnitude > slipForce || delta.magnitude > slipDelta))
			{
				slipped = true;
				ih.Release();
			}
			
		}
		protected bool slipped = false;
		protected void Release(HSCxController caller) {
			heldThing.centerOfMass = Vector2.zero;
			if (slipped) 
			{
				slipped = false;
				return;
			}
			if (slingshot)
			{
				Vector3 force = Input.mousePosition - startPosition;
				force /=  transform.lossyScale.y;
				float forcePower = force.magnitude;
				if (forcePower > minSlingshotDistance)
				{
					forcePower = Mathf.Clamp(forcePower * powerMultiply, minSlingshotDistance, maxSlingshotDistance);
					
					forcePower = forcePower /maxSlingshotDistance * maxSlingshotPower;
					force = force.normalized * forcePower;
					
					heldThing.AddForce(force, ForceMode2D.Impulse);
				
				}
			}
			if (flingOnSwipe)
				heldThing.AddForceAtPosition(storedForce, lastMousePosition, ForceMode2D.Impulse);
			
			
		}
		 
	}
}