using UnityEngine;
using System.Collections;


namespace CameraSystem.Swing
{
	public class SwingArm	: MonoBehaviour {

	
	
		public string _managerName = "SwingArmMain";
		public string _managerTag = "CameraRig";
		public int priority = 0;
		protected CASwingMain r; // manager-type
		public bool delayForPlayerInput = true;
		
		void Awake () {
			r = gameObject.FindNameXTag(_managerName, _managerTag).GetComponent<CASwingMain>();
		}
		
		void OnEnable() {
				
				r.CallTo(this);
				
				
		}
		void OnDisable() {
			
				r.Release(this);
				
				
		}
	}
}