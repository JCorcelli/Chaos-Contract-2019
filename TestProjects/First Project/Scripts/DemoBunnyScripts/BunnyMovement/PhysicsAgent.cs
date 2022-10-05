using UnityEngine;
using System.Collections;

namespace PlayerAssets.Game
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	
	
	public class PhysicsAgent : MonoBehaviour 
	{
		
		[SerializeField] public bool isFollowing = false;
		[SerializeField] public float speed = 1f;
		[SerializeField] public float stoppingDistance = 0.1f;
		public Vector3 desiredVelocity = Vector3.zero;
		public float groundCheckDistance = 1f;
		private Vector3 target;
		private Rigidbody m_Rigidbody;
		private Vector3 m_GroundNormal;
		private bool m_IsGrounded;
		

		// Use this for initialization
		private void Start () 
		{
			m_Rigidbody = GetComponent<Rigidbody>();
			
		
		}
		
		public void SetDestination(Vector3 destination)
		{
			target = destination;
			isFollowing = true;
			PlayerStatus.idle = false;
		}

		
		// Update is called once per frame
		private void FixedUpdate () 
		{
			
			if (isFollowing) 
			{
				CheckGroundStatus();
				if (!m_IsGrounded) 
				{
					desiredVelocity = Vector3.zero;
					return;
					
				}
			
				// example move = Vector3.ProjectOnPlane(move, m_GroundNormal);
				
				Vector3 p = transform.position; // this position
				Vector3 t = Vector3.ProjectOnPlane(target, m_GroundNormal);	// goal target position
				
				
				if ((t - Vector3.ProjectOnPlane(p, m_GroundNormal)).magnitude <= stoppingDistance) // distance check
				{
					desiredVelocity = Vector3.zero;
					isFollowing = false;
					PlayerStatus.idle = true;
				}
				else
				{
					Vector3 n = Vector3.MoveTowards(p, t, speed);
					desiredVelocity = (n - p); // velocity must be from origin
					m_Rigidbody.AddForce(desiredVelocity);
				}
				
			}
		
		}
		
		
		void CheckGroundStatus()
		{
			RaycastHit hitInfo;
#if UNITY_EDITOR
			// helper to visualise the ground check ray in the scene view
			Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * groundCheckDistance));
#endif
			// 0.1f is a small offset to start the ray from inside the character
			// it is also good to note that the transform position in the sample assets is at the base of the character
			if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, groundCheckDistance))
			{
				m_GroundNormal = hitInfo.normal;
				m_IsGrounded = true;
			}
			else
			{
				m_IsGrounded = false;
				m_GroundNormal = Vector3.up;
			}
		}
		
	}
}