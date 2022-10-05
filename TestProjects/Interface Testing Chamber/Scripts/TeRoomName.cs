using UnityEngine;
using System.Collections;
using UnityEngine.Events;

using UnityEngine.UI;
using Utility.Tree;

namespace Utility
{
	public class TeRoomName : MonoBehaviour {

		
		// this'll be great for UI debug / banner text
		
		protected Text t;
		

		protected void Awake() { 
			t = GetComponent<Text>(); 
		}
		
		protected virtual void SetText(string newText) { 
			t.text = newText;
		}
		
		
		protected void Update() {
			SetText("Scene: "+LoadingTree.level);
		}
	}

}