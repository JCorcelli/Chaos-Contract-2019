using UnityEngine;
using System.Collections;



namespace SelectionSystem
{
	public delegate void ConnectHubDelegate();
	
	
	// this is for all messages in scope. This can be a LOT of messages.
	public delegate void Msg_ConnectHubDelegate(int channel, int message);
	
	
	// this is for finding other game objects in scope
	public delegate void Re_ConnectHubDelegate(Object ob);
	
	public class ConnectHub : MonoBehaviour {
		// for big applications (in one hierarchy) that'll coordinate actions 
	
		public Msg_ConnectHubDelegate onMessage;
		public Re_ConnectHubDelegate onConnect;
		
		
		public enum Enum {
			Ping = 0
		}
		
		public virtual void Send(int channel, int message) {
			if (onMessage != null) onMessage(channel, message);
		}
		
		public void Connect(Object ob) {
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