using UnityEngine;
using System.Collections;
using NPCSystem;

namespace Anim.Jerri
{
	public class JACxEnableTrip: JACxPlay {

		protected Animator anim;
		
		protected void Awake() {
			anim = 	GetComponentInParent<Animator>();
		}
		
		
		
		protected override void OnEnable(){
			base.OnEnable();
			ih.InBed();
			
			// Resting on side. Can be randomized here.
			ih.CrossFadeInFixedTime("Stand_Walk_Trip_Crawl", .05f, 0,0);
			
			/*
			ih.CrossFadeInFixedTime("Stand_Walk_Trip_Walk", .5f, 0,0);
			
			ih.CrossFadeInFixedTime("Stand_Walk_Trip_Fall", .5f, 0,0);
			
			ih.CrossFadeInFixedTime("Bed_FaceUp_Wide_idle", .5f, 0,0);
			
			// active
			ih.CrossFadeInFixedTime("Bed_Laying_Laptop", .5f, 0,0);ih.CrossFadeInFixedTime("Bed_Sitting_Laptop", .5f, 0,0);
			
			*/
			
			
		}
		
	}
}