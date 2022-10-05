using UnityEngine;
using System.Collections;


namespace PlayerAssets.Game
{
	public class PlayerStatus : MonoBehaviour {
		public static bool focused = true;
		public static bool alive = true;
		public static bool tired = false;
		public static bool awake_time_remaining = false;
		public static bool hidden_by_prop = false; // is x in a prop? (or a shadow)
		public static bool exit_is_safe = true; // is hiding spot being viewed
		public static bool near_doorway = false;
		
		private static bool _hidden = false; // is x hidden
		private static bool hidden_new = false; // did x just hide
		public static int bliss = 0; // percentage

		public static bool idle = true;
		public static bool zoom = false;
		public static bool menu = false;
		
		public static int clicks = 0; // number of times player clicks the bunny
		public static bool clicked = false; // has player clicked bunny since last time this was reset?
		
		
		public static bool hidden // make hidden_new more important
		{
			get 
			{
				return (_hidden || hidden_by_prop);
			}
			set 
			{ 
				if (value == true) 
				{
					hidden_new = true; 
					_hidden = true;
				}
				else if (!hidden_new) // require a player's hidden_new status to be removed before being unhidden
				// else it's false
				_hidden = false;
			}
			
		}
		
	}
	
}