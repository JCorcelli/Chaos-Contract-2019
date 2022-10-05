using UnityEngine;
using System.Collections;

namespace Effects
{
	public class ComponentSpecificEffect : MonoBehaviour {

		
		public void AllowMove(bool moving)
		{
			PlayerAssets.Interface.PlayerInterface.locked = !moving;
		}

	}
}