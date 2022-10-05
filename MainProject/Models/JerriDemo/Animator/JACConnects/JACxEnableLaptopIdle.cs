using UnityEngine;
using System.Collections;
using NPCSystem;

namespace Anim.Jerri
{
	public class JACxEnableLaptopIdle : JACxPlay {

		protected Animator anim;
		
		protected void Awake() {
			anim = 	GetComponentInParent<Animator>();
		}
		
		
		
		protected override void OnEnable(){
			base.OnEnable();
			ih.InBed();
			
			// Resting on side. Can be randomized here.
			ih.CrossFadeInFixedTime("Bed_Laying_Laptop", .5f, 0,0);
			
			/*
			
			// active
			ih.CrossFadeInFixedTime("Bed_Sitting_Laptop", .5f, 0,0);
			
			*/
			
			
		}
		
	}
}