using UnityEngine;
using System.Collections;

namespace CameraSystem 
{
	public class ThrowStone : MonoBehaviour 
	{
		public Transform stone;
		public LayerMask activeLayer;
		private Vector3 startPosition;
		private Vector3 endPosition;
		public float distance = 10f;
		private Ray ray;
		
		void Start() {
			if (stone == null)
				stone = transform;
		}
		
		private void _ThrowStone(){
			
			ray = new Ray(stone.parent.position, endPosition - startPosition);
			
			stone.localPosition = ray.direction * distance;
			stone.LookAt(stone.parent);
			
		}
		void Update()
		{
			bool up = Input.GetButtonUp("mouse 1") || Input.GetButton("mouse 1");
				
			bool dn = Input.GetButtonDown("mouse 1");
				
			if (up || dn)
			{
				
				ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, Mathf.Infinity, activeLayer)) 
				{
					// different if I click a bunny?
					if (dn) 
						startPosition = hit.point;
					endPosition = hit.point;
				}
				_ThrowStone();
			}
			
		}
		
		
	}
}
