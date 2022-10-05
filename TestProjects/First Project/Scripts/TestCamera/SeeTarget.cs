using UnityEngine;
using System.Collections;

namespace TestProject
{
	
	public class SeeTarget : MonoBehaviour {

		public bool visible = false;
		public LayerMask activeLayer;
		public Transform target;
		public string findByTag;
		public string findByName;
		private RaycastHit hit;
		
		void Start () {
			FindTargetSelf();
		}
		
		private void FindTargetSelf () {
			
			if (!target)
			{
				bool tagged = findByTag != "";
				bool named = findByName != "";
				
				if (tagged)
				{
					if (named)
					{
						foreach (GameObject t in GameObject.FindGameObjectsWithTag(findByTag))
						{
							if (t.name == findByName)
							{
								target = t.transform;
								break;
							}
							
						}
					}
					else
						target = GameObject.FindWithTag(findByTag).transform;
				}
				else if (named)
					target = GameObject.Find(findByName).transform;
				
				
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
		
		
		void Update () {
		
			// ray = thisposition to playerposition;
			if (target == null) return;
			visible = GetVisible(target.position);
			
		}
	}
}