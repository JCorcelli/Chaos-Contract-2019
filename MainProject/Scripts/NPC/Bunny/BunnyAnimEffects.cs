using UnityEngine;
using System.Collections;
using ActionSystem;
using Utility.Managers;

namespace NPCSystem
{
	public class BunnyAnimEffects : UpdateBehaviour
	{
		
		protected Animator anim;
		
		protected EffectHUB effect;
		protected void Start()
		{
			effect = GetComponentInParent<EffectHUB>();
			anim = GetComponentInParent<Animator>();
		}
		
		protected bool loopReset = true;
		protected bool isWalking = false;
		protected bool isRunning = false;
		public void MovingSound() {
			// should not be a ghost
			
			// should touch ground
			// touch something noisy
			
			isWalking = anim.GetCurrentAnimatorStateInfo(0).IsName("Walking");
			isRunning = anim.GetCurrentAnimatorStateInfo(0).IsName("Running");
			bool isNoisy = isRunning;
			
			
			float nTime = Mathf.Repeat(anim.GetCurrentAnimatorStateInfo(0).normalizedTime, 1);
			if (nTime < 0.2f) loopReset = true;
			
			// sound effect signal
				
			if (loopReset && isNoisy)
			{
				
				if (nTime >= 0.2f)
				{
					loopReset = false;
					ActionEventDetail av = new ActionEventDetail();
					//av.who = "Bunny";
					av.what = "Moving";
					effect.Submit(av);
				}
				
			}
							
			
		}
		
		protected override void OnUpdate() {
			MovingSound();
		}
		
		
		
	}
}