using UnityEngine;
using System.Collections;

namespace StealthSystem
{
	
	public class SeeTarget : UpdateBehaviour {

		public bool visible = false;
		public LayerMask activeLayer;
		public Transform target;
		public string findByTag;
		public string findByName;
		protected RaycastHit hit;
		
		protected virtual void Start () {
			FindTargetSelf();
		}
		
		protected void FindTargetSelf () {
			
			if (!target)
			{
				target = gameObject.FindNameXTag(findByName, findByTag).transform;
				
				
			}
			
		}
		
		
		// Update is called once per frame
		//protected bool GetVisible(from, to)
		public bool GetVisible(Vector3 t) {
			// looking for many ?
			if (Physics.Linecast(transform.position,  t, out hit, activeLayer)) {
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
			if (Physics.Linecast(transform.position,  t, out hit, activeLayer)) {
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
		
		
		protected override void OnUpdate () {
		
			// ray = thisposition to playerposition;
			if (target == null) return;
			visible = GetVisible(target.position);
			
		}
	}
}