using UnityEngine;
using System.Collections;

namespace Utility.AnimatorEffects
{
	public class MTDelaySetAnimatorVariable : MecanimTriggerNode {

	
		public string varName = "playing";
		public bool valueOnEnter = true;
		
		public bool valueOnExit = false;
		public float delay = 1f;
		
		protected override void onTriggerEnter () {
			
			StartCoroutine("_onTriggerEnter");
			
		}
		
		
		protected override void onTriggerExit () {
			
			StopCoroutine("_onTriggerEnter");
			delayPlayRunning = false;
			
			anim.SetBool(varName, valueOnExit);
			
		}
		
		
		protected bool delayPlayRunning = false;
		protected IEnumerator _onTriggerEnter () {
			if (delayPlayRunning) yield break;
			delayPlayRunning = false;
			yield return new WaitForSeconds(delay);
			anim.SetBool(varName, valueOnEnter);
			delayPlayRunning = false;
		}
	}
}