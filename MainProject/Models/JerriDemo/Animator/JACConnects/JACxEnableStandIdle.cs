using UnityEngine;
using System.Collections;
using NPCSystem;

namespace Anim.Jerri
{
	public class JACxEnableStandIdle: JACxPlay {

		protected Animator anim;
		
		protected void Awake() {
			anim = 	GetComponentInParent<Animator>();
		}
		
		
		
		protected override void OnEnable(){
			base.OnEnable();
			// I should be standing or something
			
			// Resting on side. Can be randomized here.
			ih.CrossFadeInFixedTime("Stand_Idle_Loose", .05f, 0,0);
			
			/*
			ih.CrossFadeInFixedTime("Stand_Idle", .5f, 0,0);
			
			ih.CrossFadeInFixedTime("Stand_Idle2", .5f, 0,0);
			
			ih.CrossFadeInFixedTime("Stand_Idle_Tense", .5f, 0,0);
			
			// active
			ih.CrossFadeInFixedTime("Stand_LookUp_Stretch", .5f, 
			
			
			*/
			
			
		}
		protected override void OnDisable(){
			base.OnEnable();
			ih.Stand();
			ih.CrossFadeInFixedTime("Stand", .05f, 0,0);
		}
		
	}
}