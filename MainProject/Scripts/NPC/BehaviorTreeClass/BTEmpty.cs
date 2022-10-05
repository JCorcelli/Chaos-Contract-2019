using UnityEngine;
using System.Collections;

namespace BehaviorTree
{
	public class BTEmpty : BTNode {

		// Enable/Disable only
		protected override void OnEnable(){
			EnableChildren();
			
		}
		
		
		protected override void OnDisable(){
			DisableChildren();
			
		}
		
		
		
	}
}