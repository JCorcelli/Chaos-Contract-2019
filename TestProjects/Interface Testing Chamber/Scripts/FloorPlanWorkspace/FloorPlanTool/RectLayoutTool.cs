using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;


namespace FloorDesigner.Tools
{
	public class RectLayoutTool : FloorPlanWorkspaceHubConnect {
		// This is intended for big applications that need to coordinate actions
		
		
		protected FloorDesigner.IBool toolActive;
		
		public GameObject[] enabledObjects;
		public GameObject[] disabledObjects;
		
		
		protected const int message_tool =(int)FloorPlanWorkspaceHub.Enum.Rect; 
		
		
		protected override void OnDisable( ){
			AdjustSetting(false);
		}
		protected override void OnEnable( ){
			StartReconnect();
			base.OnEnable();
			
		}
		
		protected void AdjustSetting(bool b ){
			
			foreach (GameObject toolObject in enabledObjects)
				toolObject.SetActive(b);
				
			foreach (GameObject toolObject in disabledObjects)
				toolObject.SetActive(!b);
						
		}
		
		
		protected override void OnMessage(int channel, int msgEnum) {
			
			if (channel == (int)FloorPlanWorkspaceHub.Channel.Default)
			{
				
				if (msgEnum == message_tool)
				{
					
					AdjustSetting(toolActive.GetBool());
				}
					
					
			}
			
				
		}
		
		protected override void OnConnect(Object other) {
			
				
				
			GameObject ob = (GameObject)other;
			
			FloorDesigner.IMessage button = ob.GetComponent<IMessage>();
			
			if (button == null) return;
			
			
			if (button.GetMessage() == message_tool)
			{
				toolActive = ob.GetComponent<IBool>();
				AdjustSetting(toolActive.GetBool());
				
				connected = true;
			}
				
				
		}
		
		

	}
}