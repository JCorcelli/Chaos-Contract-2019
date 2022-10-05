using UnityEngine;
using System.Collections;

namespace TestProject {
	[RequireComponent(typeof(ParticleSystem))]
	public class MakeParticleReal : MonoBehaviour {
		
		// Use this for initialization
		
		public GameObject realCopy;
		public bool timed = true;
		public float timerDelay = 1f; // seconds
		private ParticleSystem m_System;
		private ParticleSystem.Particle[] m_Particles;
		
		
		private IEnumerator TimedAwake(){
			while (true)
			{
				int numParticlesAlive = m_System.GetParticles(m_Particles);
				Vector3 pos;
				// Change only the particles that are alive
				
				for (int i = 0; i < numParticlesAlive; i++)
				{
					pos = m_Particles[i].position;
					//rot = m_Particles[i].rotation;
					GameObject oInstance = Instantiate(realCopy, pos, realCopy.transform.localRotation) as GameObject;
					Rigidbody rig = oInstance.GetComponent<Rigidbody>();
					//rig.angularVelocity = m_Particles[i].angularVelocity;
					rig.velocity = m_Particles[i].velocity;
					oInstance.transform.parent = realCopy.transform.parent;
					oInstance.SetActive(true);
					m_Particles[i].remainingLifetime = -1; // kill (should I limit the number of real copies?)
				}
				// replace particles
				m_System.SetParticles(m_Particles, numParticlesAlive);
				yield return new WaitForSeconds(timerDelay);
			}
		}
		void OnEnable () {
			if (m_System == null)
				m_System = GetComponent<ParticleSystem>();


			var main = m_System.main; 
			
			
			if (m_Particles == null || m_Particles.Length < main.maxParticles)
				m_Particles = new ParticleSystem.Particle[main.maxParticles]; 
			// needed to instance the m_particles first
			StartCoroutine("TimedAwake");
		}
		void OnDisable () {
			StopCoroutine("TimedAwake");
		}
		
	}
}