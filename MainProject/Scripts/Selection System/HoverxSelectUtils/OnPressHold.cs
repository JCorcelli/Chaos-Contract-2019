using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

namespace SelectionSystem
{
	
	public class OnPressHold:  AbstractButtonComboPrecision, IPointerDownHandler {

		public bool pressed = false;
		public void  OnPointerDown( PointerEventData eventData ) {
			pressed = true;
			
		}
		protected override void OnUpdate() {
			base.OnUpdate();
			if (!button_held || !combo_held)
				pressed = false;
		}
		
		protected RaycastHit _hit;
		public RaycastHit hit {get {return _hit;} set{_hit = value;}}
		protected Ray _ray;
		
		public Ray ray {get {return _ray;} set{_ray = value;}}
		
		protected override void OnDisable(){
			base.OnDisable();
		
			pressed = false;
			combo_held = false;
			
			
		}
		
		public void Raycast() {
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			// set hit / ray values
			
			Physics.Raycast(ray, out _hit, Mathf.Infinity, Camera.main.eventMask);
			
			
		}
		
		public bool mustHold = false;
		protected override void OnRelease() {
			
			pressed = false;
			
			Raycast();
			
			if (mustHold && running)
			{
				StopCoroutine("DelayedCall");
				running = false;
			}
			
			
		}
		
		// instead of update. this is called when conditions are right
		public bool running = false;
		public float delay = .1f;
		protected override void OnHold() {
			if (pressed && !running)
			{
				StartCoroutine("DelayedCall");
				running = true;
				
			}
			
		}
		protected IEnumerator DelayedCall(){
			if (delay >.01f)
				yield return new WaitForSeconds(delay);
			
			OnCall();
			
			yield  return null;
		}
		protected virtual void OnCall(){
			
		}
		
		
		
	}
}