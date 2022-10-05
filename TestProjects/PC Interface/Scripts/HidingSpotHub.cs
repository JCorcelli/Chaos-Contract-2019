
using UnityEngine;
using System.Collections;
using SelectionSystem;

namespace NPCSystem
{
	
	public class HidingSpotHub : ConnectHub
	{
		// the connect hub sends messages, this can store variables.
		public ConnectHubDelegate onChange;
		
		public string group = "";
		
		public void Awake(){
			
		}
		
		
		public virtual void OnChange(){
			if (onChange != null) onChange();
		}
		public void OnDestroy() {
			
		}
	}
}