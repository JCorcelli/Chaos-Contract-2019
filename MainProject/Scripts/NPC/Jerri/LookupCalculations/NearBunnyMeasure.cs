using UnityEngine;
using System.Collections;

namespace NPC.BTree.Jerri
{
	public class NearBunnyMeasure : BeStateRegister {

		public SphereCollider col;
		new protected Transform transform;
		public Transform target;
		public string targetName = "OnClickBunny";
		public string targetTag = "Player";
		
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
			distance = col.radius * col.transform.lossyScale.y;
			
			if (Vector3.Distance(transform.position,target.position) < distance)
				current = true;
			else
				current = false;
		}
		
		
		protected override void Register() {
			hub.Add(ActiveStatesEnum.NearBunny);
			
			
		}
		protected override void Unregister() {
			hub.Remove(ActiveStatesEnum.NearBunny);
			
			
		}
		
	}
}