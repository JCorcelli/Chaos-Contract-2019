using UnityEngine;
using System.Collections;

using SelectionSystem;

namespace FloorDesigner
{
	
	public class FloorPlanResizeHub : ConnectHubConnect {
		// corrects the visual of a drag action
		// responds to mouse position, only while held or pressed
		
		
		
		
		protected ResizeHub rh;
		
		protected override void OnEnable( ){
			base.OnEnable();
			
			rh = GetComponent<ResizeHub>();
			
			if (rh == null) Debug.Log("need resizeHub",gameObject);
			rh.onRelease += SendMsg;
			rh.doWhilePressed += SendMsg;
				
			//hub.onMessage += OnMessage;
			//hub.onConnect += OnConnect;
			
			
			//
			Connect();
		}
		
		protected override void OnDisable( ){
			base.OnDisable();
			
			rh.doWhilePressed -= SendMsg;
			rh.onRelease -= SendMsg;
			// something can check if this is disabled.
		}
		
		
		
		
		protected override void OnMessage(int senderEnum, int msgEnum) {
				
		}
		protected override void OnConnect(Object other) {
			
		}
		
		
		protected void SendMsg() 
		{
			
			hub.Send((int)FloorPlanHub.Channel.Default, (int)FloorPlanHub.Enum.ResizeHub);
			
		}
	}
}