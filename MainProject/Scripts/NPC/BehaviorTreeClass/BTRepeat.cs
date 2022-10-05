using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MEC;

namespace BehaviorTree
{
	public class BTRepeat : BTNode {

		public float delay = 0f;
		public int exitF = 0;
		public int exitS = 0;
		public int f = 0;
		public int s = 0;
		
		
		protected override void OnEnable () {
			EnableChildren();
		}
		protected override void OnDisable () {
			DisableChildren();
			f = 0;
			s = 0;
		}
		protected override void OnSuccess(){
			s++;
			if (exitS >0 && s >= exitS) 
				Success();
			Repeat();
		}
		protected override void OnFailure(){
			f++;
			if (exitF >0 && f >= exitF) 
				Fail();
			Repeat();
		}

		protected void OnApplicationQuit () {
			SetActive(false);
		}
	
		protected void Repeat(){
			if (activeInHierarchy)
				StartCoroutine(_Repeat);
		}
			
		protected IEnumerator<float> _Repeat(){
			DisableChildren();
			yield return 0; // force at least one frame
			yield return Timing.WaitForSeconds(delay);
			EnableChildren();
			
		}
		
		
	}
}