using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace FloorDesigner
{
	
	public class FloorPlanDrag : ConnectHubConnect {
		// This is intended for big applications that need to coordinate actions
		
		
		protected Vector3 lastPosition = new Vector3(-1,-1,-1);
		
		
		new protected Transform transform;
		
		protected DragHold2DPump target;
		
		protected override void OnEnable( ){
			base.OnEnable();
			
			transform = GetComponent<Transform>();
			Connect();
			
			lastPosition = transform.localPosition;
			
			target = GetComponentInParent<DragHold2DPump>();
			
		}
		
		
		protected void Send() {
			hub.Send((int)FloorPlanHub.Channel.Default, (int)FloorPlanHub.Enum.DragHold);
		}
			
		protected bool doUpdate = false;
		protected void Press() {
			target.SetVariables();
			target.UpdateBounds();
			
		}
		
		protected override void OnLateUpdate() {
			base.OnLateUpdate();
			
			if (doUpdate)
			{
				Press();
				doUpdate = false;
				
			}
			
			if (lastPosition != transform.localPosition)
			{
				lastPosition = transform.localPosition;
				Send();
			}
		}
		
		protected override void OnMessage(int senderEnum, int msgEnum) {
			
			if (senderEnum != (int)FloorPlanHub.Channel.Default) return;
			// every time something calls a message, in scope, this will check it
			
			
			
			if (msgEnum == (int)FloorPlanHub.Enum.ZoomRect)
			{
				
				Send();
					
			} 
			else if (msgEnum == (int)FloorPlanHub.Enum.ResizeHub)
			{
				
				//doUpdate = true;
			}
		}
		protected override void OnConnect(Object other) {
			
		}
		

	}
}