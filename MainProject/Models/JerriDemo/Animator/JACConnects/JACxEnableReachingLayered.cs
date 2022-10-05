using UnityEngine;
using System.Collections;
using NPCSystem;

namespace Anim.Jerri
{
	public class JACxEnableReachingLayered : JACxPlay {
		
		
		protected override void OnEnable(){
			base.OnEnable();
			ih.Reach();
				
			
			
			
		}
		
		protected override void OnDisable(){
			base.OnDisable();
			ih.NoReach();
				
			
			
			
		}
	}
}