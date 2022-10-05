using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SelectionSystem;

namespace DialogueSystem
{
	public class UpdateDialogueHook : UpdateBehaviour {
		
		public StaticDialogueHub hub;
		
		public bool connected = false;
		public bool subscribed = false;
		public virtual void CheckConnected(){
			if (!subscribed) SubscribeHub();
			
			if (connected && hub != null) return;
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
			
			if (hub == null && ob.GetType() == typeof(StaticDialogueHub) )
			{
				hub = ((StaticDialogueHub)ob);
				
				connected = true;
				
				hub.onChange -= OnChange;
				hub.onChange += OnChange;
				
				
			}
		}
		
		
		
		
		protected override void OnEnable() {
			base.OnEnable();
			CheckConnected();
		}
		protected virtual void OnChange() {}
		protected virtual void OnMessage(int senderEnum, int msgEnum) {
			
		}
		
		
		protected virtual void OnDestroy( ){
			if (hub == null) return;
			
			hub.onChange -= OnChange;
		}
		
	}
}