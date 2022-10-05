using UnityEngine;
using System.Collections;
using NPCSystem;

namespace Anim.Jerri
{
	public class JACxEnableBedReaching : JACxPlay {

		protected Animator anim;
		
		protected void Awake() {
			anim = 	GetComponentInParent<Animator>();
		}
		
		
		
		protected override void OnEnable(){
			base.OnEnable();
			ih.InBed();
			ih.CrossFadeInFixedTime("Bed_Edge_Reaching", .5f, 0,0);
			
			
		}
		
	}
}