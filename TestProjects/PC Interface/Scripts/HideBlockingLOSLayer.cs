using UnityEngine;
using System.Collections;
using CameraSystem;
using UnityEngine.Rendering;

namespace CameraSystem 
{
	public class HideBlockingLOSLayer : UpdateBehaviour {
		public LayerMask layerMask = 0;
		
		protected MeshRenderer mesh;
		protected MeshCollider thisCollider;
		
		
		
		// Use this for initialization
		protected void Awake () {
		
			mesh = GetComponent<MeshRenderer>();
			thisCollider = GetComponent<MeshCollider>();
			
			
		}
		
		public Transform targetA {
			
			get{return CameraHolder.instance.targetA;}
		}
		
		protected override void OnUpdate(){
			base.OnUpdate();
			
			RaycastHit[] hits;
			Vector3 start = Camera.main.transform.position;
			Vector3 end = targetA.position;
			Vector3 direction = end - start;
			
			hits = Physics.RaycastAll(start, direction.normalized, direction.magnitude, layerMask);
			
			
			gameObject.layer = 0;
			foreach (RaycastHit hit in hits)
			{
				if (hit.collider == thisCollider)
				{
					
					gameObject.layer = 12;
					break;
				}
					
			}
		}
	}
}