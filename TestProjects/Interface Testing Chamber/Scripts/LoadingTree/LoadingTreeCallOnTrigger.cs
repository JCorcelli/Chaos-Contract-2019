using UnityEngine;
using System.Collections;

namespace Utility.Tree
{
	
	public class LoadingTreeCallOnTrigger : MonoBehaviour {
		public string level = "";
		protected string nextLevel = "";
	
		
		
		
		protected void Awake() {
			
			if (level == "") Debug.LogError("Level not entered.");
			else nextLevel = level;
		
		}
		protected void CallLoad() {
			
			LoadingTree.Load(nextLevel, false);
			
		}

		public string targetName = "PlayerCollider";
		
		
		public int count = 0; 

		void OnTriggerEnter( Collider col ) {
			if (targetName == col.name) {
				count ++;
					CallLoad();
				
			}
		}
		
		
	}

}