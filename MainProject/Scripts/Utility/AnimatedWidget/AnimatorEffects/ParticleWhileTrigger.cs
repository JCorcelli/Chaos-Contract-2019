using UnityEngine;
using System.Collections;
using Utility.Managers;

namespace Utility
{
	public class ParticleWhileTrigger : TriggerNode {

		
		public ParticleSystem particles;
		
		protected override void Awake() {
			base.Awake();
			if (particles == null) particles = GetComponentInChildren<ParticleSystem>();
		}
		
		protected override void onTriggerEnter () {
			particles.Play();
		}
		
		
		protected override void onTriggerExit () {
			particles.Stop();
		
		}
	}
}