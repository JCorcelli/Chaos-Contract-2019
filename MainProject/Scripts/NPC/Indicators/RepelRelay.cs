using UnityEngine;
using System.Collections;

namespace NPCSystem
{
	public class RepelRelay : MonoBehaviour, IRepellable {

		protected IRepellable target;
		
		protected void Awake () {
			target = transform.GetComponentInParent<IRepellable>();
			
		}
		
		
		
		public void Repel (float force = 0f, Vector3 source = new Vector3(), float radius = 0f){
			if ((Object)target == this || target == null) Debug.Log("err", gameObject);
			target.Repel(force, source, radius);
			
		}
	}
}