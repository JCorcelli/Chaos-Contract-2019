using UnityEngine;
using System.Collections;
using NPCSystem;

namespace Anim.Jerri
{
	public class JACxEnableKneelingReaching : JACxPlay {

		protected Animator anim;
		
		protected void Awake() {
			anim = 	GetComponentInParent<Animator>();
		}
		
		
		protected override void OnEnable(){
			base.OnEnable();
			// kneeling would start with y0
			
			ih.Kneel();
			ih.CrossFadeInFixedTime("Kneeling_Reaching", .5f, 0,0);
			
			
		}

		protected override void OnDisable(){
			base.OnDisable();
			// if y > 0.5f
			//if (anim.GetFloat("ReachY") > 0.2f)
			//{
			//if (ih.crossfading && !(Time.time > timestart + control)) return;
				
				ih.Stand();
				ih.CrossFadeInFixedTime("Stand_Walk_Idle", 1f, 0,0);
				
			//}
			/*
			else
			{
				ih.CrossFadeInFixedTime("Pose_Kneel_Idle", 1f, 0,0);
			}
			*/
			
			
			
		}
	}
}