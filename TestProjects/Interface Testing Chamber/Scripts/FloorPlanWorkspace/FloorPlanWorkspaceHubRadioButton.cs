using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;

using SelectionSystem;

namespace FloorDesigner
{
	public class FloorPlanWorkspaceHubRadioButton : RadioButton, IBool, IMessage {
		// This is intended for big applications that need to coordinate actions
		
		// radio
		
		
		public Image selectedImage;
		public Color pressedColor = Color.gray;
		public Color defaultColor = Color.white;
		public FloorPlanWorkspaceHub hub;
		
		public FloorPlanWorkspaceHub.Channel channel = FloorPlanWorkspaceHub.Channel.Default;
		
		public FloorPlanWorkspaceHub.Enum message = (FloorPlanWorkspaceHub.Enum)0;
		

		
		public bool GetBool (){ return bool_value; }
		
		public int GetMessage (){ return (int)message; }
		
		
		protected override void OnEnable( ){
			base.OnEnable();
			if (hub == null) hub = GetComponentInParent<FloorPlanWorkspaceHub>();
			
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
		
		protected virtual void Connect() {
			
			hub.Connect(gameObject);
			
			
			// everything looking for this, in scope, will now have it
			
		}
		
		protected override void OnDisable( ){
			base.OnDisable();
			if (hub == null) return;
			hub.onMessage -= OnMessage;
			
			// something can check if this is disabled.
		}
		
		
		
		
		
		
		
		protected virtual void OnMessage(int senderEnum, int msgEnum) {
			if (senderEnum != (int)channel) return;
			if (msgEnum == 0) Connect();
		
			// main function
		}
		
		
		protected virtual void OnConnect(Object other) {
			// probably nothing here by default
		}
		
		protected override void RadioDeselect() {
			
			if (!bool_value ) return;
			base.RadioDeselect();
			
			
			Connect();
			hub.Send((int)channel, (int)message);
			BoolChange();
			
		}
		
		protected override void OnPress() {
			
			base.OnPress();
			
			
			Connect();
			hub.Send((int)channel, (int)message);
			BoolChange();
		}

	}
}