using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MEC;


namespace BehaviorTree 
{
	public class BTChainDelay : BTNode {
		// delay enabling child

	
		public float delay = .2f;
		
		
		protected override void OnEnable() 
		{
			if (delay.IsZero()) 
				EnableChildren();
			else
				StartCoroutine(_OnEnable);
		}
		
		protected IEnumerator<float> _OnEnable () {
			yield return Timing.WaitForSeconds(delay);
			EnableChildren();
		}
		protected override void OnDisable () {
			StopCoroutine("_OnEnable");
			DisableChildren();
		}
	}
}