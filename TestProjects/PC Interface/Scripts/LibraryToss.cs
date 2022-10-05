using UnityEngine;
using System.Collections;
using SelectionSystem;
using CameraSystem;

namespace NPCSystem
{
	public class LibraryToss : AbstractButtonHandler {
		public HidingSpotHub hub;
		
		public string group = "1";
		public bool playing = false;
		
		
		protected void Awake(){
			if (hub == null) hub = GetComponentInParent<HidingSpotHub>();
			if (hub == null) {
				Debug.Log("no hub, this broke", gameObject);
				return; 
			}
			hub.onChange += OnChange;
			thisCollider = GetComponent<Collider>();
			
		}
		public virtual void Show(){
			
			if (hub.group == group) 
			{
				hub.group = "";
				hub.OnChange();
			}
		}
		public virtual void Hide(){
			
			
			
			hub.group = group;
			hub.OnChange();
			
			
			
		}
		protected Collider thisCollider;
		public LayerMask layerMask = 0;
		public float exradius = 1f;
		public float explosionForce = 2f;
		public Transform targetB {
			
			get{return CameraHolder.instance.targetB;}
		}
		
		protected override void OnRelease(){
			base.OnRelease();
			if (!playing) return;
			Vector3 position = thisCollider.ClosestPointOnBounds(targetB.position);
			Collider[] cols = Physics.OverlapSphere(position,exradius,layerMask);
			Rigidbody rb;
			
			
			LibraryTossable lb;
			foreach (Collider col in cols)
			{
				lb = col.gameObject.GetComponent<LibraryTossable>();
				if (lb != null && lb.group == group)
				{
					rb = col.gameObject.GetComponent<Rigidbody>();
					if (rb != null)
						rb.AddExplosionForce(explosionForce, position,exradius, 0f, ForceMode.VelocityChange); //0f upward force
					
					
				}
			}
			//AddExplosionForce
		}
		public virtual void OnChange(){
			if (hub.group == group)
			{
				playing = true;
			}
			else
			{
				playing = false;
			}
		}
	}
}