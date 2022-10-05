using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CameraSystem
{
	public class CameraHolderPiggy : UpdateBehaviour 
	{

		// This holds the camera.
		
		public bool locked = false;
		protected CameraHolder mc;
		public SmoothTransform smoothTransform;
		
		protected List<CameraAssistant> recentCalls = new List<CameraAssistant>();
		
		protected void Awake() {
			smoothTransform = GetComponent<SmoothTransform>();
			
		}
		protected void Start () {
			mc = CameraHolder.instance;
			
		}
		protected override void OnUpdate () {
			smoothTransform.target = mc.current.transform;
		}
		
	}
}