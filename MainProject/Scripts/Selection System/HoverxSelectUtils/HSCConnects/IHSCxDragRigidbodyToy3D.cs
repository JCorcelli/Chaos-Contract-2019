using UnityEngine;
using System.Collections;


namespace SelectionSystem.IHSCx
{
	
	
	public class IHSCxDragRigidbodyToy3D : IHSCxConnect {
		
		
		public Rigidbody heldThing;
		protected Vector3 hitOffset; // anchor
		protected float cameraOffset;
		protected Vector3 lastMousePosition = Vector3.zero;
		public float deltaSpeed = 5f;
		public float bodySpeed = 5f;
		public float flingSpeed = 1f;
		protected Vector3 storedForce = Vector3.zero;
		
		public LayerMask activeLayers = -1;
		public bool saveObjectOffset = false;
		
		public bool moving = true;
		public bool snapping = true;
		public bool flingOnSwipe = false;
		public bool slippery = false;
		public float slipForce = 100f;
		public float slipDelta = 30f;
		
		public bool slingshot = false;
		public float maxSlingshotDistance = 50f;
		public float minSlingshotDistance = 1f;
		public float maxSlingshotPower = 200f;
		public float cannonForce = 1f;
		protected bool snapReady = false;
		protected Transform camTransform;
		
		
		
		protected Vector3 startPosition; // so maybe i can average the throw
		protected Vector3 currentPosition; // so maybe i can average the throw
		protected Vector3 worldPosition; // so maybe i can average the throw
		
		
		protected float scaleFactor;
		protected bool touching = false;
		
		
		protected bool hasGravity = false;
		protected bool slipped = false;
		
		
		protected void Awake() {
			if (heldThing == null) heldThing = GetComponentInParent<Rigidbody>();
			if (heldThing == null) Debug.Log(name + " no rigidbody",gameObject);
			camTransform = Camera.main.transform;
			hasGravity = heldThing.useGravity;
			originalLayer = heldThing.gameObject.layer;
		}
		protected override void OnEnable(){
			Connect();
			if (ih == null) return;
			ih.doWhilePressed += Hold;
			ih.onPress += Press;
			ih.onRelease += Release;
		}
		protected override void OnDisable(){
			
			if (ih == null) return;
			ih.doWhilePressed -= Hold;
			ih.onPress -= Press;
			ih.onRelease -= Release;
			
			Cleanup();
		}
		
		
		protected void Cleanup() {
			// being disabled doesn't mean it isn't still visible and moving, but I suppose the player shouldn't be holding it with this anymore
			
			heldThing.useGravity = hasGravity;
			heldThing.gameObject.layer = originalLayer;
			
		}
		protected int originalLayer;
		protected void Press(HSCxController caller) {
			
			heldThing.gameObject.layer = 2;
			
			
			if (moving)
			{
				heldThing.useGravity = false;
			}
			
			lastMousePosition = Input.mousePosition;
			cameraOffset = Vector3.Distance(ih.hit.point, ih.ray.origin); // from view to hit point
			
			worldPosition = ih.hit.point; // 3D
				
			hitOffset =   heldThing.position - ih.hit.point; // from its center, my offset
			
			
			//heldThing.centerOfMass = hitOffset;
			storedForce = Vector3.zero;
			
			heldThing.velocity = Vector3.zero;
			heldThing.angularVelocity = Vector3.zero;
			startPosition = worldPosition; // 3D
					
			
		}
		
		
		protected void GetWorldPositions() {
			/// sets cameraOffset
			/// sets worldPosition based on raycast
			///
			
			Ray ray = Camera.main.ScreenPointToRay(lastMousePosition);
			RaycastHit hit;
			
			// set up mouse position variables, regardless of moving or not
			if (!saveObjectOffset)
				cameraOffset = Vector3.Distance(heldThing.position - hitOffset, ray.origin);
			
			if (Physics.Raycast(ray, out hit, cameraOffset, activeLayers))
			{
				worldPosition = hit.point;
			}
			else
			{
				worldPosition = ray.GetPoint(cameraOffset);
				
			}
		}
		
