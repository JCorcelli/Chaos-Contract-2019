using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace TestProject 
{
	[RequireComponent(typeof(Animator))]
    [RequireComponent(typeof (NavMeshAgent))]
	
	public class CharacterNavigate	: MonoBehaviour {
		
		[SerializeField] float m_AnimSpeedMultiplier = 1f;

		Animator m_Animator;
		
		public bool updatePosition = true;
		public bool updateRotation = false;
		
		bool m_IsGrounded;
		const float k_Half = 0.5f;
		float m_TurnAmount;
		float m_ForwardAmount;
		float m_VerticalAmount;
		Vector3 m_GroundNormal = Vector3.up;
		public Transform target;
		protected NavMeshAgent agent;
		
		
		protected void Start () {
			m_IsGrounded = true;
			m_Animator = GetComponent<Animator>();
			agent = GetComponentInChildren<NavMeshAgent>();
			
			agent.updatePosition = updatePosition;
			agent.updateRotation = updateRotation;
		}
		private void Update () {
		
            if (target != null)
			{
				if (agent.enabled)
					agent.SetDestination(target.position);
				Move(agent.desiredVelocity, false, false);
				
			}
            else
            {
                //agent.ResetPath();
                // We still need to call the character's move function, but we send zeroed input as the move param.
                Move(Vector3.zero, false, false);
            }
		}
		
        public void SetTarget(Transform target)
        {
            this.target = target;
        }
		
		
		public void Move(Vector3 move, bool crouch, bool jump)
		{

			// convert the world relative moveInput vector into a local-relative
			// turn amount and forward amount required to head in the desired
			// direction.
			if (move.magnitude > 1f) move.Normalize();
			move = transform.InverseTransformDirection(move);
			
			move = Vector3.ProjectOnPlane(move, m_GroundNormal);
			m_TurnAmount = Mathf.Atan2(move.x, move.z);
			m_ForwardAmount = move.z;
			
			// send input and other state parameters to the animator
			if (m_Animator.enabled)
				UpdateAnimator(move);
		}


		
		protected void UpdateAnimator(Vector3 move)
		{
			// update the animator parameters
			m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
			m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
			//m_Animator.SetBool("Crouch", m_Crouching);
			m_Animator.SetBool("OnGround", m_IsGrounded);


			if (m_IsGrounded && move.magnitude > 0)
			{
				m_Animator.speed = m_AnimSpeedMultiplier;
			}
			else
			{
				// don't use that while airborne
				m_Animator.speed = 1;
			}
		}




		
				


		
	}

}