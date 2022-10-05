using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace TestProject 
{
	[RequireComponent (typeof (Rigidbody))]
	public class MultibodyTracker : MonoBehaviour {

		public Rigidbody target;
		[System.Serializable]
		public class TrackerEvent : UnityEvent { }
		public TrackerEvent doOnHit;
		
		protected Vector3 contactPoint;
		protected bool delayOn = false;
		
		public void SetTarget(GameObject go){
			target = go.GetComponent<Rigidbody>();
		}
		public void SetTarget(Rigidbody go){
			target = go;
		}
		public IEnumerator timedDelay(){
			
			yield return new WaitForSeconds(2f);
			delayOn = false;
		}
		protected void OnTriggerEnter(Collider col) {
			// check if touching rigidbody once
			if (!delayOn && target != null && col.GetComponent<Rigidbody>() == target)
			{
				delayOn = true;
				contactPoint = col.transform.position; // potential memory hazard(is it?)
				StartCoroutine("timedDelay");
				doOnHit.Invoke();
			}
		}
		
	}
}