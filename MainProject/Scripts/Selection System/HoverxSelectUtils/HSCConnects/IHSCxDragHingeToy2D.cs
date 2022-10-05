using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace SelectionSystem.IHSCx
{
	
	
	public class IHSCxDragHingeToy2D : IHSCxConnect {
		
		
		public Rigidbody2D heldThing;
		protected Rigidbody2D thisThing;
		protected HingeJoint2D hingeJoint2D;
		
		protected Vector3 hitOffset; // anchor
		//protected Vector3 viewOffset;
		protected Vector3 lastMousePosition = Vector3.zero;
		public float deltaSpeed = 5f;
		public float flingSpeed = 10f;
		
		protected Vector3 storedForce = Vector3.zero;
		
		public bool moving = true;
		public bool releaseUnhinges = true; 
		public bool flingOnSwipe = true;
		public bool slippery = true;
		public float slipForce = 100f;
		public float slipDelta = 30f;
		
		public bool slingshot = true;
		public float maxSlingshotDistance = 50f;
		public float minSlingshotDistance = 1f;
		public float maxSlingshotPower = 200f;
		
		protected Canvas canvas;
		protected void Awake() {
			thisThing = GetComponent<Rigidbody2D>();
			hingeJoint2D = GetComponent<HingeJoint2D>();
			canvas = GetComponentInParent<Canvas>();
			
			if (releaseUnhinges)
				hingeJoint2D.enabled = false;
			else
				hingeJoint2D.enabled = true;
				
			
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
		
		protected void ConnectHinge() {
			if (connectingHinge) return;
			StartCoroutine("_ConnectHinge");
		}
		protected bool connectingHinge = false;
		protected IEnumerator _ConnectHinge() {
			connectingHinge = true;
			yield return new WaitForFixedUpdate();
				hingeJoint2D.enabled = true;
			connectingHinge = false;
		}
		
		protected void Press(HSCxController caller) {
			canvas = GetComponentInParent<Canvas>();
			if (releaseUnhinges)
				hingeJoint2D.enabled = true;
			lastMousePosition = Input.mousePosition;
			
			
			
			hitOffset =   thisThing.GetPoint(lastMousePosition); // from its center, my offset
			
			hingeJoint2D.anchor = hitOffset / transform.lossyScale.y;
			
			heldThing.position = lastMousePosition ;
			
			
			
			
			storedForce = Vector3.zero;
			
			heldThing.velocity = Vector2.zero;
			
			startPosition = lastMousePosition;
			
		}
		
		protected Vector3 startPosition; // so maybe i can average the throw
		protected Vector3 currentPosition; // so maybe i can average the throw
		
		
		protected void Hold(HSCxController caller) {
			
			canvas = GetComponentInParent<Canvas>();
			
			currentPosition = Input.mousePosition;
			
			// Clamp to screen

			currentPosition.x = Mathf.Clamp(currentPosition.x, 0, Screen.width);
			currentPosition.y = Mathf.Clamp(currentPosition.y, 0, Screen.height);
				
			Vector3 delta = currentPosition-lastMousePosition;
			
			// delta would be scaled because mouse position is in pixels.
			delta /=  transform.lossyScale.y;
			if (delta.magnitude > deltaSpeed) 
			{
				lastMousePosition += delta.normalized * deltaSpeed *  transform.lossyScale.y; // mouse should be slower now
				
			}
			else
			{
				lastMousePosition = currentPosition;
			}
			
			
			if (moving)
			{
				heldThing.MovePosition (lastMousePosition);
				//stay on screen
				
			}
				
			if (flingOnSwipe)
			{
				if (delta.magnitude > flingSpeed )
				{
					storedForce += delta;
				}
				else
				{
					storedForce *= .9f;
				}
			}
			
			if (slippery && (storedForce.magnitude > slipForce  || delta.magnitude > slipDelta ))
			{
				slipped = true;
				ih.Release();
			}
			
		}
		protected bool slipped = false;
		protected void Release(HSCxController caller) {
			if (releaseUnhinges)
				hingeJoint2D.enabled = false;
			heldThing.velocity = Vector3.zero;
			if (slipped) 
			{
				slipped = false;
				return;
			}
			if (slingshot)
			{
				Vector3 force = Input.mousePosition - startPosition;
				
				// mouse position is in pixels.
				force /=  transform.lossyScale.y;
				float forcePower = force.magnitude;
				if (forcePower > minSlingshotDistance)
				{
					forcePower = forcePower /maxSlingshotDistance * maxSlingshotPower;
					force = force.normalized * forcePower * thisThing.mass;
					
					thisThing.AddForce(force  * thisThing.mass, ForceMode2D.Impulse);
				
				}
			}
			if (flingOnSwipe)
				thisThing.AddForce(storedForce * thisThing.mass, ForceMode2D.Impulse);
			
		}
		 
	}
}