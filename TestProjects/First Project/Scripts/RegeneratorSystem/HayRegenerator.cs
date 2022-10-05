using UnityEngine;
using System.Collections;

namespace TestProject
{
	public class HayRegenerator : MonoBehaviour {

		// Use this for initialization
		public bool doReset = true;
		public bool doKill = true;
		public bool doResize = true;
		public HayRegeneratorTarget target;
		public float rate = .2f; // rate of disintegration
		public float size = 1f;
		private Vector3 baseSize;
		
		private float o_size;
		private Vector3 o_position;
		private bool triggering = false;
		
		private void onReset (){
			if (!doReset) return;
			transform.position = o_position;
			size = o_size;
			SetSize(size);
			
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
		
		void FixedUpdate(){
			if (!triggering) return;
			triggering = false;
			
			this.size -= this.rate * Time.deltaTime;
			
			
			if (this.size <= 0f) 
			{
				float remainder = this.rate * Time.deltaTime + this.size;
				SetSize(0);
				target.Regenerate(remainder);
				
				onReset();
				if (doKill)
					gameObject.SetActive(false);
			}
			else
			{
				
				target.Regenerate(this.rate * Time.deltaTime);
			}
			
			
			SetSize(size);
			
			
		}
		void OnTriggerStay (Collider col){
			// there is one collider
			if (col != target.gameObject.GetComponent<Collider>()) return;
			
			triggering = true;
			
			
			
			
		}
	}
}