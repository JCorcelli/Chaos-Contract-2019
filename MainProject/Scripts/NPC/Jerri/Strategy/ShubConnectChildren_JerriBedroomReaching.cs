using UnityEngine;
using NPC.Strategy;

namespace NPC.Jerri.Bedroom
{
	public class ShubConnectChildren_JerriBedroomReaching : ShubConnect {

		public Jerri.Bedroom.Reaching strategy = Jerri.Bedroom.Reaching.ReachKneeling;
		
		
		protected override void OnEnable() {
			base.OnEnable();
			if (InList(strategy.ToString()))
				OnStart();
			else
			DisableChildren();
				
		}
		protected override void OnDisable() {
			base.OnDisable();
			DisableChildren();
		}
		protected override void OnStart(){
			if (shub.recentIn != strategy.ToString()) return;
			EnableChildren();
			
		}
		protected override void OnStop(){
			
			if (shub.recentOut != strategy.ToString()) return;
			DisableChildren();
			
		}
		
		
		
	}
}