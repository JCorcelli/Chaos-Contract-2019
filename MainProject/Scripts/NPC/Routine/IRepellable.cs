using UnityEngine;
using System.Collections;


namespace NPCSystem
{
	public interface IRepellable  {

		// Use this for initialization
		void Repel (float force = 0f, Vector3 source = new Vector3(), float radius = 0f);
		
	}
}