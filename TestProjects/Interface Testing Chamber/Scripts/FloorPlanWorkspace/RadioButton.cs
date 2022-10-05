using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;


namespace SelectionSystem
{
	public class RadioButton : SelectAbstract {
		// This is intended for big applications that need to coordinate actions
		
		// radio
		public int radioButtonEnum = 0;
		public RadioButtonHub radioHub;
		
		// main hub
		
		public ConnectHubDelegate onConnect;

		
		public bool bool_value = false;
		protected override void OnEnable(){
			base.OnEnable();
			
			
			
			
			if (radioHub == null) radioHub = GetComponentInParent<RadioButtonHub>();
			if (radioHub == null) {
				Debug.Log("no hub, this broke", gameObject);
				return; 
			}
			
			radioHub.onMessage += RadioMessage;
			// radioHub.onConnect += RadioConnect;
			
			RadioConnect();
		}
		
		protected override void OnDisable( ){
			base.OnDisable();
			if (radioHub == null) return;
			radioHub.onMessage -= RadioMessage;
			
			// something can check if this is disabled.
		}
		
		
		protected virtual void RadioConnect() {
			
			radioHub.Connect(gameObject);
			
			
			
		}
		
		protected virtual void RadioMessage(int button, int msgEnum) {
			
			// anything on this channel overrides this, unless some new rule is added
			
			if (button == radioButtonEnum){
				RadioDeselect();
				
			}
		}
		
		
		
		
		
		
		protected virtual void RadioDeselect() {
			
			bool_value = false; // future things should do the same
			
			
		}
		protected override void OnPress() {
			bool_value = !bool_value;
			if (!bool_value) return;
			
			RadioConnect();
			
			radioHub.onMessage -= RadioMessage;
			radioHub.Send(radioButtonEnum, 0);
			
			if (enabled) // in case this turns me off somehow
				radioHub.onMessage += RadioMessage;
		}

	}
}