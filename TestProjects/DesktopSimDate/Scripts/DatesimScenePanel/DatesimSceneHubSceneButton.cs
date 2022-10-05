using SelectionSystem;
using UnityEngine;
using System.Collections;


namespace Datesim.Scene
{
	public class DatesimSceneHubSceneButton : SelectAbstract{

		public DatesimSceneHub hub;
		public int scene = 1;
		protected override void OnEnable(){
			base.OnEnable();
			if (hub == null)
			hub = GetComponentInParent<DatesimSceneHub>();
			if (hub == null)
				Debug.Log("no hub", gameObject);
			
		}
		
		protected override void OnPress(){
			hub.scene = scene;
			hub.OnChange();
		}
	}
}