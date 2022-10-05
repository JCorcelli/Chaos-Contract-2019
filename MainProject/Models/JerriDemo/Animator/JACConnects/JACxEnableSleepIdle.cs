﻿using UnityEngine;
using System.Collections;
using NPCSystem;

namespace Anim.Jerri
{
	public class JACxEnableSleepIdle : JACxPlay {

		protected Animator anim;
		
		protected void Awake() {
			anim = 	GetComponentInParent<Animator>();
		}
		
		
		
		protected override void OnEnable(){
			base.OnEnable();
			ih.InBed();
			
			// Resting on side. Can be randomized here.
			ih.CrossFadeInFixedTime("Bed_LieSide_Left", .5f, 0,0);
			
			/*
			ih.CrossFadeInFixedTime("Bed_LieSide_Right", .5f, 0,0);
			
			ih.CrossFadeInFixedTime("Bed_FaceDown_idle", .5f, 0,0);
			ih.CrossFadeInFixedTime("Bed_FaceUp_Wide_idle", .5f, 0,0);
			
			// active
			ih.CrossFadeInFixedTime("Bed_Laying_Laptop", .5f, 0,0);ih.CrossFadeInFixedTime("Bed_Sitting_Laptop", .5f, 0,0);
			
			*/
			
			
		}
		
	}
}