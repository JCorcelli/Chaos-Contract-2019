using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace NPCSystem
{
	[RequireComponent(typeof(Animator))]
    [RequireComponent(typeof (NavMeshAgent))]
	
	public class RollingCharacter	: UpdateBehaviour {
		
		[SerializeField] float m_AnimSpeedMultiplier = 1f;

		Animator m_Animator;
		
		public bool updatePosition = false;
		public bool updateRotation = false;
		
		protected bool m_IsGrounded;
		const float k_Half = 0.5f;
		protected float m_TurnAmount;
		protected float m_ForwardAmount;
		protected float m_VerticalAmount;
		protected Vector3 m_GroundNormal = Vector3.up;
		protected Vector3 m_GroundPoint = Vector3.up;
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
			
			agent.updatePosition = updatePosition; // I could see setting it on
			agent.updateRotation = updateRotation;
		}
		
		protected void GoToNavAgent () {
			StartCoroutine ("_GoToNavAgent");
			
			
		}
		
		protected IEnumerator _GoToNavAgent() {
			
			agent.updatePosition = true; // 
			yield return null;
			
			agent.updatePosition = false; //
			
		}
		
		protected override void OnFixedUpdate () {
		
            if (target != null)
			{
				if (agent.enabled)
				{
					agent.nextPosition = rigidbody.position;
					// essentially saves the last nearest navmesh position. before falling.
				}
				
				//if (!agent.hasPath)
				// possibly count to 10 and reload the safe position, remove inertia, reset animator
			
				CheckGroundStatus(); //updates groundpoint (foot)
				Vector3 direction = target.position - m_GroundPoint;
				Move(direction);
				
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
		protected Vector3 moveDirection;
		protected Vector3 moveOnPlane;
		public void Move(Vector3 move)
		{

			// convert the world relative moveInput vector into a local-relative
			// turn amount and forward amount required to head in the desired
			// direction.
			
			distance = move.magnitude;
			if (distance > 1f)
				move.Normalize();
			
			//move = transform.InverseTransformDirection(move);
			moveDirection = move;
			
			move = Vector3.ProjectOnPlane(move, m_GroundNormal);
			moveOnPlane = move;
			
			m_TurnAmount = Mathf.Atan2(move.x, move.z);
			m_ForwardAmount = move.z;
			moveDirection = move;
			
			//if (isTurnAssisted)
			ApplyExtraTurnRotation();
		
		
			ApplyPhysicalMovement();
			
			// send input and other state parameters to the animator
			if (m_Animator.enabled)
				UpdateAnimator(move);
		}
		
		
		protected float moveTime = 0f;
		protected float maxSpeedTime = 2f;
		protected float m_StationaryTurnSpeed = 360f;
		protected float m_MovingTurnSpeed = 180f;
		
		
		protected float m_StationaryBoostSpeed = 0f;
		protected float m_MovingBoostSpeed = .1f;
		
		protected float desiredSpeed = 2f;
		
		protected void ApplyPhysicalMovement(){
			
			float boostSpeed = .5f;
			
			
			
			
			
			float savedY = rigidbody.velocity.y;
			Vector3 alteredVelocity = rigidbody.velocity;
			
			
			if (distance < .25f) // stopping distance essentially
			{
				
				rigidbody.AddForce(moveOnPlane * boostSpeed, ForceMode.VelocityChange); // or just kill velocity
			}
			else
			{
				if  (rigidbody.velocity.magnitude > desiredSpeed) return;
				rigidbody.AddForce( moveOnPlane*boostSpeed, ForceMode.VelocityChange);
			}
			
			alteredVelocity = rigidbody.velocity;
			
			
			alteredVelocity.y = savedY;
			rigidbody.velocity = alteredVelocity;
			
		}
		protected void ApplyExtraTurnRotation()
		{
			// help the character turn faster (this is instead of root rotation)
			
			
			if (m_TurnAmount < .02f) return;
			
			float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
			
			rigidbody.rotation *= Quaternion.Euler(0, m_TurnAmount * turnSpeed  * Time.deltaTime, 0);
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


		protected bool touchingGround = true;
		protected float m_GroundCheckDistance = 1f;
		[SerializeField] protected LayerMask activeLayers = 1;
		protected void CheckGroundStatus()
		{
			RaycastHit hitInfo;
#if UNITY_EDITOR
			// helper to visualise the ground check ray in the scene view
			Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
			// 0.1f is a small offset to start the ray from inside the character
			// it is also good to note that the transform position in the sample assets is at the base of the character
			if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance, activeLayers))
			{
				touchingGround = true;
				m_GroundNormal = hitInfo.normal;
				m_GroundPoint = hitInfo.point;
				
			}
			else
			{
				touchingGround = false;
				m_GroundNormal = Vector3.up;
				m_GroundPoint = transform.position;
			}
		}		
				


		
	}

}