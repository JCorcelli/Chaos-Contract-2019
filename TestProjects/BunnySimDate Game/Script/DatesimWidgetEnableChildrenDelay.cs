using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using Utility.Triggers ;

namespace Datesim
{
	public class DatesimWidgetEnableChildrenDelay : EnableChildrenDelay {
		// This is to simulate an interruption in the speech
		
		
		public int message = 0;
		
		public DatesimVariables vars;
		
		protected override void OnEnable(){
			base.OnEnable();
			
			
			if (vars == null) 
			{
				vars = GetComponentInParent<DatesimVariables>();
			}
			if (vars == null) {
				Debug.Log("no vars, this broke", gameObject);
				return; 
			}
			
				//vars.onChange += OnChange;
			
		}
		protected virtual void OnChange(){
			
			
			
			// interrupted?
			//if (senderEnum == (int)optionChannel) 
			//	StopAllCoroutines();
		
			
			if (vars.stage == (int)message ) return;
				StopAllCoroutines();
				
				
		}
		
	}
}