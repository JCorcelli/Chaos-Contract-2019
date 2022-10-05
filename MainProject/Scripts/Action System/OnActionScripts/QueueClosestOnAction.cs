using UnityEngine;

using System.Collections;

namespace ActionSystem.OnActionScripts {

	public class QueueClosestOnAction : MonoBehaviour, IOnAction {
		
		public string whatAction = "Sleeping";
		
		
		public string _managerName;
		public string _managerTag;
		protected IndicatorTargetManager r;
		
		void Start()
		{
			r = gameObject.FindNameXTag(_managerName, _managerTag).GetComponent<IndicatorTargetManager>();
			
			
		}
		public void OnAction(ActionEventDetail data) {
			if (data.what.ToLower() == whatAction.ToLower())
			{
				r.Query(gameObject);
				
				
			}
			//else
				// give control back?
			
			
		
		}
	}
}