using UnityEngine;
using System.Collections;
using CameraSystem;
using UnityEngine.Rendering;

namespace CameraSystem 
{
	public class HideBlockingLOS : UpdateBehaviour {
		public LayerMask layerMask = 0;
		
		protected MeshRenderer mesh;
		protected MeshCollider thisCollider;
		protected Material baseMat;
		public Material hiddenMat;
		
		
		// Use this for initialization
		protected void Awake () {
		
			mesh = GetComponent<MeshRenderer>();
			thisCollider = GetComponent<MeshCollider>();
			baseMat = mesh.sharedMaterial;
			hiddenMat = (Material)Resources.Load("Materials/ZoneVisualOutline", typeof(Material));
			
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
			
			mesh.material = baseMat;
			
			foreach (RaycastHit hit in hits)
			{
				if (hit.collider == thisCollider)
				{
					mesh.material = hiddenMat;
					
				}
					
			}
		}
	}
}