using UnityEngine;
using System.Collections;


namespace SelectionSystem
{
	public class HPhysicClient : MonoBehaviour {

		new protected Transform transform;
		new protected Rigidbody2D rigidbody2D; // optional
		
		public bool isActive = false;
		public bool isHovered = false;
		public bool pressed = false;
		
		protected virtual void Awake() {
			transform = GetComponent<Transform>();
			
			// this is a bit of a hack since the class is getting less and less abstract
			rigidbody2D = GetComponent<Rigidbody2D>();
			if (rigidbody2D != null)
			{
				HPhysicPress.press += PhysicPress;
				HPhysicPress.touch += PhysicTouch;
			}
		}
		protected void OnDestroy() {
			if (rigidbody2D != null)
			{
				HPhysicPress.press -= PhysicPress;
				HPhysicPress.touch -= PhysicTouch;
			}
		}
		protected bool hasFocus = false;
		public void OnTriggerEnter2D(Collider2D col){
			if (HPhysicPress.available && col.name == "MouseHotSpot") 
			{
				HPhysicPress.available = false;
				HPhysicPress.touching = this.gameObject;
				hasFocus = true;
				Enter();
			}
		}
		public void  Enter() {
			_OnEnter();
		}
		public virtual void  _OnEnter() {
			isHovered = true;
			
			OnEnter();
		}
		public virtual void  OnEnter() {}
		
		public void OnTriggerExit2D(Collider2D col){
			if (hasFocus && col.name == "MouseHotSpot") 
			{
				HPhysicPress.available = true;
				HPhysicPress.instance.Touch();
				hasFocus = false;
				Exit();
			}
		}
		public void Exit() {_OnExit();}
		public void _OnExit() {
			isHovered = false;
			OnExit();
			}
		public virtual void  OnExit() {}
		
		public void PhysicTouch(){
			if (!HPhysicPress.available || hasFocus) return;// because this is the object that called it
			
			

			if (rigidbody2D.IsTouching(HPhysicPress.instance.GetCollider()))
			{
				// duplicate
				HPhysicPress.available = false;
				HPhysicPress.touching = this.gameObject;
				hasFocus = true;
				Enter();
			}
			
		}
		public void PhysicPress() {
			if (hasFocus) Press();
		}
		public void Press() {OnPress();}
		public virtual void  OnPress() {}

		protected void OnDisable(){
		
			if (hasFocus)
			{
				HPhysicPress.available = true;
				HPhysicPress.instance.Touch();
				hasFocus = false;
			}
			pressed = false;
			_OnExit();
			
		}		
		
		
	}
}