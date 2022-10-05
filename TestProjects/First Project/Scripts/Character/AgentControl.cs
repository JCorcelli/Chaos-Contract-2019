using UnityEngine;
using System.Collections;

namespace TestProject 
{
	[RequireComponent(typeof(CharacterController))]
	public class AgentControl : MonoBehaviour {
		
		[SerializeField] public float speed = 1f;
		[SerializeField] public float stoppingDistance = 0.1f;
		public Vector3 desiredVelocity = Vector3.zero;
		
		
		
		private CharacterController m_Rigidbody;
		private Vector3 m_GroundNormal = Vector3.zero;
		
		private Vector3 target;
		
		// Use this for initialization
		void Start () {
			m_Rigidbody = GetComponent<CharacterController>();
			Debug.Log("DEPRECATION WARNING: Ground normal not reassigned.");
		}
		public void SetDestination(Vector3 destination)
		{
			target = destination;
			//PlayerStatus.idle = false;
		}
		
		// Update is called once per frame
		private void FixedUpdate () 
		{
			
			
		
			Vector3 p = transform.position; // this position
			
			Vector3 t = Vector3.ProjectOnPlane(target, m_GroundNormal);	// goal target position
			
			
			if ((t - Vector3.ProjectOnPlane(p, m_GroundNormal)).magnitude <= stoppingDistance) // distance check
			{
				desiredVelocity = Vector3.zero;
				//isFollowing = false;
				//PlayerStatus.idle = true;
				
				
		
			}
			else
			{
				Vector3 n = Vector3.MoveTowards(p, t, speed);
				desiredVelocity = (n - p); // velocity must be from origin
				
				 // n is the position, which was originally used by the navigation agent for exact movement.
				
				m_Rigidbody.SimpleMove(desiredVelocity*Time.deltaTime); 
			}
			
		
		}
		
	
	}
}