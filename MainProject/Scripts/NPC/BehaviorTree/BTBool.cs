using UnityEngine;
using System.Collections;

namespace NPC.BTree
{
	public class BTBool : BTNode {

		// Use this for initialization
		
		public bool reverse = false;
		public bool succeed = false;
		
		
		protected virtual void OnEnable(){
			EnableChildren();
		}
		protected virtual void OnDisable(){
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