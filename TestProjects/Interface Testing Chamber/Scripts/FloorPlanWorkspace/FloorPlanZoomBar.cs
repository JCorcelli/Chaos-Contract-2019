using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace FloorDesigner
{
	public class FloorPlanZoomBar : ConnectHubConnect {
		// This is intended for big applications that need to coordinate actions
		
		
		
		
		protected FloorPlanZoomRect target;

		protected override void OnEnable( ){
			base.OnEnable();
			
			Connect();
		}
		
		
		
		
		
		protected override void OnMessage(int senderEnum, int msgEnum) {
			
			// every time something calls a message, in scope, this will check it
			
			if (senderEnum == (int)FloorPlanHub.Enum.ZoomRect)
			{} // issue? I can just be connected, and not take messages
		}
		protected override void OnConnect(Object other) {
			// FloorPlanHub.Enum.ZoomBar, probably don't need it other than to sync
			GameObject ob = (GameObject)other;
			
			target = ob.GetComponent<FloorPlanZoomRect>();
			
				
			if (target != null)
				connected = ob;
			else
				Debug.Log("Something wrong with zoomRect", gameObject);
				
		}
		
		public void OnChange(Slider g){
			
			
			target.SetZoom(g.value);
		}
		

	}
}