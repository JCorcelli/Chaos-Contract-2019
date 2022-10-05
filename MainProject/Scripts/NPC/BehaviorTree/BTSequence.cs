using UnityEngine;
using System.Collections;

namespace NPC.BTree
{
	public class BTSequence : BTNode {
		///<summary> assumes starting from first child
		///	assumes continue on fail
		///</summary>
	
		// I can bool/ return success or fail, but that might make more sense with an extended class
	
		public int fails = 0;
		public int successes = 0;
		public int count = 0;
		public bool resetOnDisable = true;
		public int exitF = 0;
		public int exitS = 0;
		// Update is called once per frame
		protected bool running = false;
		
		protected void OnEnable () {
			
			if (transform.childCount < 1) return;
			
			if (!resetOnDisable)
				return;
			else
			{
				successes = 0;
				fails = 0;
				count = 0;
				running = false;
			}
			
			EnableChild(count);
			
			running = true;
		}
		protected void OnDisable () {
			
			if (transform.childCount < 1) return;
			
			DisableChild(count);
				
			
		}
		protected override void OnSuccess(BTNode b){
			successes ++;
			if (exitS > 0 && successes >= exitS) { running = false; Success(); return; }
			b.gameObject.SetActive(false);
			Next();
		}
		protected override void OnFailure(BTNode b){
			fails ++;
			if (exitF > 0 && fails >= exitF) {running = false; Fail(); return; }
			b.gameObject.SetActive(false);
			Next();
		}
		protected virtual void Next () {
			// is enabled
			
			if (!gameObject.activeInHierarchy ) 
				
			{
				
				return;
			
			}
			
			count ++;
			count = count % transform.childCount;
			
			// is reset to first child
			if (count == 0) 
			{
				running = false;
				
				Success();
				
				
				return;
			}
			// next is set on
			
			transform.GetChild(count).gameObject.SetActive(true);
		}
		
	}
}