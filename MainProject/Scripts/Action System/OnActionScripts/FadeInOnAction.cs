using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ActionSystem.OnActionScripts {
	public class FadeInOnAction : MonoBehaviour, IOnAction {

		protected Image image;
		protected Color col;
		public float secondsOut = 1f; //seconds
		public string whatAction = "FadeIn"; // e.g. moving
		
		
		void Awake () {
			
			image = GetComponent<Image >();
			col = image.color;
		}
		
		void OnEnable () {
			// just for a manual attempt
			image.enabled = true;
			col = image.color;
			col.a =1;
			image.color = col;
		}
		public void OnAction(ActionEventDetail data) {
			// delay?
			if (data.what.ToLower() == whatAction.ToLower())
			{
				image.CrossFadeAlpha(0, secondsOut, true);
				
			}
			
		}
	}
}