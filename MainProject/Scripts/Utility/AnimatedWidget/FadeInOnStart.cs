using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Utility {
	public class FadeInOnStart : MonoBehaviour {

		protected Image image;
		protected Color col;
		public float secondsOut = 1f; //seconds
		
		
		void Awake () {
			
			image = GetComponent<Image >();
		}
		
		void OnEnable () {
			// just for a manual attempt
			col = image.color;
			col.a =1;
			image.color = col;
			image.CrossFadeAlpha(0, secondsOut, true);
		}
	}
}