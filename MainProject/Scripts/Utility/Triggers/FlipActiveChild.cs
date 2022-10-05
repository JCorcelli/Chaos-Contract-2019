using UnityEngine;
using System.Collections;


namespace Utility.Trigger
{
	public class FlipActiveChild : MonoBehaviour {


		protected GameObject child; 
		public bool repeat = true;
		public float startDelay = 0f;
		public float timeOn = 3f;
		public float timeOff = 1f;
		
		void Awake () { 
			child = transform.GetChild(0).gameObject; 
		}
		
		protected void OnEnable() {
			
			StartCoroutine("_FlipActive");
		}
		
		protected IEnumerator _FlipActive () {
			
			float delay = startDelay;
			
			if (delay > 0)
			{
				yield return new WaitForSeconds(delay);
			}
				child.SetActive(!child.activeSelf);
			while (repeat)
			{
				if (child.activeSelf) // child is active
					delay = timeOn;
				else
					delay = timeOff;
			
				yield return new WaitForSeconds(delay);
				child.SetActive(!child.activeSelf);
				
			}
		}
		
		
		protected void OnDisable () {
			StopCoroutine("_FlipActive");
			child.SetActive(false);
		}
	}
}