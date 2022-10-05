using UnityEngine;
using System.Collections;
using SelectionSystem;

namespace Datesim.Scene
{
	public class DatesimSceneHub : ConnectHub {

		public ConnectHubDelegate onChange;
		
		public int scene = 1;
		public RectTransform rectTransform;
		//start menu
		// end Date// date running
		// starting Date// setup date
		public DatesimSceneHubRadioTab mainScene;
		
		protected virtual void OnEnable(){
			if (rectTransform == null) 
			{
				rectTransform= GetComponent<RectTransform>();
				foreach (RectTransform t in rectTransform as RectTransform)
				{
					t.gameObject.SetActive(true);
					t.position = rectTransform.position;
				}
			}
			OnChange();
		}
		
		public bool calling = false;
		public void OnChange(){
			if (calling) return;
			calling = true;
			if (onChange != null) onChange();
			CheckScenes();
			calling  = false;
		}
		public void CheckScenes(){
			
			Canvas c;
			foreach (Transform t in rectTransform)
			{
				c = t.gameObject.GetComponent<Canvas>();
				
				if (c!= null && c.enabled) return;
			}
			
			mainScene.EnableCanvas();
		}
		
	}
}