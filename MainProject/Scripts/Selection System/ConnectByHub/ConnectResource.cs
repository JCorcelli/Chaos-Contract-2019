using UnityEngine;
using System.Collections;



namespace SelectionSystem
{
	
	
	
	// this is for finding other game objects in scope + seeking a resource hub
	public delegate void Re_ConnectResourceDelegate(object ob);
	
	public class ConnectResource : MonoBehaviour {
		// for big applications (in one hierarchy) that'll coordinate actions 
	
		public Msg_ConnectHubDelegate onMessage;
		public Re_ConnectResourceDelegate onConnect;
		
		
		public enum Enum {
			Ping = 0
		}
		
		public virtual void Send(int channel, int message) {
			if (onMessage != null) onMessage(channel, message);
		}
		
		public void Connect(object ob) {
			if (onConnect != null) onConnect(ob);
		}
		
		public void Ping(){
			if (onMessage != null)
			{
				// foreach channel...
					onMessage(0, (int)Enum.Ping);
		
			}
			
		}
		
		
	}
}