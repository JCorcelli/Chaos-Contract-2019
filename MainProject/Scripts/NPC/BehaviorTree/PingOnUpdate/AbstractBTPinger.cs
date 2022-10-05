using UnityEngine;
using System.Collections;
using Utility.Managers;

namespace NPC.BTree
{
	public class BTPinger : BTNode {

		// This will return at least once the first frame it's called.  calls repeated when pingChangeOnly = false.
		
		public bool current 	= false;
		public bool pingChangeOnly = true;
		
		protected bool prev 	= false;
		protected bool firstCall = true;
		
		protected virtual void OnEnable()

		{
			firstCall = true;
			GeneralUpdateManager.onUpdate += _OnUpdate;
			
			
		}
			
		protected virtual void OnDisable () {
			
			firstCall = true;
			GeneralUpdateManager.onUpdate -= _OnUpdate;
		}

		protected void Ping(){
			if (!firstCall && pingChangeOnly && prev == current) return;
			prev = current;
			firstCall = false;
			if (current) Succeed();
			else
				Fail();
		}
		protected void _OnUpdate() {
			OnUpdate();
			Ping();
		}
		
		protected virtual void OnUpdate() {
			
		}
			
	}
}