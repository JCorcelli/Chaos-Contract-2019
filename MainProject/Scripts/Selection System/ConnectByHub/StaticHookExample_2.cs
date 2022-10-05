using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;
using Datesim;

namespace SelectionSystem
{
	public class StaticHookExample_2 : SelectAbstract {
		// This is intended for big applications that need to coordinate actions
		
		
		public Image selectedImage;
		public Color pressedColor = Color.white;
		public Color defaultColor = Color.white;
		public DatesimHub hub;
		public DatesimHub.Channel channel = DatesimHub.Channel.Option;
		
		public int message = 0;
		
		public DatesimVariables vars;
		
		public bool connected = false;
		public bool subscribed = false;
		public virtual void CheckConnected(){
			if (!subscribed) SubscribeHub();
			
			if (connected && vars!= null) return;
			// hook
			Ping();
			
			//Connect();
			
		}
		protected void Ping(){
			StaticHub.Ping();
		}
		
		public virtual void UnsubscribeHub(){
			subscribed = false;
			
			StaticHub.onConnect -= OnConnect;
			
		}
		
		public virtual void SubscribeHub(){
			subscribed = true;
			
			UnsubscribeHub();
			StaticHub.onConnect += OnConnect;
			
		}
		
		
		protected virtual void OnConnect(object ob) {
			// for hooks
			
			if (vars == null && ob.GetType() == typeof(DatesimVariablesListener) )
			{
				DatesimVariablesListener target = ((DatesimVariablesListener)ob);
				
				connected = true;
				vars = target.GetComponent<DatesimVariables>();
				
				vars.onChange -= OnChange;
				vars.onChange += OnChange;
				
				
			}
		}
		
		
		protected override void OnEnable( ){
			base.OnEnable();
			
			CheckConnected();
			
			
			
		}
		
		protected virtual void OnChange() {}
		protected virtual void OnMessage(int senderEnum, int msgEnum) {
			
		}
		
		
		protected virtual void OnDestroy( ){
			if (hub == null) return;
			
			hub.onMessage -= OnMessage;
			vars.onChange -= OnChange;
		}
		
		
		
		protected override void OnPress() {
			
			
			
		}
		

	}
}