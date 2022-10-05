using UnityEngine;
using System.Collections;


namespace SelectionSystem.IHSCx
{
	
	
	public class IHSCxHoverSubtle : IHSCxConnect {


		public GameObject other;
		public bool dragFilter = true;
		public bool allowRelease = true;
		
		protected override void OnEnable() {
			if (other == null) 
			{
				enabled = false;
				return;
			}
			base.OnEnable();
			if (ih == null) return;
			
			ih.onEnter += Enter;
			ih.onExit += Exit;
			
			CheckTime();
		}
		
		protected override void OnDisable() {
			base.OnDisable();
			
			if (ih == null) return;
			ih.onEnter -= Enter;
			ih.onExit -= Exit;
			
			// turning off object breaks couroutine anyway, keep the time enabled true
			StopCoroutine("DelayOff");
		}
		
		protected void Enter(HSCxController caller) {
			if (dragFilter && Input.GetButton("mouse 1")) return;
			Enter();
		}
		
		protected void Enter() {
			
			other.SetActive(true);
			countingDown = false;
			StopCoroutine("DelayOff");
		}
		
		protected override void OnUpdate() {
			if (allowRelease && Input.GetButtonUp("mouse 1") && ih.isHovered) 
				Enter();
			
		}
		
		
		protected void Exit(HSCxController caller) {
			timeExit = Time.time;
			countingDown = true;
			StartCoroutine("DelayOff", delay);
		}
		
		protected void OtherOff() {
			other.SetActive(false);
			countingDown = false;
			
		}
		protected void CheckTime(){
			if (countingDown)
			{
				float spentTime = Time.time - timeExit;
				if (spentTime > delay)
				{
					OtherOff();
				}
				else
					StartCoroutine("DelayOff", spentTime);
					
			}
				
		}
		
		public float delay = 1f;
		protected float timeExit = 1f;
		protected bool countingDown = false;
		protected IEnumerator DelayOff(float timedelay) {
			if (timedelay > 0)
				yield return new WaitForSeconds(timedelay);
			OtherOff();
			
		}
		
		
	}
}