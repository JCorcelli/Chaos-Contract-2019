using UnityEngine;
using System.Collections;
using  NPC.BTree.Jerri;
using Animations;

namespace Anim.Jerri
{
	public class JerriUseDoor : UseDoorMonitored {

		// This assumes that the user is walking through and closing the door
		
		protected JerriAnimController ih;
		
		protected JerriBeStateHUB stateHUB;
		
		protected override void OnEnable() {
			base.OnEnable();
			Connect(); // copy from JACxConnect
			stateHUB = GetComponentInParent<JerriBeStateHUB>();
			if (stateHUB == null) Debug.LogError(name + " has no stateHUB", gameObject);
			
		}
		protected override void OnDisable() {
			base.OnDisable();
			
			controlling = false;
			
			
		}
		
		protected void Connect() {
				
			ih = gameObject.GetComponentInParent<JerriAnimController>();
			if (ih == null) Debug.LogError(name + " has no JerriAnimController", gameObject);
			
		}
		
		protected override void OnUpdate(){
			if (anim.GetFloat("ReachDist") < 1.5f)
					controlling = true;
				
			if (controlling) // && opening, not closing
			{
				// opening
				if (!isOpen && anim.GetCurrentAnimatorStateInfo(1).IsName("Stand_Door_TouchKnob_Open"))
				{
					
					Open();
				}
				else if (isOpen && anim.GetFloat("ReachDist") > 1.65f)
				{ // add calculation to check if I changed sides of door
					Close();
					ih.NoReach();
					stateHUB.Remove(ActiveStatesEnum.WantDoor); // copy from JerriBeStateDecor
					controlling = false;
				}
				
			}
			// abstract. not controlling
			// eg. bunny too close! lost control
		}
		
		
	}
}