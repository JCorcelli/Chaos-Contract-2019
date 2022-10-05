using UnityEngine;
using System.Collections;

namespace PlayerAssets.Interface
{
	[RequireComponent (typeof (PlayerInterface))]
	public class PlayerInterfaceHelper : MonoBehaviour {

		// Use this for initialization
		
		public bool looking = false;
		public bool locked = false;
		void Awake () {
			PlayerInterface.looking = looking;
			PlayerInterface.locked = locked;
			
			StartCoroutine("UpdateVars");
		}
		
		// Update is called once per frame
		private IEnumerator UpdateVars () {
			while (true){
				looking = PlayerInterface.looking;
				locked = PlayerInterface.locked;
				
				yield return new WaitForSeconds(0.5f);
			}
		}
	}
}