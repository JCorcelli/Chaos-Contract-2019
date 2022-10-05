using UnityEngine;
using System.Collections;
using Utility.Managers;

namespace NPC.BTree.Jerri
{
	public class JerriBeLeaf : BTUpdateNode {

		// this is like Pinger, but it never returns until explicitly programmed.
		/*
		protected virtual void OnEnable() {
			// all
			if (parentNode == null) return;
			GeneralUpdateManager.onUpdate += OnUpdate;
			
		}
		protected virtual void OnDisable() {
			// all
			if (parentNode == null) return;
			GeneralUpdateManager.onUpdate -= OnUpdate;
			
		}
		*/
		protected override void OnUpdate(){
			
		}
		
	}
}