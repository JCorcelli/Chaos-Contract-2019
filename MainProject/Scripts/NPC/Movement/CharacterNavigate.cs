using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace NPCSystem
{
	[RequireComponent(typeof(Animator))]
    [RequireComponent(typeof (NavMeshAgent))]
	
	public class CharacterNavigate	: UpdateBehaviour {
		
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
		public string _targetName;
		public string _targetTag;
		
		protected NavMeshAgent agent;
		new protected Rigidbody rigidbody;
		
		
		protected virtual void Start () {
			if (_targetName != "" )
				target = gameObject.FindNameXTag(_targetName, _targetTag).transform;
			
			rigidbody = GetComponent<Rigidbody>();
			m_IsGrounded = true;
			m_Animator = GetComponent<Animator>();
			agent = GetComponentInChildren<NavMeshAgent>();
			
			agent.updatePosition = updatePosition;
			agent.updateRotation = updateRotation;
		}
		protected override void OnUpdate () {
		
            if (target != null)
			{
				if (agent.enabled)
				{
					agent.nextPosition = rigidbody.position;
					agent.SetDestination(target.position);
				
					transform.position = new Vector3(agent.nextPosition.x, rigidbody.position.y, agent.nextPosition.z);
				}
					
				if (!agent.hasPath) 	
				{
					return;
				}
				Move(agent.desiredVelocity * Vector3.Distance(transform.position, target.position));
				
			}
            else
            {
                //agent.ResetPath();
                // We still need to call the character's move function, but we send zeroed input as the move param.
                Move(Vector3.zero);
				if (agent.enabled)
					agent.ResetPath();
            }
		}
		
        public void SetTarget(Transform target)
        {
            this.target = target;
        }
		
		
		protected float distance = 0f;
		public void Move(Vector3 move)
		{

			// convert the world relative moveInput vector into a local-relative
			// turn amount and forward amount required to head in the desired
			// direction.
			distance = move.magnitude;
			if (distance > 1f)
				move.Normalize();
			
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