using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utility.Managers;
using NPC.Strategy;
using MEC;

namespace BehaviorTree
{
	public class BTReturnDelay : BTNode {

		// pings after there is no success for a given time
		
		
		
		public float timer = 10f;
		public float delay = 10f;
		protected IEnumerator<float> SucceedAfter () {
			yield return Timing.WaitForSeconds(delay);
			Succeed();
			
		}
		protected override void OnEnable() {
			
			EnableChildren();
			timer = delay;
			
			Timing.RunCoroutine(SucceedAfter());
		}
		protected override void OnDisable() {
			
			DisableChildren();
			Timing.KillCoroutines("SucceedAfter");
			
		}
		
		protected override void OnSuccess(){
			timer = delay;
			Timing.KillCoroutines("SucceedAfter");
			Timing.RunCoroutine(SucceedAfter());
		}
		protected override void OnFailure(){}
		
		
		
	}
}