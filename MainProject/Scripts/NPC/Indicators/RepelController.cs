using UnityEngine;
using System.Collections;

namespace NPCSystem
{
	public class RepelController : UpdateBehaviour {

		public IRepellable target;
		public string targetName = "PresenceIndicator";
		public string targetTag = "PlayerRig";
		protected float radius;
		
		void Awake () {
			target = gameObject.FindNameXTag(targetName, targetTag).GetComponent<IRepellable>();
			if (target == null) Debug.Log("no IRepallable in target",gameObject);
			
			radius = GetComponent<SphereCollider>().radius;
		}
		
		
		public bool causesJump = true;
		
		protected override void OnFixedUpdate () {
			
			if (causesJump)
				target.Repel(1f, transform.position, radius);
			else 
				target.Repel();
			
				
		}
	}
}