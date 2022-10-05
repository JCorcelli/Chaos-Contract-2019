using UnityEngine;
using System.Collections;

namespace Utility.AnimationEffects
{
	public class ATWrapToggle : AnimTriggerNode {

		public WrapMode wrapMode = WrapMode.Default;
		
		protected override void onTriggerEnter () {
			anim.wrapMode = wrapMode;
		}
		
		
		protected override void onTriggerExit () {
			anim.wrapMode = WrapMode.Default;
		
		}
	}
}