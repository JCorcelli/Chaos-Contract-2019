using UnityEngine;
using System.Collections;

namespace Utility.Triggers 
{
	public class EnableChainDelay : MonoBehaviour {


		protected GameObject child; 
		public float delay = .2f;
		
		void Awake () { child = transform.GetChild(0).gameObject; }
		
		
		protected void OnEnable() 
		{
			child.SetActive(false);
			StartCoroutine("_OnEnable");
		}
		
		protected IEnumerator _OnEnable () {
			yield return new WaitForSeconds(delay);
			child.SetActive(true);
		}
		protected void OnDisable () {
			StopCoroutine("_OnEnable");
			child.SetActive(false);
		}
	}
}