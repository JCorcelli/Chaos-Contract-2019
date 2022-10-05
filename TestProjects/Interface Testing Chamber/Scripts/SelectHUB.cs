using UnityEngine;
using System.Collections;

namespace Utility.Tree
{
	public class SelectHUB : MonoBehaviour {

		
		
			// will run after reaching end of list
		public bool repeat = false;
			// has an active child
		public bool running = false;
		
			// will run when enabled
		public bool ready = true;
		public int count = 0;
		protected GameObject child;
		protected void Awake() { 
		
			child = transform.GetChild(count).gameObject; 
			if (transform.childCount < 2) Debug.Log(name + " should really have more children");
		}
		
		protected void OnEnable() {
			
			// has the last child finished
			if (running || ready) Next();
		}
		protected void OnDisable() {
			count = 0;
			foreach (Transform t in transform)
				t.gameObject.SetActive(false);
		}
		public void Ready () {
			
			// prep, in case this is disabled
			ready = true;
			
			// the current child finished
			running = false;
			Next();
		}
		protected virtual void Next () {
			// is enabled
			if (!gameObject.activeInHierarchy ) return;
			
			// is cycling to next child
			ready = false;
			
			child.SetActive(false);
			count ++;
			count = count % transform.childCount;
			
			// is reset to first child
			if (count == 0 && !repeat) 
			{
				foreach (Transform t in transform)
					t.gameObject.SetActive(false);
					
				return;
			
			}
			// has an active child
			running = true;
			
			// turns on next child
			child = transform.GetChild(count).gameObject; 
			child.SetActive(true);
		}
		
	}
}