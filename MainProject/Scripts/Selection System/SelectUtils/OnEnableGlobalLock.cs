using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace SelectionSystem 
{
	public class OnEnableGlobalLock : MonoBehaviour {

		// Use this for initialization
		void OnEnable() {
			
			SelectGlobal.locked = true;
		}
		void OnDisable() {
			SelectGlobal.locked = false;
		}
	}
}