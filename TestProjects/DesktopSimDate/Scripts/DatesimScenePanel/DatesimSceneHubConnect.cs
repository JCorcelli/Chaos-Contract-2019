using UnityEngine;
using System.Collections;


namespace Datesim.Scene
{
	public class DatesimSceneHubConnect : MonoBehaviour{

		public DatesimSceneHub hub;
		//public int scene = 1;
		protected void OnEnable(){
			if (hub == null)
			hub = GetComponentInParent<DatesimSceneHub>();
			if (hub == null)
				Debug.Log("no hub", gameObject);
			else
			{
				hub.onChange -= OnChange;
				hub.onChange += OnChange;
			}
		}
		public virtual void OnChange(){
			//if (hub.scene == scene)
				
		}
	}
}