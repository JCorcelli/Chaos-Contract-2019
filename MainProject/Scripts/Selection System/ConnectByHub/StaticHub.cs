using UnityEngine;
using System.Collections;
using System.Collections.Generic;



namespace SelectionSystem
{
	// this is for finding other game objects in scope
	public delegate void StaticHubDelegate(Object ob);
	
	public delegate void StaticPingDelegate();
	
	public class StaticHub {
		// for big applications (in one hierarchy) that'll coordinate actions 
	
		public static StaticHubDelegate onConnect;
		
		public static StaticPingDelegate onPing;
		
		
		public static void Connect(Object ob) {
			if (onConnect != null) onConnect(ob);
		}
		
		public static void Ping(){
			if (onPing != null) onPing();
			
		}
		
		
	}
}