using UnityEngine;
using System.Collections;

namespace TestProject.Cameras
{
	public class CADistractible
	{
		public float delayReponse = 1f;
		public float randomDelay  = 1f;
		public Transform [] distractors;
		
		// Basic movement
		protected virtual void  MotionDistractCheck (){}
		protected virtual void  MotionDistractCheck (Transform distractor){}
		protected virtual void  MotionDistractCheck (Transform[] distractors){}
					 
		protected virtual void LookDistractCheck (){}
		protected virtual void LookDistractCheck (Transform distractor){}
		protected virtual void LookDistractCheck (Transform[] distractors){}
		
		
	}
}