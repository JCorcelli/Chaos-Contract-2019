using UnityEngine;
using System.Collections;


namespace SelectionSystem.IHSCx
{
	
	
	public class IHSCxDragSpringToy3D : IHSCxConnect {
		
		
		public Rigidbody heldThing;
		protected Rigidbody thisThing;
		protected Transform thisThingTransform;
		
		protected SpringJoint springJoint;
		
		
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
		public bool releaseUnhinges = true; 
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
		
		public bool balloon = false;
		protected bool hasGravity = false;
		protected bool slipped = false;
		public float springForce = 100f;
		public float springDamper = .2f;
		
		protected void ConnectHinge() {
			if (connectingHinge) return;
			StartCoroutine("_ConnectHinge");
		}
		
		protected bool connectingHinge = false;
		protected IEnumerator _ConnectHinge() {
			connectingHinge = true;
			yield return new WaitForFixedUpdate();
			
			springJoint = thisThing.gameObject.AddComponent<SpringJoint>();
			
			springJoint.connectedBody = heldThing;
			springJoint.autoConfigureConnectedAnchor = false;
			
			springJoint.connectedAnchor = new Vector3(0f,0f,0f);
			SetSpringValues();
			connectingHinge = false;
			
		}
		protected void SetSpringValues() {
			if (springJoint == null) return;
			
			springJoint.anchor = hitOffset * .9f;
			
			springJoint.spring = springForce;
			springJoint.damper = springDamper;
			
		}
		
		protected void Awake() {
			if (thisThing == null) thisThing = GetComponentInParent<Rigidbody>();
			if (thisThing == null) Debug.Log(name + " no rigidbody",gameObject);
			thisThingTransform = thisThing.transform;
			springJoint = GetComponentInParent<SpringJoint>();
			
			if (heldThing == null) {
				Debug.Log(name + "need a hinge set in heldThing",gameObject);
				thisThing.gameObject.SetActive(false);
				return;
			}
			heldThing.transform.parent = thisThing.transform.parent;
			camTransform = Camera.main.transform;
			
			if (springJoint != null)
			{
				if (releaseUnhinges)
					GameObject.Destroy(springJoint);
			}
			else if (!releaseUnhinges)
				ConnectHinge();
				
			// if there's not a connected hinge then I need to set its position anyway.
			
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
		}
		
		protected int originalLayer;
		protected void Press(HSCxController caller) {
			
			originalLayer = thisThing.gameObject.layer;
			
			thisThing.gameObject.layer = 2;
			
			hasGravity = thisThing.useGravity;
			
			if (balloon && moving)
			{
				thisThing.useGravity = false;
			}
			
			lastMousePosition = Input.mousePosition;
			cameraOffset = Vector3.Distance(ih.hit.point, ih.ray.origin); // from view to hit point
			
			worldPosition = ih.hit.point; // 3D
			
			hitOffset =  thisThingTransform.InverseTransformPoint	(ih.hit.point); // from its center, my offset
			
			
			heldThing.MovePosition( worldPosition );
			
			if (releaseUnhinges) // assuming autoposition I can stop caring here
				ConnectHinge();
			else
				SetSpringValues();
				
			
			storedForce = Vector3.zero;
			
			thisThing.velocity = Vector3.zero;
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
				cameraOffset = Vector3.Distance(heldThing.position, camTransform.position);
			
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
				Vector3 moveTo = worldPosition ;
				
				
				if (snapping && delta.magnitude < flingSpeed)
				{
					snapReady = true;
					
				}
				else
					snapReady = false;
				
				
				// finalized move
				heldThing.MovePosition(moveTo) ;
				
				
			}
			
			if (flingOnSwipe)
			{
				if (delta.magnitude > flingSpeed)
				{
					
					storedForce += worldDelta;
					
				}
				else
				{
					
					storedForce *= .9f;
					
				}
			}
			
			
			if (thisThing.velocity.magnitude > bodySpeed) thisThing.velocity = thisThing.velocity.normalized * bodySpeed;
			
			if (storedForce.magnitude > bodySpeed) storedForce = storedForce.normalized * bodySpeed;
			
			if (slippery && (storedForce.magnitude > slipForce || delta.magnitude > slipDelta))
			{
				slipped = true; // maybe use the "break" instead
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
			if (releaseUnhinges && springJoint != null)
				GameObject.Destroy(springJoint);
			
			heldThing.velocity = Vector3.zero;
			
			//thisThing.centerOfMass = Vector3.zero;
			
			thisThing.gameObject.layer = originalLayer;
		
			thisThing.useGravity = hasGravity;
				
			if (slipped) 
			{
				slipped = false;
				return;
			}
			
			if (moving && snapReady)
			{
				snapReady = false;
				
				thisThing.velocity = Vector3.zero;
				thisThing.angularVelocity = Vector3.zero;
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
					
					thisThing.AddForce(force, ForceMode.VelocityChange);
				
				}
			}
			if (flingOnSwipe)
				thisThing.AddForceAtPosition(storedForce, worldPosition, ForceMode.Impulse);
			
		}
		
		 
	}
}