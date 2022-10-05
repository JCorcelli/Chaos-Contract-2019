using UnityEngine;

using UnityEngine.UI;
using System.Collections;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimEffectAnim : MonoBehaviour {
		
		public DatesimHub.Channel channel = DatesimHub.Channel.Effect;
		public DatesimHub.EffectEnum message = (DatesimHub.EffectEnum)1;
		
		public Animator anim;
		public string animatorState = "";
		
		protected virtual void OnEnable(){
			if (anim == null) anim = GetComponentInParent<Animator>();
		}
		
		
		protected virtual void Call() {
			// check if it's playing
			// queue a loop or quit
			// basic
			
			anim.Play(animatorState, 0, 0f);
		}
		
	}
}