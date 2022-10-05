using UnityEngine;
using System.Collections;

namespace NPC.BTree
{
	public class BTParallel: BTNode {

		protected bool running = false;
		public int exitS = 0;
		public int exitF = 0;
		public int s = 0;
		public int f = 0;
		
		
		public bool successDisablesChild = false;
		public bool failureDisablesChild    = true;
		public bool exitOnTotal = false;
		
		public bool resetVars = true;
		protected void OnEnable () {
			running = true;
			foreach (Transform child in transform)
			{
				if (!gameObject.activeInHierarchy) break; // succeeded or failed
				child.gameObject.SetActive(true);
			}
		}
		protected void OnDisable () {
			
			
			if (resetVars)
			{
				f = 0;
				s = 0;
			}
				
			DisableChildren();
		}
		
		protected override void OnFailure(BTNode b) {
			f++;
			Total();
			if (!gameObject.activeInHierarchy) return;
			if (exitF> 0 && f >= exitF)
			{
				running = false;
				
				Fail(); // probably disables this
			}
			
			if (failureDisablesChild) b.gameObject.SetActive(false);
		}
		protected override void OnSuccess(BTNode b) {
			s++;
			Total();
			if (exitS > 0 && s >= exitS)
			{
				running = false;
				
				Succeed(); // probably disables this
			}
			
			if (successDisablesChild) b.gameObject.SetActive(false);
		}
		protected void Total(){
			if (!exitOnTotal) return;
			if (s + f >= transform.childCount) 
			{
				running = false;
				
				Succeed();
			}
		}
	}
}