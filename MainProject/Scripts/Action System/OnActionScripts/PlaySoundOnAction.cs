using UnityEngine;
using System.Collections;

namespace ActionSystem.OnActionScripts {
	public class PlaySoundOnAction : MonoBehaviour, IOnAction {

		protected AudioSource sound;
		public string whatAction = ""; // e.g. sound name
		public Transform sourceAnchor;
		new protected Transform transform;
		
		void Start () {
			transform = GetComponent<Transform>();
			sound = GetComponent<AudioSource >();
		}
		public void OnAction(ActionEventDetail data) {
			// delay?
			if (data.what.ToLower() == whatAction.ToLower())
			{
				if (sourceAnchor != null)
					transform.position = sourceAnchor.position;
				sound.Play();
				
			}
			
			
		
		}
	}
}