using UnityEngine;
using NPC.Strategy;

namespace NPC.Jerri.Bedroom
{
	public class ShubConnectChildren_JerriBedroom : ShubConnect {

		public Jerri.Bedroom.Strategy strategy = Jerri.Bedroom.Strategy.Wait;
		
		
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