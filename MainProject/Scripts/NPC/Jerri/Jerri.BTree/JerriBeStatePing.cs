using UnityEngine;
using System.Collections;

namespace NPC.BTree.Jerri
{
	
	public class JerriBeStatePing	: BTPinger {

		public ActiveStatesEnum activeStates = (ActiveStatesEnum)1;
		
		protected JerriBeStateHUB stateHUB;
		
		protected override void OnEnable(){
			base.OnEnable();
			stateHUB = GetComponentInParent<JerriBeStateHUB>();
			if (stateHUB == null) Debug.Log("no state hub", gameObject);
		}
		protected override void OnUpdate() {
			current = stateHUB.Has(activeStates);
		}
	}

}