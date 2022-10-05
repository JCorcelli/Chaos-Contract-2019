using UnityEngine;
using System.Collections;


// optimization
// [if gameObject is selected (clicked on)]
// 		enable
// 		check for all NPC via personal view angles
//		
// else disable

namespace PlayerAssets.Game
{
	public class HideInsideProp : MonoBehaviour {
		private bool triggering = false;
		public bool isOccupied = false;
		public bool focused = false;
		public GameObject overlay;
		
		private GameObject player;
		// Use this for initialization
		private void Awake () {
		
			player = GameObject.FindWithTag("PlayerRig");
			if (overlay == null) overlay = GameObject.Find("PropOverlay");

		}

		private void Start () {
			overlay.SetActive( false );
		}
		
		// Update is called once per frame
		
		public void Clicked(){
			focused = !focused;
			
		}
		void Update () {
			// temporary
			if (isOccupied && Input.GetButton("mouse 1"))
			{
				isOccupied = false;
				PlayerStatus.hidden_by_prop = false;
				overlay.SetActive( false );
				player.SetActive( true );


			}
			else if (triggering && focused)
			{
				isOccupied = true;
				focused = false;
				PlayerStatus.hidden_by_prop = true;
				overlay.SetActive( true );
				player.SetActive( false );

				PlayerStatus.hidden = true; // statushelper adjusts this this
				triggering = false;
			}
			
			// final
			// [if gameobject is not being looked at]
			// 		PlayerStatus.exit_is_safe
			// 		make head pop out of the correct side,
			// 		addition: reveal a visible bunny at the moment of entry

		}
		

		void OnTriggerEnter (Collider c) {
			// INCOMPLETE, is it the player
			if (c.tag == "Player")
			{
				if (!PlayerStatus.hidden_by_prop)
					triggering = true;
			}
		}
		void OnTriggerExit (Collider c) {
			// INCOMPLETE, is it the player
			if (c.tag == "Player")
			{
				if (!PlayerStatus.hidden_by_prop)
					triggering = false;
			}
		}
	}
}