using UnityEngine;
using System.Collections;
using Utility.Managers;

namespace Utility
{
	public class ParticleSettingOnAwake : MonoBehaviour {

		
		public ParticleSystem particles;
		public float startSize = -1f;
		
		protected void Awake() {
			if (particles == null) particles = GetComponentInChildren<ParticleSystem>();
			
			
			if (startSize > 0)
			{
				var main = particles.main;
				main.startSize = startSize;
				
			}
		}
		
	}
}