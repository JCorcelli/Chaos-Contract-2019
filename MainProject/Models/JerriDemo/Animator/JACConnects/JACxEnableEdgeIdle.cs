using UnityEngine;
using System.Collections;
using NPCSystem;

namespace Anim.Jerri
{
	public class JACxEnableEdgeIdle : JACxPlay {

		protected Animator anim;
		
		protected void Awake() {
			anim = 	GetComponentInParent<Animator>();
		}
		
		
		
		protected override void OnEnable(){
			base.OnEnable();
			ih.InBed();
			ih.CrossFadeInFixedTime("Bed_Edge_Rest_Idle", .5f, 0,0);
			
			/*
			ih.CrossFadeInFixedTime("Bed_Edge_Rest_Lure", .5f, 0,0);
			ih.CrossFadeInFixedTime("Bed_Edge_Rest_Lure2", .5f, 0,0);
			ih.CrossFadeInFixedTime("Bed_Edge_Rest_Lure3", .5f, 0,0);
			ih.CrossFadeInFixedTime("Bed_Edge_Rest_Idle", .5f, 0,0);
			
			*/
			
			
		}
		
	}
}