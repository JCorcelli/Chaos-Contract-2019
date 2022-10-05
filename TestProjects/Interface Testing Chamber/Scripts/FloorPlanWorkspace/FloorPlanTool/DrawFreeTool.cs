using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;


namespace FloorDesigner.Tools
{
	public class DrawFreeTool : FloorPlanWorkspaceHubConnect {
		// This is intended for big applications that need to coordinate actions
		
		
		protected FloorDesigner.IBool toolActive;
		
		public GameObject toolObject;
		
		
		protected const int message_tool =(int)FloorPlanWorkspaceHub.Enum.DrawFree; 
		
		
		
		protected override void OnEnable( ){
			StartReconnect();
			base.OnEnable();
			if (toolObject == null) Debug.Log("need to assign tool object", gameObject);
			else
				toolObject.SetActive(false);
			
			
		}
		
		
		
		
		protected override void OnMessage(int channel, int msgEnum) {
			
			if (channel == (int)FloorPlanWorkspaceHub.Channel.Default)
			{
				
				if (msgEnum == message_tool)
				{
					toolObject.SetActive(toolActive.GetBool());
				}
					
					
			}
			
				
		}

		
		
		protected override void OnConnect(Object other) {
				
			GameObject ob = (GameObject)other;
			
			FloorDesigner.IMessage button = (IMessage)other;
			
			
			if (button == null) return;
			
			
			if (button.GetMessage() == message_tool)
			{
				toolActive = ob.GetComponent<IBool>();
				toolObject.SetActive(toolActive.GetBool());
				
				connected = true;
			}
			
			//count = 0;
			//if (message_tool != null ) count ++
		}
		
		

	}
}