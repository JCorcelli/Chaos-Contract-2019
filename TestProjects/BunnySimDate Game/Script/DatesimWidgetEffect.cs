using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimWidgetEffect : DatesimAppConnect {
		// This is intended for big applications that need to coordinate actions
		
		
		
		public DatesimHub.EffectEnum[] effects = new DatesimHub.EffectEnum[]{};
		
		
		
		
		protected override void OnEnable( ){
			base.OnEnable();
			
			
		}
		
		
		protected override void OnChange() {
			
			if (vars.stage == 0 ) return;
				
			if (vars.effect == message)
			{
				PlayEffects();
			}
			// every time something calls a message, in scope, this will check it
			
				
		}
		
		protected virtual void PlayEffects(){
			if (effects.Length > 0) 
			{
				foreach (DatesimHub.EffectEnum effect in effects)
				{
					vars.effect =  (int)effect;
					vars.OnChange();
				}
			}
			
		}
		
	}
}