using UnityEngine;
using System.Collections;
using SelectionSystem;
using CameraSystem;


namespace Zone
{
	public class AddZoneVisual : MonoBehaviour {
		
		public Material mat;
		public bool original = true;
		protected void Awake(){
			if (!original) 
			{
				GameObject.Destroy(this);
				return;
				
			}
			MeshRenderer mesh;
			
			original = false;
			
			//Zonevisual
			Transform nt = Instantiate(transform) as Transform;
			
			
			
			nt.name = "ZoneVisual";
			mesh = nt.gameObject.GetComponent<MeshRenderer>();
			
			mesh.material = mat;
			nt.SetParent(transform, true); // something's wrong with position
			nt.localPosition = Vector3.zero;
			nt.gameObject.AddComponent<ZoneToggleActive>();
			
			
			//this
			gameObject.AddComponent<Rigidbody>();
			
			gameObject.AddComponent<BoxCollider>();
		}
	}
}