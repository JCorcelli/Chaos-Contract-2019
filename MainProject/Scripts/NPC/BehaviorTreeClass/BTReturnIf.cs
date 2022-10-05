using UnityEngine;
using System.Collections;

namespace BehaviorTree
{
	public class BTReturnIf : BTNode {

		// Use this for initialization
		public bool returnSuccess = false;
		public bool returnFail = false;
		
		protected override void OnEnable(){
			EnableChildren();
			
		}
		
		
		protected override void OnDisable(){
			DisableChildren();
			
		}
		protected override void OnSuccess(){
			if (returnSuccess) Succeed();
		}
		protected override void OnFailure(){
			
			if (returnFail) Fail();
		}
		
		
		
	}
}