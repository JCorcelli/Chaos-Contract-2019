using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;


namespace FloorDesigner.Tools
{
	public class DrawLineTool : FloorPlanWorkspaceHubConnect {
		// This is intended for big applications that need to coordinate actions
		
		
		protected FloorDesigner.IBool toolActive;
		
		public GameObject toolObject;
		
		
		protected const int message_tool =(int)FloorPlanWorkspaceHub.Enum.DrawLine; 
		
		
		protected override void OnEnable( ){
			StartReconnect();
			base.OnEnable();
			if (toolObject == null) Debug.Log("need to assign tool object", gameObject);
			else
				toolObject.SetActive(false);
		}
		
		
		
		
		protected override void OnMessage(int senderEnum, int msgEnum) {
			if (senderEnum != (int)channel) return;
			if (msgEnum == 0) Connect();
			
			if (msgEnum == message_tool)
			{
				toolObject.SetActive(toolActive.GetBool());
			}
					
			
				
		}
		
		protected override void OnConnect(Object other) {
			
			GameObject ob = (GameObject)other;
				
			FloorDesigner.IMessage button = ob.GetComponent<IMessage>();
			
			if (button == null) return;
			
			
			if (button.GetMessage() == message_tool)
			{
				toolActive = ob.GetComponent<IBool>();
				toolObject.SetActive(toolActive.GetBool());
				connected = true;	
			}
				
				
		}
		
		

	}
}