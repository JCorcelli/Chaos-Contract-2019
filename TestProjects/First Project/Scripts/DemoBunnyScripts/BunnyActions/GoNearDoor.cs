using UnityEngine;
using System.Collections;

// simplest way to tell if something is close to a door
namespace PlayerAssets.Game
{
	public class GoNearDoor : MonoBehaviour {
		public bool focused = false;
		public GameObject overlay;
		// Use this for initialization
		private void Awake () {

			if (overlay == null) overlay = GameObject.Find("PropOverlay");


			// probably works better for OnTriggerStay, 
			// modify the object regardless of being player or npc

			// if (focused)
			//{
			//	which side of the door is it on? (requires door prefab model
			// // door only has 2 sides right?
			// if front then assign front room to player stats (require room number or name)
			// else assign back room
			//}
		}

		private void Start () {
			overlay.SetActive (false);
		}

		private void OnTriggerEnter (Collider c) {
			
			if (c.tag == "Player")
			{
				focused = true;
				PlayerStatus.near_doorway = true;
				overlay.SetActive( true );
				overlay.transform.position = transform.position;
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
				PlayerStatus.near_doorway = false;
				overlay.SetActive( false );
			}
			else if (c.tag == "NPC")
			{
				// shut off warning
			}
		}
	}
}