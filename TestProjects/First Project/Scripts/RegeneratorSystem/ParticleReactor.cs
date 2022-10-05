using UnityEngine;
using System.Collections;

namespace TestProject 
{
	public class ParticleReactor : MonoBehaviour {

		public HayRegeneratorTarget target;
		private Collider collisionGoal;
		
		

		public bool doReset = true;
		public bool doKill = true;
		public bool doResize = true;
		
		public float rate = .5f; // rate of disintegration
		public float size = 1f;
		private Vector3 baseSize;
		
		private float o_size;
		private Vector3 o_position;
		private bool triggering = false;
		private ParticleSystem particles;
		
		
		private void onReset (){
			if (!doReset) return;
			transform.position = o_position;
			size = o_size;
			SetSize(size);
			
		}
		
		void Awake (){
			collisionGoal = target.gameObject.GetComponent<Collider>();
			particles = gameObject.GetComponentInChildren<ParticleSystem>();
			
			// m_audio = gameObject.GetComponentInChildren<AudioSource>();
		}
		void Start () {
			o_size = size;
			o_position = transform.position;
			
			baseSize = transform.localScale;
			SetSize(size);
			
		}
		
		private void SetSize(float s){
			
			if (!doResize) return;
			transform.localScale = baseSize * s ;
				
		}
		
		private void OnKill(){
			
						
			var emission = particles.emission; // Stores the module in a local variable
			emission.enabled = false; // Applies the new value directly to the Particle System

			gameObject.SetActive(false);
			triggering = false;
		}
		
		void FixedUpdate(){
			if (!triggering) 
			{
				
				var emission = particles.emission; 
				
				emission.enabled = false;
				return;
			}
			
			this.size -= this.rate * Time.deltaTime;
			
			if (this.size <= 0f) 
			{
				float remainder = this.rate * Time.deltaTime + this.size;
				SetSize(0);
				target.Regenerate(remainder);
				
				onReset();
				if (doKill)
					OnKill();
			}
			else
			{
				
				target.Regenerate(this.rate * Time.deltaTime);
			}
			
			
			SetSize(size);
			
			
		}
		
		
		void OnTriggerEnter (Collider col){
			// this primes the emission
			if (col != collisionGoal) 
				return;
			
			triggering = true;
			
				
			var emission = particles.emission; 
			
			emission.enabled = true; 
			
		}
		
		
		void OnTriggerExit(Collider col){
			// there is one collider
			if (col != collisionGoal) 
				return;
			
			triggering = false;
			var emission = particles.emission; 
			
			emission.enabled = false; 
				
			
		}
		
		
	}
}