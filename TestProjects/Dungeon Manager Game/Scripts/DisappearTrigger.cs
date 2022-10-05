using UnityEngine;
using System.Collections;


namespace Dungeon
{
	public class DisappearTrigger : MonoBehaviour {

		
		public GameObject target; 
		public string targetName = "PlayerCollider";
		
		
		protected IEnumerator Drop()
		{
			Disappear();
			yield return new WaitForSeconds(3f);
			Reappear();
			
		}
		
		protected void Disappear() {
			
			
			target.SetActive(false);
		}
		protected void Reappear() {
			
			
			target.SetActive(true);
		}
		
		void OnTriggerEnter( Collider col ) {
			if (targetName == col.name) {
				
				StartCoroutine("Drop");
				
			}
		}
		
	}
}