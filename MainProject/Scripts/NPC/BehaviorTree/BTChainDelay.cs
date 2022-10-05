using UnityEngine;
using System.Collections;

namespace NPC.BTree 
{
	public class BTChainDelay : BTNode {


	
		public float delay = .2f;
		
		
		protected void OnEnable() 
		{
			if (delay.IsZero()) 
				EnableChildren();
			else
				StartCoroutine("_OnEnable");
		}
		
		protected IEnumerator _OnEnable () {
			yield return new WaitForSeconds(delay);
			EnableChildren();
		}
		protected void OnDisable () {
			StopCoroutine("_OnEnable");
			DisableChildren();
		}
	}
}