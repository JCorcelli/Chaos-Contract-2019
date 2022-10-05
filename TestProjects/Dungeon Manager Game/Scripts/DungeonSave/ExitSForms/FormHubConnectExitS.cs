using UnityEngine;

using UnityEngine.UI;
using System.Collections;


namespace Dungeon.Save
{
	public class FormHubConnectExitS : FormHubConnect {
		
		// needs "hub" passed to it. OnEnable / OnDisable called. as well as any additional variables added for future version.
		
		public DungeonUIFormExitSButton.ExitSEnum thisEnum;
		
		public bool isConnected = false;
		
		protected override void OnEnable( ){
			base.OnEnable();
			
			
			isConnected = Connect((int)thisEnum);
			
			
		}
		
		
		
		protected override void OnConnect(int compare, GameObject other) {
			if (compare != (int)thisEnum) return;
			
			connected = other;
			isConnected = true;
			
			hub.connected = gameObject;
			
			// use the other gameObject
			if (onConnect != null) onConnect();
		}
		
		

	}
}