using UnityEngine;
using System.Collections;

namespace TestProject 
{
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(CharacterController))]
	
	public class CharacterMove : MonoBehaviour {
		
		[SerializeField] float m_MovingTurnSpeed = 360;
		[SerializeField] float m_StationaryTurnSpeed = 180;
		//[SerializeField] float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
		[SerializeField] float m_MoveSpeedMultiplier = 1f;
		[SerializeField] float m_AnimSpeedMultiplier = 1f;
		[SerializeField] float m_GroundCheckDistance = 0.1f;

		Animator m_Animator;
		bool m_IsGrounded;
		const float k_Half = 0.5f;
		float m_TurnAmount;
		float m_ForwardAmount;
		float m_VerticalAmount;
		Vector3 m_GroundNormal;
		
		// modded to charactercontroller
		//float m_CapsuleHeight;
		//Vector3 m_CapsuleCenter;
		CharacterController m_Capsule;
		
		void Start () {
			m_IsGrounded = true;
			m_Animator = GetComponent<Animator>();
			m_Capsule = GetComponent<CharacterController>();
			//m_CapsuleHeight = m_Capsule.height;
			//m_CapsuleCenter = m_Capsule.center;
		
		}
		
		
		
		public void Move(Vector3 move, bool crouch, bool jump)
		{

			// convert the world relative moveInput vector into a local-relative
			// turn amount and forward amount required to head in the desired
			// direction.
			if (move.magnitude > 1f) move.Normalize();
			move = transform.InverseTransformDirection(move);
			CheckGroundStatus();
			move = Vector3.ProjectOnPlane(move, m_GroundNormal);
			m_TurnAmount = Mathf.Atan2(move.x, move.z);
			m_ForwardAmount = move.z;
			
			if (m_IsGrounded)
				ApplyExtraTurnRotation();
			//ScaleCapsuleForCrouching(crouch);
			//PreventStandingInLowHeadroom();

			// send input and other state parameters to the animator
			if (m_Animator.enabled)
				UpdateAnimator(move);
		}


		
		void UpdateAnimator(Vector3 move)
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




		void ApplyExtraTurnRotation()
		{
			// help the character turn faster (this is in addition to root rotation in the animation)
			float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);

			transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
		}
		
		public void OnAnimatorMove()
		{
			// we implement this function to override the default root motion.
			// this allows us to modify the positional speed before it's applied.
			if (Time.deltaTime <= 0) return;
			
			if (m_IsGrounded)
			{
				Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;

				// we preserve the existing y part of the current velocity.
				// v.y = m_Capsule.velocity.y;
				m_Capsule.SimpleMove(v );
			}
			else
			{
				m_Capsule.SimpleMove(m_Animator.deltaPosition * Time.deltaTime);
			}
		}	
		
		
		protected RaycastHit hitInfo;
		protected void CheckGroundStatus()
		{
#if UNITY_EDITOR
			// helper to visualise the ground check ray in the scene view
			Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
			// 0.1f is a small offset to start the ray from inside the character
			// it is also good to note that the transform position in the sample assets is at the base of the character
			
			if (m_Capsule.isGrounded || Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
			{
				m_GroundNormal = hitInfo.normal;
				m_IsGrounded = true;
				//m_Animator.applyRootMotion = true;
			}
			else
			{
				m_IsGrounded = false;
				m_GroundNormal = Vector3.up;
				//m_Animator.applyRootMotion = false;
			}
		}
				


		
	}

}