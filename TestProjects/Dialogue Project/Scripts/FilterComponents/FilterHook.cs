
using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using SelectionSystem;


namespace DialogueSystem
{
	public delegate void FilterHookDelegate(List<DFilter> filterArray);
	public class FilterHook : ConnectHubHook {
		// so both filter and component use this
		
		public List<DFilter> filters;
		public FilterHookDelegate onTryConnectFilter ;
		protected override void OnConnect(object ob) {
			// for hooks
			
			var ot = ob.GetType();
			
			if ( ot == typeof(FilterHook) )
			{
				
				((FilterHook)ob).TryConnectFilter(this);
			}
			
		}
		
		public virtual void TryConnectFilter(object ob){
			/* idea for component
				accepted filter typeof
				(type)ob
			*/
			//Type ot = ob.GetType();
			
			// if ( ot is FilterResource )
			if (filters != null && onTryConnectFilter != null) onTryConnectFilter(filters);
		}
		
		
		protected virtual void OnChange() {}
		
		
	}
}