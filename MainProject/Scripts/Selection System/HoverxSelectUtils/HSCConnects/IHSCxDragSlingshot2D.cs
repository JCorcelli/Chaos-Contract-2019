using UnityEngine;
using System.Collections;


namespace SelectionSystem.IHSCx
{
	
	
	public class IHSCxDragSlingshot2D : IHSCxConnect {
		
		
		public Rigidbody2D heldThing;
		protected Vector3 hitOffset; // anchor
		//protected Vector3 viewOffset;
		protected Vector3 lastMousePosition = Vector3.zero;
		public float deltaSpeed = 5f;
		
		protected Vector3 storedForce = Vector3.zero;
		
		public bool addSpin = true;
		public float powerMultiply = 1f;
		public float maxSlingshotDistance = 50f;
		public float minSlingshotDistance = 1f;
		public float maxSlingshotPower = 200f;
		
		protected Canvas canvas;
		protected void Awake() {
			
			canvas = GetComponentInParent<Canvas>();
			
		}
		
		protected override void OnEnable(){
			Connect();
			if (ih == null) return;
			//ih.doWhileHovered += Press;
			ih.onPress += Press;
			
			ih.onRelease += Release;
		}
		protected override void OnDisable(){
			
			if (ih == null) return;
			ih.onPress -= Press;
			
			ih.onRelease -= Release;
		}
		
		protected void Press(HSCxController caller) {
			
			canvas = GetComponentInParent<Canvas>();
			startPosition = Input.mousePosition;
			
		}
		
		protected Vector3 startPosition; // so maybe i can average the throw
		
		protected void Release(HSCxController caller) {
			canvas = GetComponentInParent<Canvas>();
			if (Input.GetButton("mouse 1")) return; // this is a sign that it slipped
			
			Vector3 force = Input.mousePosition - startPosition;
			force /=  transform.lossyScale.y;
			float forcePower = force.magnitude;
			if (forcePower > minSlingshotDistance)
			{
				forcePower = forcePower /maxSlingshotDistance * maxSlingshotPower;
				force = force.normalized * forcePower * powerMultiply;
				
				if (addSpin)
					heldThing.AddForceAtPosition(force, startPosition, ForceMode2D.Impulse);
				else
					heldThing.AddForce(force, ForceMode2D.Impulse);
			
			}
			
		}
		 
	}
}