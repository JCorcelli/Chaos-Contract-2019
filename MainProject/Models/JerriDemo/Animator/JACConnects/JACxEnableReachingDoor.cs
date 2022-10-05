using UnityEngine;
using System.Collections;
using NPCSystem;

namespace Anim.Jerri
{
	public class JACxEnableReachingDoor : JACxPlay {
		
		
		protected override void OnEnable(){
			base.OnEnable();
			
			ih.CrossFadeInFixedTime("Stand_Door_Reach", .5f, 1,0);
				
			
			
			
		}
		
	}
}