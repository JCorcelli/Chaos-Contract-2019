using UnityEngine;
using System.Collections;

namespace CameraSystem 
{
	
	public class CAOnEnable : CameraAssistant {
		
		protected void Start () { CallCamera (); }
		protected override void OnEnable () { base.OnEnable(); CallCamera (); }
		protected override void OnDisable () { base.OnDisable(); Release (); }
		
	}
}