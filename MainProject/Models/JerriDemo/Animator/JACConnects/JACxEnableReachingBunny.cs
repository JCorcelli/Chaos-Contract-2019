using UnityEngine;
using System.Collections;
using NPCSystem;

namespace Anim.Jerri
{
	public class JACxEnableReachingBunny : JACxPlay {
		
		
		protected override void OnEnable(){
			base.OnEnable();
			ih.CrossFadeInFixedTime("Reaching", 2f, 1,0);
				
			// this may be unnecessary
			
			
		}
		
	}
}