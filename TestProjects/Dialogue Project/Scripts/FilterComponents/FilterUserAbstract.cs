
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using SelectionSystem;


namespace DialogueSystem
{
	
	public abstract class FilterUser : UpdateBehaviour {
		// Example of someone who'll use filters
		
		private FilterHook filterHook;
		
		protected override void OnEnable(){
			filterHook = new FilterHook();
			filterHook.onTryConnectFilter += AddFilter;
			filterHook.hub = GetComponentInParent<ConnectResource>();
			
			filterHook.Connect();
		}
		
		protected virtual void AddFilter(List<DFilter> filters) {
			// processor.filters.Add(f);
			
			// if filters[i].filtertype == "Processor"
		}
		
		
	}
}