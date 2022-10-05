using UnityEngine;
using System.Collections;


namespace NPCSystem
{
	public interface IPosable  {

		// Use this for initialization
		void SetPose (int newPose);
		int GetPose ();
		
	}
}