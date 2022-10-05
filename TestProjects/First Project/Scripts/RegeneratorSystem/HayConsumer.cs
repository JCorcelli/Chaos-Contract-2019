using UnityEngine;
using System.Collections;

namespace TestProject
{
	public class HayConsumer : MonoBehaviour {
		public HayRegeneratorTarget target;
		public bool on;

		private IEnumerator DoConsume(){
			
			while (on)
			{
				target.Consume();
				
				yield return new WaitForSeconds(1f);
			}
			
			yield return null;
			
		}
		void OnTriggerEnter (Collider col){
			// there is one collider
			if (col != target.gameObject.GetComponent<Collider>()) return;
			
			if (!on)
			{
				on	= true;
				StartCoroutine("DoConsume");
			}
			
		}
		void OnTriggerExit (Collider col){
			// there is one collider
			if (col != target.gameObject.GetComponent<Collider>()) return;
			
			if (on)
			{
				on = false;
				StopCoroutine("DoConsume");
			}
		}
		
	}
}