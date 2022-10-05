using UnityEngine;
using System.Collections;
using Utility.Managers;
using NPC.Strategy;

namespace NPC.BTree
{
	public class BTUpdateNode : BTNode {

		// this is like Pinger, but it never returns until explicitly programmed.
		
		
		protected virtual void OnEnable() {
			// all
			
			GeneralUpdateManager.onUpdate += OnUpdate;
			
		}
		protected virtual void OnDisable() {
			// all
			
			GeneralUpdateManager.onUpdate -= OnUpdate;
			
		}
		
		protected virtual void OnUpdate(){
			
		}
		
	}
}