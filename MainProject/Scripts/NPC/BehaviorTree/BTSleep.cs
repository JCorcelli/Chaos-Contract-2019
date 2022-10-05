using UnityEngine;
using System.Collections;

namespace NPC.BTree
{
	public class BTSleep : BTNode {

		public float delay = 2f;
		protected bool running = false;
		// Update is called once per frame
		protected void OnEnable () {
			StartCoroutine("SleepTime");
		}
		protected void OnDisable () {
			StopCoroutine("SleepTime");
			if (running) Failure();
		}
		
		protected IEnumerator SleepTime() {
			running = true;
			yield return new WaitForSeconds(delay);
			running = false;
			
			Succeed(); // probably disables this
		}
	}
}