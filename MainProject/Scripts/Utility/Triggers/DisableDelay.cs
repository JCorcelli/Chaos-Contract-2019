using UnityEngine;
using System.Collections;

namespace Utility.Triggers 
{
	public class DisableDelay : MonoBehaviour {


		public float delay = 3.5f;
		
		
		
		protected void OnEnable() 
		{
			StopCoroutine("_OnEnable");
			StartCoroutine("_OnEnable");
		}
		
		protected IEnumerator _OnEnable () {
			yield return new WaitForSeconds(delay);
			gameObject.SetActive(false);
		}
		protected void OnDisable () {
			StopCoroutine("_OnEnable");
		}
	}
}