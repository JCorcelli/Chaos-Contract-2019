using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace FloorDesigner
{
	public class FloorPlanWorkspaceHubToggleButton : SelectAbstract, IBool, IMessage {
		// This is intended for big applications that need to coordinate actions
		
		public Image selectedImage;
		public Color pressedColor = Color.gray;
		public Color defaultColor = Color.white;
		public FloorPlanWorkspaceHub hub;
		
		FloorPlanWorkspaceHub.Channel channel = FloorPlanWorkspaceHub.Channel.Default;
		public FloorPlanWorkspaceHub.Enum message = (FloorPlanWorkspaceHub.Enum)0;
		
		public ConnectHubDelegate onConnect;
		

		public bool bool_value = false;
		
		public bool GetBool (){ return bool_value; }
		
		public int GetMessage (){ return (int)message; }
		
		
		protected override void OnEnable( ){
			
			
			base.OnEnable();
			
			if (hub == null) hub = GetComponentInParent<FloorPlanWorkspaceHub>();
			if (hub == null) {
				Debug.Log("no hub, this broke", gameObject);
				return; 
			}
			
			hub.onMessage -= OnMessage;
			hub.onConnect -= OnConnect;
			hub.onMessage += OnMessage;
			hub.onConnect += OnConnect;
			
			
			Connect();
			
			BoolChange();
		}
		
		
		protected void BoolChange() {
			
			if (selectedImage != null)
			{
				if (bool_value) 
					selectedImage.color = pressedColor;
				else  
					selectedImage.color = defaultColor;
			}
		}
		
		protected override void OnDisable( ){
			base.OnDisable();
			if (hub == null) return;
			hub.onMessage -= OnMessage;
			
			// something can check if this is disabled.
		}
		
		
		protected virtual void Connect() {
			
			
			hub.Connect(gameObject);
			
			
			// everything looking for this, in scope, will now have it
			
		}
		
		protected virtual void OnMessage(int senderEnum, int msgEnum) {
			
			if (msgEnum == 0) Connect();
			// every time something calls a message, in scope, this will check it
			
				
		}
		protected virtual void OnConnect(Object other) {
			// there's no telling which will connect first. Only one needs to use the connection, whether it's here or OnEnable.
			
			// if (thisenum == (int)Enumerator) 
			// connected = other;
			// hub.connected = gameObject;
			
			// use the other gameObject
			// if onconnect != null onConnect() something else grabs the connection, done
		}
		
		protected override void OnPress() {
			base.OnPress();
			Connect();
			bool_value = !bool_value;
			
				
			hub.Send((int)channel, (int)message);
			BoolChange();
		}

	}
}