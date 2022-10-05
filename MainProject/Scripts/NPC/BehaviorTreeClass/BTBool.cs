using UnityEngine;
using System.Collections;

namespace BehaviorTree
{
	public class BTBool : BTNode {

		// true/false instantly
		
		// reverse result
		public bool reverse = false;
		
		// force a success
		public bool succeed = false;
		
		
		protected override void OnEnable(){
			EnableChildren();
		}
		protected override void OnDisable(){
			DisableChildren();
		}
		
		
		
		protected override void OnSuccess(){Pop(true);}
		protected override void OnFailure(){Pop(succeed || false);}
		protected void Pop(bool b = true) {
			if (b != reverse)
				Succeed();
			else
				Failure();
		}
		
		
	}
}