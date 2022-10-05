using UnityEngine;
using System.Collections;


namespace PlayerAssets.Interface
{
	public class PlayerInterface : MonoBehaviour {

		// Use this for initialization
		private static bool _looking = false;
		private static bool looking_new = false;
		public static bool locked = false;
	
		public static bool looking // the change next frame
		{
			get 
			{
				return (_looking);
			}
			set 
			{ 
				looking_new = value; 
			}
			
		}
		
		
		private void LateUpdate()
		{
			_looking = looking_new;
		}
	}
}