using UnityEngine;
using System.Collections;

namespace NPC.BTree
{
	public class BTRepeat : BTNode {

		public float delay = 0f;
		public int exitF = 0;
		public int exitS = 0;
		public int f = 0;
		public int s = 0;
		
		
		protected void OnEnable () {
			EnableChildren();
		}
		protected void OnDisable () {
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
			gameObject.SetActive(false);
		}
	
		protected void Repeat(){
			if (gameObject.activeInHierarchy)
				StartCoroutine("_Repeat");
		}
			
		protected IEnumerator _Repeat(){
			DisableChildren();
			yield return null; // force at least one frame
			yield return new WaitForSeconds(delay);
			EnableChildren();
		}
		
		
	}
}