using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MEC;

namespace BehaviorTree
{
	public class BTWait : BTNode {
		/// <summary>
		///	waits for delay.
		/// succeeds or fails after delay
		/// </summary
	
		public float delay = 2f;
		public bool successState = true; // success or failure occurred while running
		public bool trigger = false;
		
		protected bool running = false;
		// Update is called once per frame
		protected override void OnEnable () {
			EnableChildren();
			StartCoroutine(SleepTime);
		}
		protected override void OnDisable () {
			DisableChildren();
			StopCoroutine("SleepTime");
			successState = false;
			if (running) Finish();
			running = false;
		}
		protected void Finish() {
			if (successState) Success(); else Failure();
			
		}
		protected IEnumerator<float> SleepTime() {
			if (running) yield break;
			running = true;
			yield return Timing.WaitForSeconds(delay);
			running = false;
			Finish();
		}
		
		protected override void OnSuccess(){
			if (!running) return;
			successState = true;
			if (trigger) 
			{
				StopCoroutine("SleepTime");
				running = false;
				Succeed();
			}
		}
		protected override void OnFailure(){
			if (!running) return;
			successState = false;
			if (trigger) 
			{
				
				StopCoroutine("SleepTime");
				running = false;
				Fail();
			}
		}
		
	}
}