using UnityEngine;
using System.Collections;
using Utility.Managers;

namespace BehaviorTree 
{
	public class BTDecor : BTNode {


		public bool current 	= false;
		public bool pingChangeOnly = true;
		
		protected bool prev 	= false;
		public bool firstCall = true;	
		

		protected override void OnEnable()

		{
			firstCall = true;
			GeneralUpdateManager.onUpdate += _OnUpdate;
			
			
		}
			
		protected override void OnDisable () {
			
			DisableChildren();
			firstCall = true;
			GeneralUpdateManager.onUpdate -= _OnUpdate;
		}

		protected void Ping(){
			if (!firstCall && pingChangeOnly && prev == current) return;
			prev = current;
			firstCall = false;
			
			if (current) 
			{
				EnableChildren();
				Success();
			}
			else
			{
				Fail();
			}
		}
		protected void _OnUpdate() {
			OnUpdate();
			Ping();
		}
		
		protected virtual void OnUpdate() {
		}
		
		protected override void OnSuccess(){}
		protected override void OnFailure(){}
	}
}