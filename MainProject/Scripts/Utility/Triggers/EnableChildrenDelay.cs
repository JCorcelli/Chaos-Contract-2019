using UnityEngine;
using System.Collections;

namespace Utility.Triggers 
{
	public class EnableChildrenDelay : MonoBehaviour {


		public float delayStart = .0f;
		public float delay = .2f;
		new public Transform transform;
		
		
		protected virtual void OnEnable() 
		{
			if (transform == null)  transform = GetComponent<Transform>(); 
			
			DisableChildren();
			
			StartCoroutine("_OnEnable");
		}
		
		protected IEnumerator _OnEnable () {
			if (delayStart > .01f)
				yield return new WaitForSeconds(delayStart);
			foreach (Transform child in transform)
			{
			
				child.gameObject.SetActive(true);
				yield return new WaitForSeconds(delay);
			}
			
		}
		protected void OnDisable () {
			StopCoroutine("_OnEnable");
			DisableChildren();
		}
		
		protected void DisableChildren(){
			foreach (Transform child in transform)
			{
				child.gameObject.SetActive(false);
			}
		}
	}
}