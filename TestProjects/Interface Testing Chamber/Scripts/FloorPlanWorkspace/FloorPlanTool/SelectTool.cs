using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;


namespace FloorDesigner.Tools
{
	public class SelectTool : FloorPlanWorkspaceHubConnect {
		// This is intended for big applications that need to coordinate actions
		
		protected FloorDesigner.IBool rooms;
		protected FloorDesigner.IBool toolActive;
		
		protected FloorDesigner.IBool itoms;
		public GameObject toolObject;
		
		public bool bRooms = false;
		public bool bItoms = false;
		
		protected const int message_itoms = (int)FloorPlanWorkspaceHub.Enum.SelectIncludeItoms;
		protected const int message_rooms = (int)FloorPlanWorkspaceHub.Enum.SelectIncludeRooms;
		protected const int message_tool =(int)FloorPlanWorkspaceHub.Enum.SelectTool; 
		
		
		protected override void OnEnable( ){
			base.OnEnable();
			StartReconnect();
			hub.Ping();
			if (toolObject == null) Debug.Log("need to assign tool object", gameObject);
			else
				toolObject.SetActive(false);
		}
		
		
		
		
		protected override void OnMessage(int senderEnum, int msgEnum) {
			
			if (senderEnum != (int)channel) return;
			if (msgEnum == 0) Connect();
		
			if (msgEnum == message_rooms)
			{
				
				if (rooms == null) return;
				bRooms = rooms.GetBool();
			}
			else if (msgEnum == message_itoms)
			{
				if (itoms == null) return;
				bItoms = itoms.GetBool();
			}
			else if (msgEnum == message_tool)
			{
				if (toolActive == null) 
				{
					Debug.Log("", gameObject);
					return;
				}
				toolObject.SetActive(toolActive.GetBool());
			}
				
				
			
				
		}
		
		protected override void OnConnect(Object other) {
				
			
			GameObject ob = (GameObject)other;
			
			FloorDesigner.IMessage button = ob.GetComponent<IMessage>();
			
			if (button == null) return;
			
			
			if (button.GetMessage() == message_rooms)
			{
				rooms = ob.GetComponent<IBool>();
				bRooms = rooms.GetBool();
			}
			else if (button.GetMessage() == message_itoms)
			{
				itoms = ob.GetComponent<IBool>();
				bItoms = itoms.GetBool();
			}
			else if (button.GetMessage() == message_tool)
			{
				
				toolActive = ob.GetComponent<IBool>();
				toolObject.SetActive(toolActive.GetBool());
			}
			
			if (rooms != null && itoms != null && toolActive != null) connected = true;
				
		}
		
		

	}
}