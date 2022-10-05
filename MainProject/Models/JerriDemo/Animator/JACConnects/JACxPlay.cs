using UnityEngine;
using System.Collections;

namespace Anim.Jerri
{
	public abstract class JACxPlay : JACxConnect {

		
		
		protected override void OnEnable(){
			base.OnEnable();
			if (ih == null) return;
		}
		
		
		public void Play(string stateName, int layer = -1, float normalizedTime = float.NegativeInfinity){
			// used to enter or escape a very simple animation state
			ih.Play( name, layer, normalizedTime);
			
			
		}
		
		public void CrossFadeInFixedTime(string stateName, float transitionDuration, int layer = -1, float fixedTime = 0.0f){
			ih.CrossFadeInFixedTime(stateName, transitionDuration, layer, fixedTime);

		}
		
		public void CrossFade(string stateName, float transitionDuration, int layer = -1, float normalizedTime = float.NegativeInfinity){
			ih.CrossFade(stateName, transitionDuration, layer, normalizedTime);
			
		}
		
		
	}
}