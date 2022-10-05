using UnityEngine;
using System.Collections;

namespace Utility.Tree
{
	
	public class LoadingPersonal : MonoBehaviour {

	
		public void LoadPersonal(string newLevel, bool additive) {
			LoadingTree.Load(newLevel, additive);
			
		}
		public void LoadPersonal(string newLevel) {
			LoadingTree.Load(newLevel);
		}
		
		
	}

}