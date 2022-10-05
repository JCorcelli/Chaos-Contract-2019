using UnityEngine;
using System.Collections;

namespace NPCSystem
{

	public class ConstrainRigidbody : UpdateBehaviour {

		// Use this for initialization
		public SphereCollider distanceLimiter;
        public string _sphereColliderName;
        public string _sphereColliderTag;
		
		
		// Update is called once per frame
		protected Vector3 m_GroundNormal = Vector3.up;
		protected bool touchingGround = false;
		new public Rigidbody rigidbody;
		[SerializeField] protected LayerMask activeLayers = 1;
		protected void Start () {
			
			if (_sphereColliderName != "")
			{
				distanceLimiter = gameObject.FindNameXTag(_sphereColliderName, _sphereColliderTag).GetComponent<SphereCollider>();
			}
			rigidbody = GetComponent<Rigidbody>();
		}
		protected override void OnLateUpdate () {
			CheckGroundStatus();
			
			if (touchingGround)
				StayInZone();
		}
		protected void StayInZone() {
			float d = Vector3.Distance( distanceLimiter.transform.position, transform.position ); // distance from center
			
			float scaledDistanceLimiter = distanceLimiter.radius * distanceLimiter.transform.lossyScale.y;
			if ( d   > scaledDistanceLimiter  )
			{
				Vector3 backmove = Vector3.MoveTowards(transform.position, distanceLimiter.transform.position, d - scaledDistanceLimiter); // extra distance after subtracting the max
				
				rigidbody.MovePosition( backmove);
				//rigidbody.velocity = Vector3.zero;
			}
			
			
		}
		protected void CheckGroundStatus()
		{
			RaycastHit hitInfo;
			
			
			// 0.1f is a small offset to start the ray from inside the character
			// it is also good to note that the transform position in the sample assets is at the base of the character
			
			if (Physics.Raycast(transform.position + (transform.up * 0.1f), -transform.up, out hitInfo, 0.4f, activeLayers))
			{
				touchingGround = true;
				m_GroundNormal = hitInfo.normal;
				
					
				
				
				
			}
			else
			{
				touchingGround = false;
				m_GroundNormal = Vector3.up;
				
			}
			
			
		}
		
	}
}