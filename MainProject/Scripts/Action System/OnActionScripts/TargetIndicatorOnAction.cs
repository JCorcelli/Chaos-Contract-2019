using UnityEngine;

using System.Collections;

namespace ActionSystem.OnActionScripts {

	public class TargetIndicatorOnAction : MonoBehaviour, IOnAction {
		
		public string whatAction = "Sleeping";
		
        public string _setTargetOnName = "DirectionIndicator";
        public string _setTargetOnTag = "Magnets";
        public GameObject setTargetOn;
		
		public string _targetName = "TargetIndicator";
		public string _targetTag = "Magnets";
		public Transform target;
		
		
		void Start()
		{
			target = gameObject.FindNameXTag(_targetName, _targetTag).transform;
			
			setTargetOn = gameObject.FindNameXTag(_setTargetOnName, _setTargetOnTag);
			
			
		}
		
		
		public void OnAction(ActionEventDetail data) {
			if (data.what.ToLower() == whatAction.ToLower())
			{
				target.transform.position = transform.position;
				setTargetOn.SendMessage("SetTarget", target);
				
				
			}
			//else
				// give control back?
			
			
		
		}
	}
}