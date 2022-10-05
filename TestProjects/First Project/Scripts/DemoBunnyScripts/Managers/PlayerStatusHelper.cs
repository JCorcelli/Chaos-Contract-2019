using UnityEngine;
using System.Collections;


namespace PlayerAssets.Game
{
	[RequireComponent (typeof (PlayerStatus))]
	public class PlayerStatusHelper : MonoBehaviour {
		public bool focused = false;
		public bool alive = true;
		public bool tired = false;
		public bool awake_time_remaining = false;
		public bool hidden = false;
		public bool hidden_by_prop = false;
		public bool exit_is_safe = false;
		public bool near_doorway = false;
		public int bliss = 0;
		public bool idle = false;
		public bool zoom = false;
		public bool menu = false;
		
		
		public int clicks = 0; // number of times player clicks the bunny
		public bool clicked = false; // number of times player clicks the bunny
		
		private void Start() {
			StartCoroutine("UpdateVars");
		}
		
		// Update is called once per frame
		private IEnumerator UpdateVars () {
			
			while (true) 
			{
				alive = 		PlayerStatus.alive;
				tired = 		PlayerStatus.tired;
				focused = 		PlayerStatus.focused;
				awake_time_remaining = 	PlayerStatus.awake_time_remaining;
				hidden = 		PlayerStatus.hidden;
				hidden_by_prop = PlayerStatus.hidden_by_prop;
				exit_is_safe = 	PlayerStatus.exit_is_safe;
				near_doorway = 	PlayerStatus.near_doorway;
				bliss = 		PlayerStatus.bliss;
				idle = 			PlayerStatus.idle;
				zoom = 			PlayerStatus.zoom;
				menu = 		PlayerStatus.menu;
				clicks = 		PlayerStatus.clicks;
				clicked = 		PlayerStatus.clicked;
				
				yield return new WaitForSeconds(0.5f);
			}
			
		}
		
	}
}