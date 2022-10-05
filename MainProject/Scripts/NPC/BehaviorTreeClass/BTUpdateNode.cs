using UnityEngine;
using System.Collections;
using Utility.Managers;
using NPC.Strategy;

namespace BehaviorTree
{
	public class BTUpdateNode : BTNode {

		// this is like Pinger, but it never returns until explicitly programmed.
		
		
		protected override void OnEnable() {
			// all
			
			GeneralUpdateManager.onUpdate += OnUpdate;
			
		}
		protected override void OnDisable() {
			// all
			
			GeneralUpdateManager.onUpdate -= OnUpdate;
			
		}
		
		protected virtual void OnUpdate(){
			
		}
		
	}
}