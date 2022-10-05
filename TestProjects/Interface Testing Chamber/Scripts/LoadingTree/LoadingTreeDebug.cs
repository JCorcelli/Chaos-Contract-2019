using UnityEngine;
using System.Collections;

namespace Utility.Tree
{
	
	public class LoadingTreeDebug : MonoBehaviour {
		public string nextLevel = "";
	
		protected string start = "AutoLoader";
		protected string level = "RoomWithABox";
		
		protected string level2 = "RoomWith2DInterface";
		protected string level3 = "RoomWith3DMeld";
		
		public float delay = 5f;
		
		protected IEnumerator Start() {
			LoadingTree.level = start;
			
			nextLevel = level;
			yield return new WaitForSeconds(delay);
			
			LoadingTree.Load(nextLevel, true);
			
			nextLevel = level2;
			yield return new WaitForSeconds(delay);
			LoadingTree.Load(nextLevel, true);
			
			nextLevel = level3;
			yield return new WaitForSeconds(delay);
			LoadingTree.Load(nextLevel, false);
		}
		
	}

}