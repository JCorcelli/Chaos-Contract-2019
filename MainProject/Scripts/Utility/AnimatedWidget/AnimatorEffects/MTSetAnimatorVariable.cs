using UnityEngine;
using System.Collections;

namespace Utility.AnimatorEffects
{
	public class MTSetAnimatorVariable : MecanimTriggerNode {

	
		public string varName = "playing";
		public bool valueOnEnter = true;
		public bool valueOnExit = false;
		
		protected override void onTriggerEnter () {
			anim.SetBool(varName, valueOnEnter);
		}
		
		
		protected override void onTriggerExit () {
		
			anim.SetBool(varName, valueOnExit);
		}
	}
}