using UnityEngine;
using System.Collections;

namespace Utility.Tree
{
	public class SendReadyDelay : MonoBehaviour {

		// prototype
		
		
		protected SelectHUB select;
		
		public void OnEnable() {
			select = GetComponentInParent<SelectHUB>();
			Load();
		}
		
		protected void Load() {
			StartCoroutine("_DelayLoad");
		}
		
		public float delay = 1f;
		protected IEnumerator _DelayLoad() {
			
			
			yield return new WaitForSeconds(delay);
			
			select.Ready();
			
		}
		
	}
}