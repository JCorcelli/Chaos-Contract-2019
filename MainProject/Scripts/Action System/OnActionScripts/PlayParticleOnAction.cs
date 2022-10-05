using UnityEngine;
using System.Collections;

namespace ActionSystem.OnActionScripts {
	public class PlayParticleOnAction : MonoBehaviour, IOnAction {

		protected  ParticleSystem particle;
		public string whatAction = ""; // e.g. sound name
		public Transform sourceAnchor;
		new protected Transform transform;
		
		protected void Awake () {
			
			transform = GetComponent<Transform>();
			if (particle == null)
				particle = GetComponentInChildren<ParticleSystem>();
		}
		public void OnAction(ActionEventDetail data) {
			// delay?
			if (particle == null) return;
			if (data.what.ToLower() == whatAction.ToLower())
			{
				if (sourceAnchor != null)
					transform.position = sourceAnchor.position;
				particle.Play();
				
			}
			
			
		
		}
	}
}