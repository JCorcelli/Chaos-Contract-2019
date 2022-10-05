using UnityEngine;

using UnityEngine.UI;
using System.Collections;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	
	public class DatesimEffectAnimMaster : DatesimAppConnectLite {
		
		public DatesimHub.Channel channel = DatesimHub.Channel.Effect;
		public DatesimHub.EffectEnum message = (DatesimHub.EffectEnum)1;
		
		public Animator anim;
		public string clipName = "DatesimNoAnimation";
		public string state = "Default";
		public AnimationClip[] animations = new AnimationClip[]{};
		
		public AnimationClip defaultAnimation;
		
		public AnimatorOverrideController control;
		public bool hiding = false;
		public bool onStage = false;
		public AnimationClip currentAnim;
		public DatesimAudioMaster audioMaster;
		
		protected override void OnEnable(){
			base.OnEnable();
			if (anim == null) anim = GetComponentInParent<Animator>();
			if (audioMaster == null) audioMaster = GetComponentInParent<DatesimAudioMaster>();
			//if (control == null)
			//	control = anim.runtimeAnimatorController;
			//else
			anim.runtimeAnimatorController = control;
		}
		
		
		
		protected virtual void Call(DatesimHub.EffectEnum msg) {
			// hide, on/off reverses
			
			if (msg ==DatesimHub.EffectEnum.DatesimToggleHide)
			{
				if (hiding)
				{
					hiding = false;
					StringMatch("DatesimUnhide");
				}
				else
				{
					hiding = true;
					StringMatch("DatesimHide");
				}
				
				return;
			}
			
			if (hiding)
			{
				hiding = false;
			}
			
			StringMatch( msg.ToString());
		}
		
		protected void StringMatch(string msg){
			
			foreach (AnimationClip a in animations)
			{
				if (a.name == msg)
				{
					
					control[clipName] = a;
					return;
				}
				
			}
			//control[clipName] = defaultAnimation;
		}
		
		protected override void OnChange(){
			string clipName = "";
			
			// start or end
			if (!onStage && vars.date_on ) clipName = "DatesimAppear";
			else if (onStage && !vars.date_on ) clipName = "DatesimDisappear";
			else if (vars.effect < 0) return;
			
			Call((DatesimHub.EffectEnum)vars.effect);
			
			onStage = vars.date_on;
			StringMatch(clipName);
			
			vars.effect= -1;
		}

		protected void Forward() {
			
			if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0f)
				anim.Play(state, 0, 0f);
			anim.SetFloat("animSpeed", 1);
			anim.speed = 1;
			//anim.Play(state);
			//StartCoroutine("StartAndStopThreeTimes");
			
		}
		
		protected void Reverse() {
			
			if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f)
				anim.Play(state, 0, 1f);
			anim.SetFloat("animSpeed", -1);
			anim.speed = 1;
			//if anim.GetCurrentAnimatorStateInfo(-1).normalizedTime == 0;
			//anim.Play(state, -1, 1);
		}
			
		
		protected override void OnUpdate(){
			if (currentAnim != null)
			{
			control[clipName] = currentAnim;
			//anim.runtimeAnimatorController = control;
			//anim.Play(clipName, 0, 0f);
			currentAnim = null;
			}
		}
		
		public void PlayAudio(AudioClip clip)
		{
			audioMaster.PlayAudio(clip);
		}
		
	}
}