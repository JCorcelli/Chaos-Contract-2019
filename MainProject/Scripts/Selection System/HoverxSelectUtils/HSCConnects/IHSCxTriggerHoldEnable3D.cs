using UnityEngine;
using System.Collections;


namespace SelectionSystem.IHSCx
{
	
	public class IHSCxTriggerHoldEnable3D : IHSCxConnect {
		
		public string targetName = "ConstraintForSelectable";
		
		public bool onEnter = true;
		public bool enterActive = true;
		public GameObject enterThing;
		protected int count = 0;
		
		
		public bool pushing = true;
		public float pushSpeed = 2f;
		
		
		public float pullSpeed = 5f;
		// keep things in a range
		public bool pullInRangeAlways = false;
		public bool pullOnPress = true;
		public bool pullOnActive = true;
		public bool pullOnHover = true;
		public bool blockNextOnPress = true;
		public bool blockNextOnActive = true;
		public bool blockNextOnHover = true;
		
		public bool activeIfHeld = false;
		
		protected Transform camTransform;
		new protected Transform transform;
		public float defaultRange = 1f;
		public float range = 1f;
		protected Transform holdOrigin;
		
		protected void Awake () { 
			Connect();
			if (enterThing == null) enterThing= gameObject;
			if (enterActive) enterThing.SetActive( !onEnter); // not triggered at start
			camTransform = Camera.main.transform;
			transform = GetComponent<Transform>();
			holdOrigin = camTransform;
		}
		
		
		protected override void OnEnable(){
			base.OnEnable();
			if (ih == null) return;
			ih.onRelease += Release;
			
			if (pushing)
				ih.doWhilePressed += Push;
			
		}
		protected override void OnDisable(){
			
			base.OnDisable();
			
			ih.onRelease -= Release;
		
			if (pushing)
				ih.doWhilePressed -= Push;
			
			if (enterActive) enterThing.SetActive( !onEnter); 
		}
		
		
		protected void PressCheck() {
			
			bool block;
			// If I'm blocking the next check then I return early, this is opposed to an else, else if chain.
			
			block = (blockNextOnPress && ih.pressed);
			
			if ( block ) 
			{
				if (pullOnPress)
					PullInRange();
				return;
			}
			else if (pullOnPress && ih.pressed)
			{
				PullInRange();
				return;
			}
			
			block = (blockNextOnActive && ih.isActive);
			
			if (block )
			{
				if (pullOnActive)
					PullInRange();
				return;
			}
			else if (pullOnActive && ih.isActive)
			{
				PullInRange();
				return;
			}
			
			block = (blockNextOnHover && ih.isHovered);
			
			if (block)
			{
				if (pullOnHover)
				PullInRange();
				return;
			}
			else if (pullOnHover && ih.isHovered)
			{
				PullInRange();
				return;
			}
				
		}
		protected override void OnLateUpdate() {
			
			if (pullInRangeAlways)
			{
				PullInRange();
			}
			else
			{
				PressCheck();
				
			}
			
			
			if (keepAboveOrigin && transform.position.y < holdOrigin.position.y)
			{
				Vector3 pos = transform.position;
				pos.y = holdOrigin.position.y;
				transform.position = pos;
			}
			
		}
		protected void Release(HSCxController caller) {
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
			Vector3 push = camTransform.forward * pushSpeed * Time.deltaTime;
			transform.position += push;
		}
		public bool keepAboveOrigin = true;
		protected void PullInRange(HSCxController caller) {PullInRange();}
		protected void PullInRange() {
			float d = Vector3.Distance(holdOrigin.position, transform.position);
			
			if (  d > range  )
			{
				
				Vector3 goal = Vector3.MoveTowards(transform.position, holdOrigin.position, d - range); // extra distance after subtracting the max
				
				transform.position = Vector3.MoveTowards(transform.position, goal, pullSpeed * Time.deltaTime); // extra distance after subtracting the max
			}
			
			
			
		}
		
		
			
		 
	}
}