		protected void Hold(HSCxController caller) {
			currentPosition = Input.mousePosition;
			Vector3 delta = currentPosition-lastMousePosition;
			
			Vector3 worldDelta; // so maybe i can average the throw
		
			//scaleFactor = Screen.width / 800f;
			//delta /= scaleFactor;
			if (delta.magnitude > deltaSpeed) 
				lastMousePosition += delta.normalized * deltaSpeed; // * scaleFactor;
			else
				lastMousePosition = currentPosition;
			
			
			
			worldDelta = delta.x * camTransform.right + delta.y * camTransform.up;
			
			GetWorldPositions();
				
			if (moving)
			{
				Vector3 moveTo = worldPosition + hitOffset ;
				
				
				if (snapping && delta.magnitude < flingSpeed)
				{
					snapReady = true;
					
				}
				else
					snapReady = false;
				
				
				// finalized move
				heldThing.MovePosition(Vector3.MoveTowards(heldThing.position, moveTo, bodySpeed * Time.deltaTime)) ;
				
				
			}
			
			if (flingOnSwipe)
			{
				if (delta.magnitude > flingSpeed)
				{
					//Vector3 cm = heldThing.centerOfMass;
					//heldThing.centerOfMass = Vector3.zero;
					heldThing.AddForceAtPosition(worldDelta, worldPosition, ForceMode.Force);
					storedForce += worldDelta;
					//heldThing.centerOfMass = cm;
				}
				else
				{
					//Vector3 cm = heldThing.centerOfMass;
					//heldThing.centerOfMass = Vector3.zero;
					heldThing.AddForceAtPosition(storedForce * .5f, heldThing.position, ForceMode.Force);
					storedForce *= .5f;
					//heldThing.centerOfMass = cm;
				}
			}
			
			
			if (heldThing.velocity.magnitude > bodySpeed) heldThing.velocity = heldThing.velocity.normalized * bodySpeed;
			if (storedForce.magnitude > bodySpeed) storedForce = storedForce.normalized * bodySpeed;
			
			if (slippery && (storedForce.magnitude > slipForce || delta.magnitude > slipDelta))
			{
				slipped = true;
				ih.Release();
			}
			
			
		}
		
		
		protected IEnumerator SnapOT(){
			
			float timer = 0;
			heldThing.freezeRotation = true;
			
			while (timer <= .1f)
			{
				heldThing.velocity = Vector3.zero;
				timer += Time.deltaTime;
				
				yield return null;
			}
			heldThing.freezeRotation = false;
		}
		
		
		protected void Release(HSCxController caller) {
			//heldThing.centerOfMass = Vector3.zero;
			heldThing.useGravity = hasGravity;
			heldThing.gameObject.layer = originalLayer;
			
			if (slipped) 
			{
				slipped = false;
				return;
			}
			
			if (moving && snapReady)
			{
				snapReady = false;
				
				heldThing.velocity = Vector3.zero;
				heldThing.angularVelocity = Vector3.zero;
				//StartCoroutine("SnapOT");
				return;
			}
			if (slingshot)
			{
				GetWorldPositions(); // assuming there's a new hit point for main object.
				
				Vector3 force = worldPosition - startPosition;
				
				float forcePower = force.magnitude;
				
				
				if (cannonForce > 0 || forcePower > minSlingshotDistance)
				{
					forcePower = Mathf.Clamp(forcePower, minSlingshotDistance, maxSlingshotDistance);
				
					forcePower = forcePower /maxSlingshotDistance * maxSlingshotPower;
					
					Vector3 direction = ih.hit.point - startPosition;
					force = direction.normalized  * forcePower + (camTransform.forward + Vector3.up).normalized * cannonForce;
					
					heldThing.AddForce(force, ForceMode.VelocityChange);
				
				}
			}
			if (flingOnSwipe)
				heldThing.AddForceAtPosition(storedForce, worldPosition, ForceMode.Impulse);
			
		}
		
		 
	}
}