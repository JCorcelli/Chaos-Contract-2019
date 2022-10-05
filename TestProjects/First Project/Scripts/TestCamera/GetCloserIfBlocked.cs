using UnityEngine;
using System.Collections;
using TestProject;

namespace TestProject.Cameras
{
	[RequireComponent (typeof (SeeTarget))]
	
	public class GetCloserIfBlocked : MonoBehaviour {
		
		public float sphereCastRadius = 0.1f;
		private SeeTarget st;
		
		private Ray ray;
		private RaycastHit hit;
		private Vector3 origin;
		private bool moved = false;

		// Use this for initialization
		void Start () {
			st = gameObject.GetComponent<SeeTarget>();
			origin = transform.localPosition;
		
		}
		
		private void CastSphere(){
			float rayLength = (transform.position - st.target.position).magnitude;
			ray = new Ray(st.target.position, transform.position - st.target.position);

			if (Physics.SphereCast(ray, sphereCastRadius, out hit, rayLength, st.activeLayer)) {
				if (hit.collider.tag == st.findByTag || hit.collider.name == st.findByName)
				{
					return; // I hit the wrong thing?
				}
				if (!moved)
				{
					origin = transform.localPosition;
					moved = true;
				}
				transform.position = hit.point - sphereCastRadius * ray.direction;
			}
		}
		// Update is called once per frame
		void LateUpdate () {
			if (st.visible) 
			{
				if (moved)
				{
					transform.localPosition = origin;
					CastSphere();
					return;
				}
				return;
			}
			CastSphere();
			
		}
	}
}