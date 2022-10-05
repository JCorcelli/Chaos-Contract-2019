using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Utility.GUI;
using SelectionSystem;

namespace Datesim
{
	public class DatesimAppStaticConnect : SelectAbstract {
		// This is intended for big applications that need to coordinate actions
		
		
		public Image selectedImage;
		public Color pressedColor = Color.white;
		public Color defaultColor = Color.white;
		
		
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
			
			if (ob.GetType() == typeof(DatesimVariablesListener) )
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
		
		
		protected virtual void Destroy( ){
			if (vars == null) return;
			
			
			vars.onChange -= OnChange;
			vars.onConnect -= OnConnect;
		}
		
		
		
		protected override void OnPress() {
			
			
			
		}
		

	}
}