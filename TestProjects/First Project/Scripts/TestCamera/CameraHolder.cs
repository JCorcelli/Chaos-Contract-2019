using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TestProject.Cameras
{
	public class CameraHolder : MonoBehaviour 
	{

		// This holds the camera.
		public static CameraHolder instance;
		private SmoothTransform sf;
		private float defaultSmoothTime = 1f;
		[SerializeField] private Transform defaultTarget;
		private List<Transform> recentCalls = new List<Transform>();
		
		void Awake () {
			if (instance == null) instance = this;
			else
			{				
				GameObject.Destroy(this);
				return;
			}
			sf = gameObject.GetComponent<SmoothTransform>();
			
			defaultSmoothTime = sf.smoothTime;
			if ( sf.target == null ) SetTarget(defaultTarget);
		}
		
		
		// Update is called once per frame
		public void SetTarget(Transform other)
		{
			// forces camera
			sf.target = other;
			sf.smoothTime = defaultSmoothTime;
			
		}
		public void SnapTo(Transform other)
		{
			// forces camera
			if (sf.target != defaultTarget) return;
			sf.target = other;
			sf.SetVelocity();
			sf.smoothTime = 0f;
			transform.position = other.position;
			transform.rotation = other.rotation;
		}
		
		
		public void CallTo(Transform other)
		{
			// asks camera
			// add new transform
			if (!recentCalls.Contains(other)) recentCalls.Add(other);
			if (sf.target != defaultTarget) return;
			sf.target = other;
		}
		public void Release(Transform other)
		{
			if (recentCalls.Contains(other)) recentCalls.Remove(other);
			if (recentCalls.Count > 0) {
				if (sf.target == other) sf.target = recentCalls[recentCalls.Count - 1]; // most recent
			}
			else {
				// if all other transforms released
				sf.target = defaultTarget;
			}
			sf.smoothTime = defaultSmoothTime;
		}
		
	}
}