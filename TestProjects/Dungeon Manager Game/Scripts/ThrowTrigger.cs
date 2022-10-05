using UnityEngine;
using System.Collections;


namespace Dungeon
{
	public class ThrowTrigger : MonoBehaviour {

		
		public Transform target; // option 1, set target
		public Vector3 direction = Vector3.up; // option 2
		public float throwForce = 100f; 
		public string targetName = "PlayerCollider";
		
		public float delay = 0f;
		public int count = 0; 
		protected void Start() {
			
			Transform tv = DungeonVars.keyObjectTransform;
			if (tv.GetComponent<Rigidbody>() == null) Debug.LogError("there's no rigidbody on keyObjectTransform found in Dungeonvars, ob is highlighted"+tv.name, tv.gameObject);
			
			
			// set optional throw direction to aim at target, directly
			if (target != null) direction = (target.position - transform.position).normalized;
		}
		protected IEnumerator Throw()
		{
			if (delay > 0.01f) yield return new WaitForSeconds(delay);
			_Throw();
			
		}
		
		protected void _Throw() {
			
			Rigidbody rb = DungeonVars.keyObjectTransform.GetComponent<Rigidbody>();
			
			rb.AddForce(direction * throwForce, ForceMode.VelocityChange);
		}
		
		
		protected void OnTriggerEnter( Collider col ) {
			if (targetName == col.name) {
				
				count ++;
				StartCoroutine("Throw");
				
			}
		}
		protected void OnTriggerExit( Collider col ) {
			if (targetName == col.name) {
				
				count --;
				if (count <= 0)
				{
					count = 0;
					StopCoroutine("Throw");
				}
				
			}
		}
		
	}
}