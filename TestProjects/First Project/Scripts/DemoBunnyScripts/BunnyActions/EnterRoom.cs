using UnityEngine;
using System.Collections;
using UnityStandardAssets.Cameras;

// think of this as a prototype to trigger a room's responsiveness
// player enters room
// all objects activate & become selectable
// 

namespace PlayerAssets.Game
{
	public class EnterRoom : MonoBehaviour {
		public bool focused = false;



		public GameObject cameraRig;
		public GameObject cameraDestination;
		public GameObject targetFOV; // need to adjust the field of view for the camera
		public GameObject targetPosition;

		 



		private void OnTriggerEnter (Collider c) {
			
			if (c.tag == "Player")
			{

				cameraRig.SetActive (true); 



				focused = true;
				TargetFieldOfView tfv = cameraRig.GetComponentInChildren<TargetFieldOfView>();
				if (tfv != null)
					tfv.SetTarget( targetFOV.transform );

				targetPosition.transform.parent = cameraDestination.transform;
				targetPosition.transform.localRotation = Quaternion.identity;
				targetPosition.transform.localPosition = Vector3.zero;

				

				// overlay.SetActive( true );

				// overlay.transform.position = transform.position;
				// the warning should always be visible, like a shouted text balloon
			}
			else if (c.tag == "NPC")
			{
				// turn on warning
			}
		}
		private void OnTriggerExit (Collider c) {
			
			if (c.tag == "Player")
			{
				focused = false;
				// overlay.SetActive( false ); // technically I'm in that room.
			}
			else if (c.tag == "NPC")
			{
				// shut off warning
			}
		}
	}
}
