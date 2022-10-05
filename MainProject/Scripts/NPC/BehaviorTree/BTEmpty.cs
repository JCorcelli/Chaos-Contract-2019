using UnityEngine;
using System.Collections;

namespace NPC.BTree
{
	public class BTEmpty : BTNode {

		// Use this for initialization
		protected void OnEnable(){
			EnableChildren();
		}
		
		
		protected void OnDisable(){
			DisableChildren();
			
		}
		
		
		
	}
}