using UnityEngine;
using System.Collections;

public class SelectableProp : MonoBehaviour {

	public void Select() {
		BroadcastMessage("SetFocus", true);
		
	}
	public void Deselect() {
		BroadcastMessage("SetFocus", false);
		
	}
}
