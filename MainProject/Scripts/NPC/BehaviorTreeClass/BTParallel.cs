using UnityEngine;
using System.Collections;

namespace BehaviorTree
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
		protected override void OnEnable () {
			running = true;
			foreach (BTNode child in children)
			{
				if (!activeInHierarchy) break; // succeeded or failed
				child.SetActive(true);
			}
		}
		protected override void OnDisable () {
			
			
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
			if (!activeInHierarchy) return;
			if (exitF> 0 && f >= exitF)
			{
				running = false;
				
				Fail(); // probably disables this
			}
			
			if (failureDisablesChild) b.SetActive(false);
		}
		protected override void OnSuccess(BTNode b) {
			s++;
			Total();
			if (exitS > 0 && s >= exitS)
			{
				running = false;
				
				Succeed(); // probably disables this
			}
			
			if (successDisablesChild) b.SetActive(false);
		}
		protected void Total(){
			if (!exitOnTotal) return;
			if (s + f >= children.Count) 
			{
				running = false;
				
				Succeed();
			}
		}
	}
}