using UnityEngine;
using System.Collections;

namespace TestProject.Cameras
{
	public class CamNonAssist
	{
		// know CameraHolder.instance
		public float viewTime   = 1f;
		public float loopTime   = 1f;
		public float viewDelay  = 1f;
		public float motionTime = 1f;
		public float loopMotion = 1f;
		public float motionDelay= 1f;
		
		
		

	// actions which briefly disrupt movement or view
	protected virtual void DrawGaze (  float speed, int percent, float time) {
		// negative percent should push
	}
	protected virtual void DrawMotion (float speed, int percent, float time) {
		// negative percent should push
	}
	
	// determine if target is viewed
	protected virtual void FindTarget () {}
	protected virtual void FeelTarget () {}
	protected virtual void ReachTarget() {}
	protected virtual void GoTo (Vector3 location, float time, bool ignoreEverything = false) {}
	protected virtual void Warp () {
		// warp immediately
	} 
	

		
		
	}
}