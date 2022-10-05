using System;
using UnityEngine;
using System.Collections;

namespace PlayerAssets.Game
{
	public class BunnyCover : MonoBehaviour {
			
		
		void OnTriggerStay (Collider c) {
			if (c.tag == "Player")
			{
				PlayerStatus.hidden = true;
			}
		}
		
		void OnTriggerExit (Collider c) {
			if (c.tag == "Player")
			{
				PlayerStatus.hidden = false;
			}
		}
	}
}