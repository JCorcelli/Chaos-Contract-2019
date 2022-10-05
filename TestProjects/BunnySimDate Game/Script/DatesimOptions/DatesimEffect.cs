using UnityEngine;

using UnityEngine.UI;
using System.Collections;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimEffect : MonoBehaviour {
		
		public DatesimHub.Channel channel = DatesimHub.Channel.Effect;
		public DatesimHub.EffectEnum message = (DatesimHub.EffectEnum)1;
		
		protected virtual void OnEnable(){
			
		}
		
		
		protected virtual void Call() {
			
		}
		
	}
}