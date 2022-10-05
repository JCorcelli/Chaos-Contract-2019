using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MEC;


namespace BehaviorTree
{
	public class BTSleep : BTNode {

		public float delay = 2f;
		protected bool running = false;
		// Update is called once per frame
		protected override void OnEnable () {
			StartCoroutine(SleepTime);
		}
		protected override void OnDisable () {
			StopCoroutine("SleepTime");
			if (running) Failure();
		}
		
		protected IEnumerator<float> SleepTime() {
			running = true;
			yield return Timing.WaitForSeconds(delay);
			running = false;
			
			Succeed(); // probably disables this
		}
	}
}