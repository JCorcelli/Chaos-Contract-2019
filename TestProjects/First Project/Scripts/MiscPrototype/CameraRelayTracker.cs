using UnityEngine;
using System.Collections;

namespace TestProject 
{
	public class CameraRelayTracker : MultibodyTracker {

		// Use this for initialization
		public GameObject cameraSystem;
		private Transform frameHit;
		
		void Start () {
			// do on hit is the inherited event
			doOnHit.AddListener (OnHitMethod);
			frameHit = new GameObject().transform;
		}
		
		private void OnHitMethod(){
			cameraSystem.SendMessage("RSetTarget", target.transform);
			cameraSystem.SendMessage("RSetFrame", frameHit);
			frameHit.position = contactPoint; // vector3
		}
		
	}
}