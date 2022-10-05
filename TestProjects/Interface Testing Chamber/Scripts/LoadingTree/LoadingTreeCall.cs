using UnityEngine;
using System.Collections;

namespace Utility.Tree
{
	
	public class LoadingTreeCall : MonoBehaviour {
		public string level = "";
		protected string nextLevel = "";
	
		
		
		protected void Awake() {
			
			if (level == "") Debug.LogError("Level not entered.");
			else nextLevel = level;
		
		}
		protected void CallLoad() {
			
			LoadingTree.Load(nextLevel, false);
			
		}
		
	}

}