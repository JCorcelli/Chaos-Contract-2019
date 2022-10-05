using UnityEngine;
using System.Collections;
using Utility.Managers;
using NPC.Strategy;

namespace NPC.BTree
{
	public class BTReturnDelay : BTUpdateNode {

		// this is like Pinger, but it never returns until explicitly programmed.
		
		
		protected override void OnEnable() {
			// all
			base.OnEnable();
			EnableChildren();
			timer = delay;
		}
		protected override void OnDisable() {
			// all
			base.OnDisable();
			DisableChildren();
			
		}
		
		protected override void OnSuccess(){
			timer = delay;
		}
		protected override void OnFailure(){}
		
		public float timer = 10f;
		public float delay = 10f;
		protected override void OnUpdate(){
			timer -= Time.deltaTime;
			timer = Mathf.Clamp(timer, 0, delay);
			if (timer.IsZero()) Succeed();
		}
		
	}
}