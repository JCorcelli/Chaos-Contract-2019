using UnityEngine;
using System.Collections;

namespace ActionSystem.OnActionScripts {
	public class PlaySoundOnMessage : MonoBehaviour, IOnAction {

		private AudioSource sound;
		public string whatAction = ""; // e.g. moving
		
		void Start () {
			
			sound = GetComponent<AudioSource >();
		}
		public void OnAction(ActionEventDetail data) {
			// delay?
			Debug.Log("message = " + data.message,gameObject);
			if (data.message.ToLower() == whatAction.ToLower())
			{
				Debug.Log("play p 4, sound",gameObject);
				sound.Play();
				
			}
			//else
				// give control back?
			
			
		
		}
	}
}