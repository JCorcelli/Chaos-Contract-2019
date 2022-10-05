using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


namespace SelectionSystem
{
	
	
	public class OneButtonDelayed: SelectAbstract {
		
		public bool useAnim = true;
		public float delay = .1f;
		
		public bool running = false;
		public bool started = false;
		
		
		protected Animator _anim;
		protected override void OnEnable(){
			base.OnEnable();
			if (_anim== null) _anim = GetComponentInParent<Animator>();
		}
		protected override void OnDisable(){
			base.OnDisable();
			
			//Debug.Log(pressed.ToString() + running.ToString() + started.ToString());
			pressed = false;
			running = false;
			started = false;
			StopCoroutine("DelayedCress");
			
			if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == this.gameObject) EventSystem.current.SetSelectedGameObject(null);
			
			
		}
		
		protected float delayFromHover = .1f;
		protected float hoverEnd = 0f;
		protected override void OnEnter() {
			hoverEnd = delayFromHover;
			
		}
		protected override void OnExit() {
			if (!mustWait)return;
			
			OnRelease();
			if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == this.gameObject) EventSystem.current.SetSelectedGameObject(null);
			
		}
		
		
		
		public bool mustHold = true;
		public bool mustWait = true;
		protected override void OnRelease() {
			// it's onrelease, but it's being called from two 
			
			
			if (mustHold && running)
			{
				StopCoroutine("DelayedCress");
				running = false;
			}
			
		}
		
		
		protected void AnimMethod(){
			
			if (_anim.IsInTransition(0))
			{
				return;
				
			}
			StartCoroutine("DelayedCress");
		}
		
		protected IEnumerator DelayedCress(){
			if (delay >.01f)
				yield return new WaitForSeconds(delay);
			
			
			OnCall();
			running = false;
			
			yield  return null;
		}
		
		protected override void OnUpdate() {
			base.OnUpdate();
			
			// the precision's equivalent to letting go
			
			
			if (running) 
			{
				if (useAnim) AnimMethod();
				return;
			}
			
			// hover delaying
			hoverEnd -= Time.deltaTime;
			if (hoverEnd < 0f) hoverEnd = -1f;
			else return;
			
			if (useAnim && mustWait)
			{
				//if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("Highlighted")) return;
				//
				//float nTime = _anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
				
				float nTime = _anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
				if (_anim.IsInTransition(0) || nTime < .98f)
				{
					return;
					//running = false;
					//_canvasGroup.interactable = IsOpen();
					//_anim.enabled = false; // opp Play() / StartPlayback (related to recording something)
				}
			
			}
			
			// started, in case I don't need to hold it.
			if (pressed) started = true;
			
			
			if (started && hoverEnd < 0f && !running)
			{
				started = false;
				if (!useAnim) StartCoroutine("DelayedCress");
				running = true;
				
			}
		}
		
		protected virtual void OnCall(){
			
		}
		 
	}
}