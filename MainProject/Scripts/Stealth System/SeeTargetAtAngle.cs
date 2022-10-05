using UnityEngine;
using System.Collections;

namespace StealthSystem
{
	
	public class SeeTargetAtAngle : MonoBehaviour {

		public bool visible = false;
		public LayerMask activeLayer;
		public Transform target;
		public string findByTag;
		public string findByName;
		public float shadowDistance = 5f;
		
		private RaycastHit hit;
		
		
		
		void Start () {
			FindTargetSelf();
			
		}
		
		private void FindTargetSelf () {
			
			if (!target)
			{
				target = gameObject.FindNameXTag(findByName, findByTag).transform;
				
				
			}
			
		}
		
		
		// Update is called once per frame
		//protected bool GetVisible(from, to)
		public bool GetVisible(Vector3 t) {
			// looking for many ?
			
			if (Physics.Linecast(t - transform.forward * shadowDistance,  t, out hit, activeLayer)) {
				if (hit.collider.tag == findByTag || hit.collider.name == findByName)
				{
					return true;
				}
				
			}
			else
			{
				return true;
			}
			return false;
			
			
		}
		public bool GetVisible(Vector3 t, string tname) {
			// looking for specific object
			if (Physics.Linecast(t - transform.forward * shadowDistance,  t, out hit, activeLayer)) {
				if ( hit.collider.name == tname )
				{
					return true;
				}
				
			}
			else
			{
				return true;
			}
			return false;
			
			
		}
		
		
		void Update () {
		
			// ray = thisposition to playerposition;
			if (target == null) return;
			
			visible = GetVisible(target.position);
			
		}
	}
}