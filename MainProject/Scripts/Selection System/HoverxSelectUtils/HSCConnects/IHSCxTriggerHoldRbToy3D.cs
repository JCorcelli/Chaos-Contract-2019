using UnityEngine;
using System.Collections;


namespace SelectionSystem.IHSCx
{
	
	public class IHSCxTriggerHoldRbToy3D : IHSCxConnect {
		
		public string targetName = "ConstraintForSelectable";
		
		public bool onEnter = true;
		public bool enterActive = true;
		public GameObject enterThing;
		protected int count = 0;
		
		
		public bool pushing = true;
		public bool pushFromOrigin = false;
		public float originUpwardOffset = .5f;
		public bool pulling = true;
		public bool repelling = false;
		public float bodySpeed = 4f;
		
		public float pushForce = 2f;
		public float repelForce = 5f;
		
		
		
		public float pullSpeed = 5f;
		// keep things in a range
		public bool alwaysPullOrigin = false;
		public bool alwaysPushOrigin = false;
		public bool alwaysRepel = false;
		
		public bool effectOnPress = false;
		public bool effectOnActive = true;
		public bool effectOnHover = false;
		public bool blockOnPress  = false;
		public bool blockOnActive = false;
		public bool blockOnHover  = false;
		
		public bool activeIfHeld = false;
		
		protected Transform camTransform;
		new protected Transform transform;
		protected Rigidbody heldThing;
		public float defaultRange = 1f;
		public float range = 1f;
		public Transform holdOrigin;
		
		protected void Awake () { 
			Connect();
			if (enterThing == null) enterThing= gameObject;
			if (enterActive) enterThing.SetActive( !onEnter); // not triggered at start
			camTransform = Camera.main.transform;
			transform = GetComponent<Transform>();
			heldThing = GetComponent<Rigidbody>();
			
		}
		
		
		protected override void OnEnable(){
			base.OnEnable();
			if (ih == null) return;
			ih.onRelease += Release;
			
			ih.doWhilePressed += Push;
			
		}
		protected override void OnDisable(){
			
			base.OnDisable();
			if (ih == null) return;
			
			ih.onRelease -= Release;
		
			ih.doWhilePressed -= Push;
			
			if (enterActive) enterThing.SetActive( !onEnter);
			count = 0;
		}
		
		
		protected bool StatusCheck() {
			
			bool block;
			// If I'm blocking the next check then I return early, this is opposed to an else, else if chain.
			
			block = (blockOnPress && ih.pressed);
			used= false;
			if ( block ) 
			{
				if (effectOnPress)
					return used = true;
				return false;
			}
			else if (effectOnPress && ih.pressed)
			{
				return true;
			}
			
			block = (blockOnActive && ih.isActive);
			
			if (block )
			{
				if (effectOnActive)
					return used = true;
				return false;
			}
			else if (effectOnActive && ih.isActive)
			{
				return true;
			}
			
			block = (blockOnHover && ih.isHovered);
			
			if (block)
			{
				if (effectOnHover)
					return used = true;
				return false;
			}
			else if (effectOnHover && ih.isHovered)
			{
				return used = true;
			}
			return false;
				
		}
		protected override void OnLateUpdate() {
			if (holdOrigin == null) return;
			
			// if these are both running it keeps object at border
			PushOutOfRange();
			PullInRange();
			
			if (used && keepAboveOrigin && heldThing.position.y < holdOrigin.position.y)
			{
				Vector3 pos = heldThing.position;
				pos.y = holdOrigin.position.y;
				heldThing.MovePosition(Vector3.MoveTowards(heldThing.position, pos, bodySpeed * Time.deltaTime));
			}
			used = false;
		}
		protected void Release(HSCxController caller) {
			Repel();
			
			Exit();
		}
		
		
		
		protected void Exit() {
			if (count <= 0)
			{
				count = 0;
				if (enterActive) enterThing.SetActive(!onEnter);
			}
		}
		
		
		protected void TryGetRange(Collider col) {
			// trying to get a radius
			SphereCollider s =  col.GetComponent<SphereCollider>();
			
			if (s != null)
			{
				range = s.radius * s.transform.lossyScale.z;
				return;
			}
		
			
			CapsuleCollider cap =  col.GetComponent<CapsuleCollider>();
			
			if (cap != null)
			{
				range = cap.radius  * cap.transform.lossyScale.z;
				return;
			}
			
			range = defaultRange; // or I could see how far I was from the center of collider
			
		}
		protected void Enter() {
			if (enterActive) enterThing.SetActive(onEnter);
		}
		protected void OnTriggerEnter( Collider col ) {
			if (targetName == col.name) {
				count ++;
				TryGetRange(col);
				holdOrigin = col.transform;
				Enter();
			}
		}
		protected void OnTriggerExit( Collider col ) {
			if (targetName == col.name) {
				
				count --;
				if (ih.pressed) return; // ignoring Exit
				Exit();
				
			}
			
		}
		protected void Push(HSCxController caller) {
			
			Push();
		}
		protected void Push() {
			if (!pushing) return;
			
			Vector3 push = camTransform.forward * pushForce;
			heldThing.AddForce( push, ForceMode.Force );
		}
		protected void Repel() {
			if (!repelling || holdOrigin == null) return;
			if (!(alwaysRepel || StatusCheck())) return; 
				
			Vector3 push = (heldThing.position - holdOrigin.position).normalized * repelForce;
			heldThing.AddForce( push, ForceMode.Impulse );
		}
		
		public bool keepAboveOrigin = true;
		public bool used = false;
		protected void PullInRange(HSCxController caller) {PullInRange();}
		protected void PullInRange() {
			if (!pulling) return;
			if (!(alwaysPullOrigin || used || StatusCheck())) return;
			
			float d = Vector3.Distance(holdOrigin.position, heldThing.position);
			
			if (  d > range  )
			{
				
				Vector3 goal = holdOrigin.position + (heldThing.position - holdOrigin.position).normalized * range;
				
				
				heldThing.MovePosition(Vector3.MoveTowards(heldThing.position, goal, pullSpeed * Time.deltaTime)); // extra distance after subtracting the max
			}
			
		}
		
		protected void PushOutOfRange() {
			if (!pushFromOrigin) return;
			if (!(alwaysPushOrigin || used || StatusCheck())) return;
			
			float d = Vector3.Distance(holdOrigin.position, heldThing.position);
			
			if (  d < range  )
			{
				Vector3 push = (heldThing.position - (holdOrigin.position + originUpwardOffset * - holdOrigin.up)).normalized ;
				
				
				Vector3 goal = push * range;
				
				heldThing.MovePosition(Vector3.MoveTowards(heldThing.position, goal, bodySpeed * Time.deltaTime)); // extra distance after subtracting the max
				
				if (repelling)
				{
				heldThing.AddForce( push * pushForce, ForceMode.Impulse );
				if (heldThing.velocity.magnitude > bodySpeed) heldThing.velocity = heldThing.velocity.normalized * bodySpeed;
				}
			
			}
			
		}
		
		
			
		 
	}
}