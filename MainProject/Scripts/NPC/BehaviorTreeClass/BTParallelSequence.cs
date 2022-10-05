using UnityEngine;
using System.Collections;
using Utility.Managers;

namespace BehaviorTree
{
	public class BTParallelSequence : BTNode {
		///<summary> assumes starting from first child
		///	assumes continue on fail
		///</summary>
	
		// I can bool/ return success or fail, but that might make more sense with an extended class
	
		public int fails = 0;
		public int successes = 0;
		public int count = 0;
		public bool resetOnDisable = true;
		
		public int exitF = 0;
		public int exitS = 0;
		
		
		public bool failDisablesChild = true;
		public bool successDisablesChild = false;
		public bool repeatFailures = false; // this will accrue more failures
		
		public bool loopReset = true;
		
		
		public bool wait = false;
		public float waitTime = -1f;
		protected float timer = 0f;
		
		protected bool running = false;
		protected bool waiting = false;
		
		protected override void OnEnable () {
			if (children.Count < 1) return;
			
			GeneralUpdateManager.onUpdate += OnUpdate;
			children[count].SetActive(true);
			
			running = true;
			timer = waitTime;
			waiting = wait;
			
		}
		protected override void OnDisable () {
			GeneralUpdateManager.onUpdate -= OnUpdate;
			
			if (resetOnDisable && !running)
			{
				count = 0;
				successes = 0;
				fails = 0;
				running = false;
			}
			
			DisableChildren();
		}
		
		
		protected void ResetOnLoop(){
			if (!loopReset) return;
			
			fails = (repeatFailures) ? 0 : fails;
			
		}
		
		protected override void OnSuccess(BTNode b){
			if (!activeInHierarchy) return;
			successes ++;
			if (exitS > 0 && successes >= exitS) { running = false; Success(); return; }
			
			
			
			if (successDisablesChild) {
				b.SetActive(false);
			}
			waiting = false;
		}
		protected override void OnFailure(BTNode b){
			
			fails ++;
			if (exitF > 0 && fails >= exitF) {running = false; Fail(); return; }
			
			
			
			
			if (failDisablesChild) {
				b.SetActive(false);
			}
			
			waiting = false;
		}
		
		protected virtual void Next () {
			// is enabled
			
			
			if (waiting)
			{
				if (waitTime < 0) return; // means I need something to stop me waiting.
				timer -= Time.deltaTime;
				if (timer > 0)
					return;
			}

			
			count ++;
			count = count % children.Count;
			
			if ( count == 0) 
			{
				if (!repeatFailures )
				{
					running = false;
					return;
				}
				else
				{
					ResetOnLoop();
				}
					
			}
			// is reset to first child
			// next is set on
			
			children[count].SetActive(true);
			timer = waitTime;
			waiting = wait;
		}
		
		protected void OnUpdate() {
			// no timer, but this will call them in order, so long as it is active.
			if (!activeInHierarchy || !running)
				GeneralUpdateManager.onUpdate -= OnUpdate;
			else
			{
				Next();
			}
		}
		
	}
}