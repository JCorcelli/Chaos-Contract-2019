using UnityEngine;
using System.Collections;

namespace TestProject.Cameras
{
	public class CallCamera : MonoBehaviour 
	{

		// Use this for initialization
		public bool debugOn = true;
		public bool snap = false;
		public bool snapTo = false;
		void Start() {
			if (debugOn)
				Debug.Log(name + " calling camera.");
			if (snap) CameraHolder.instance.SnapTo(transform);
			else CameraHolder.instance.CallTo(transform);
		}
		
		
		void OnEnable () {
			if (snapTo){
				transform.rotation = CameraHolder.instance.transform.rotation;
				transform.position = CameraHolder.instance.transform.position;
			} 
			if (snap) CameraHolder.instance.SnapTo(transform);
			else CameraHolder.instance.CallTo(transform);
		}
		
		void OnDisable () {
			CameraHolder.instance.Release(transform);
		}
	}
}