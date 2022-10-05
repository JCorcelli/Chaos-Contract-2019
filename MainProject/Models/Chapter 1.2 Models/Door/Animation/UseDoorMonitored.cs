using UnityEngine;
using System.Collections;


namespace Animations
{
	public class UseDoorMonitored : UseDoor {

		// This assumes that the user is walking through and closing the door
		
		// action 1: walk over grab door (nobody else can use it), immediately open door
		// action 2: walk away let go of door (door closes)
		
		public bool controlling = false;
		protected Animator anim;
		
		protected override void OnEnable() {
			base.OnEnable();
			anim = GetComponentInParent<Animator>();
		}
		
		protected override void OnDisable() {
			base.OnDisable();
			
			if (controlling) // && closeOnExit
			{
				Close();
			}
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
				else if (isOpen && anim.GetFloat("ReachDist") > 1.75f)
				{ // add calculation to check if I changed sides of door
					Close();
					controlling = false;
				}
				
			}
			// abstract. not controlling
			// eg. bunny too close! lost control
		}
		
		
	}
}
