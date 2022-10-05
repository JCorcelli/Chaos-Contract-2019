using UnityEngine;
using System.Collections;


namespace Utility
{
	public class TimeReverser : MonoBehaviour {
		// doesn't work because scale needs to be between 0 and 100.
		
		
		public float scale = 1f;
		
		protected float prevTime = 0f;
		
		void Update () {
		
			//Debug.Log(Time.deltaTime);
			//Debug.Log("Time.time" + (Time.time - prevTime).ToString());
			prevTime = Time.time;
			
			Time.timeScale = scale;
			
		}
	}
}
