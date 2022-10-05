using UnityEngine;
using System.Collections;

namespace TestProject
{
	public class HotSwapCameraTarget : MonoBehaviour {
		private bool isLive = false;
		
		public GameObject defaultMain;
		public GameObject activeCam;
		
		// Use this for initialization
		void Start () {
			defaultMain = Camera.main.gameObject;
			if (activeCam != null)
			{
				isLive = true;
				
				defaultMain.SetActive(false);
			}
			else
				activeCam = defaultMain;
		}
		
		void OnDisable () {
			if (activeCam != null)
				activeCam.SetActive(false);
			if (defaultMain != null)
				defaultMain.SetActive(true);
		}
		
		
		void Update () {
			
			if (activeCam == null) isLive = false;
			if (isLive) return;
			activeCam.SetActive(false); // current room camera is wrong
			activeCam = defaultMain;	// defalt camera is right
			activeCam.SetActive(true);
		}
		// determine if available camera is preferable
		public void AutoSwap (Transform available ){
			if (!isLive) // default, or no camera active
			{
				isLive = true;
				activeCam.SetActive(false);
				activeCam = available.gameObject;
				activeCam.SetActive(true);
				
				
			}
		}
		public void Kill (Transform miss){
			if (miss.gameObject != activeCam) return;
				
			isLive = false;
		}
		
		
	}
}