using UnityEngine;
using System.Collections;
using ActionSystem.OnActionScripts;
using ActionSystem;

namespace NPCSystem
{
	public class BunnyOnJerriSayBunny : BunnyDelayedPose {
		protected override void OnAction(ActionEventDetail data) {
			if (data.what == JerriActions.say_bunny)
			{
				SetPose(); // uses pose
				
			}
			
			
		
		}
		
	}
}