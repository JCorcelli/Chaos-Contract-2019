using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace FloorDesigner
{
	public class FloorPlanDragHold : ConnectHubConnect {
		// This is intended for big applications that need to coordinate actions
		
		
		
		protected bool doUpdate = false;
		protected DragHold2D target;
		
		protected override void OnEnable( ){
			base.OnEnable();
			
			Connect();
			
			target = GetComponentInParent<DragHold2D>();
			
			
			
		}
		
		
		protected void Send() {
			
			
			hub.Send((int)FloorPlanHub.Channel.Default, (int)FloorPlanHub.Enum.DragHold);
		}
			
		protected void Press() {
			target.SetVariables();
			target.UpdateBounds();
			
		}
		protected override void OnLateUpdate() {
			base.OnLateUpdate();
			if (target.pressed) Send();
			
			if (doUpdate) 
			{
				Press();
				doUpdate = false;
			}
		}
		
		protected override void OnMessage(int senderEnum, int msgEnum) {
			
			if (senderEnum != (int)FloorPlanHub.Channel.Default) return;
			// every time something calls a message, in scope, this will check it
			
			if (msgEnum == (int)FloorPlanHub.Enum.ZoomRect)
			{
				
				
				Press();
					
			} 
			else if (msgEnum == (int)FloorPlanHub.Enum.ResizeHub)
			{
				
				
				doUpdate = true;
				
			}
		}
		protected override void OnConnect(Object other) {
			
		}
		

	}
}