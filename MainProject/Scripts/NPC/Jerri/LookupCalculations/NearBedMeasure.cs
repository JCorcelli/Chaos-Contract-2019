using UnityEngine;
using System.Collections;

namespace NPC.BTree.Jerri
{
	public class NearBedMeasure : BeStateRegister {

		public SphereCollider col;
		new protected Transform transform;
		public Transform target;
		public string targetName = "H.BedEnter";
		public string targetTag = "NPC";
		public LayerMask layermask = 1 << 0;
		
		protected float distance;
		protected JerriBeStateHUB hub;
		protected void Start () {
			if (col == null) col = GetComponent<SphereCollider>();
				
			if (col == null)
				Debug.Log("col == null",gameObject);
			
			transform = GetComponent<Transform>();
			hub = GetComponentInParent<JerriBeStateHUB>();
			
			if (target == null)
				target = gameObject.FindNameXTag(targetName, targetTag).transform;
			if (target == null)
				Debug.Log("target == null",gameObject);
		}
		
		
		protected override void OnUpdate () {
			RaycastHit hitInfo;
			if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, 2f, layermask))
			{
				if (hitInfo.collider.name == "BedCollider")Register2();
				else
					Unregister2();
					
			}
			else
			{
				
					Unregister2();
			}
			
			/*
			distance = col.radius * col.transform.lossyScale.y;
			
			if (Vector3.Distance(transform.position,target.position) < distance)
				current = true;
			else
				current = false;
			*/
		}
		
		protected ActiveStatesEnum state = ActiveStatesEnum.NearBed;
		protected ActiveStatesEnum state2 = ActiveStatesEnum.OnBed;
		protected bool registered2 = false;
		protected override void Register() {
			hub.Add(state);
			
			
		}
		protected override void Unregister() {
			hub.Remove(state);
			
			
		}
		protected void Register2() {
			if (registered2) return;
			hub.Add(state2);
			registered2 = true;
			
			
		}
		protected void Unregister2() {
			if (!registered2) return;
			hub.Remove(state2);
			registered2 = false;
			
		}
		
		protected void OnTriggerEnter(Collider col)
		{
			if (col.name == "BedCollider")
			{
				current = true;
			}
		}
		protected void OnTriggerExit(Collider col)
		{
			if (col.name == "BedCollider")
			{
				current = false;
			}
		}
		
	}
}