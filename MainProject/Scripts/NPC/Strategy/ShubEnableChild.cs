using UnityEngine;

namespace NPC.Strategy
{
	public class ShubEnableChild : ShubConnect {

		public string targetName = "";
		
		protected override void OnEnable() {
			base.OnEnable();
			if (InList(targetName))
				OnStart();
		}
		protected override void OnDisable() {
			base.OnDisable();
		}
		protected override void OnStart(){
			if (shub.recentIn != targetName) return;
			transform.GetChild(0).gameObject.SetActive(true);
			
		}
		protected override void OnStop(){
			
			if (shub.recentOut != targetName) return;
			transform.GetChild(0).gameObject.SetActive(false);
			
		}
		
		
		
	}
}