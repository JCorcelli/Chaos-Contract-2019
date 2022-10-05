using UnityEngine;
using System.Collections;

namespace SelectionSystem
{
	public class OnPressCycleChildren : AbstractButtonHandler {

		
		protected GameObject child;
		
		
		public int count = 0;
		void Awake() { 
			child = transform.GetChild(count).gameObject; 
			if (transform.childCount < 2) Debug.Log(name + " should really have more children",gameObject);
		}
		protected override void OnPress () {
			if (!gameObject.activeInHierarchy || SelectGlobal.locked ) return;
			child.SetActive(false);
			count ++;
			count = count % transform.childCount;
			child = transform.GetChild(count).gameObject; 
			child.SetActive(true);
		}
		
	}
}