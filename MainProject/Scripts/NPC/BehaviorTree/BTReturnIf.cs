using UnityEngine;
using System.Collections;

namespace NPC.BTree
{
	public class BTReturnIf : BTNode {

		// Use this for initialization
		public bool returnSuccess = false;
		public bool returnFail = false;
		
		protected void OnEnable(){
			EnableChildren();
			
		}
		
		
		protected void OnDisable(){
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