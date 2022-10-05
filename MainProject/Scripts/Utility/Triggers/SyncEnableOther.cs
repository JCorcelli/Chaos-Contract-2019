using UnityEngine;
using System.Collections;

namespace Game.Utility
{
	
	public class SyncEnableOther : MonoBehaviour {
		public GameObject other;
		// Use this for initialization
		void OnEnable(){
			
			if (other != null)
				other.SetActive( true );
		}
		void OnDisable(){
			if (other != null)
				other.SetActive( false );
		}
	}
}