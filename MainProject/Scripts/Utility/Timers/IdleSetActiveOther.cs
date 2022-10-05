using UnityEngine;
using System.Collections;
using SelectionSystem;

namespace Utility.Timers
{
	public class IdleSetActiveOther : AbstractIdleHandler {

		public GameObject other;
		public float delay = 1f;
		public bool whilePlayerIdle = true;
		
		protected override IEnumerator _Idle(){
			if (_idle) yield break;
			_idle = true;
			yield return new WaitForSeconds(delay);
			Idle();
				
		}
		protected override void Idle(){
			other.SetActive(whilePlayerIdle);
		}
		
	}
}