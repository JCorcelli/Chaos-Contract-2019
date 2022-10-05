using UnityEngine;
using System.Collections;


namespace SelectionSystem.IHSCx
{
	
	
	public class IHSCxHoverToggleOtherDelayOff : IHSCxConnect {


		public GameObject other;
		
		protected override void OnEnable() {
			Connect();
			if (ih == null) return;
			ih.onEnter += Enter;
			ih.onExit += Exit;
			
			CheckTime();
		}
		
		protected override void OnDisable() {
			
			if (ih == null) return;
			ih.onEnter -= Enter;
			ih.onExit -= Exit;
			
			// turning off object breaks couroutine anyway, keep the time enabled true
			StopCoroutine("DelayOff");
		}
		
		
		protected void Enter(HSCxController caller) {
			
			other.SetActive(true);
			countingDown = false;
			StopCoroutine("DelayOff");
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
			yield return new WaitForSeconds(timedelay);
			OtherOff();
			
		}
		
		
	}
}