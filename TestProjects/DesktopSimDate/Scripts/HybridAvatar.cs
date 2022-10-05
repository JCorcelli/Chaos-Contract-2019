using UnityEngine;
using UnityEngine.AI;
using System.Collections;

using SelectionSystem;

namespace NPCSystem
{
	[RequireComponent(typeof(Animator))]
	
	
	public class HybridAvatar	: AbstractButtonHandler {
		
		[SerializeField] float m_AnimSpeedMultiplier = 1f;

		Animator m_Animator;
		
		
		const float k_Half = 0.5f;
		protected float m_TurnAmount;
		protected float m_ForwardAmount;
		protected float m_VerticalAmount;
		protected Vector3 m_GroundNormal = Vector3.up;
		protected Vector3 m_GroundPoint = Vector3.up;
		public Transform target;
		public string _targetName;
		public string _targetTag;
		
		public Vector3 gravity;
		public float terminalVelocity = 1.5f;
		
		new protected Rigidbody rigidbody;
		public Transform grapple;
		
		public Transform adjustedTransform;
		public bool grabbing = false;
		
		protected Vector3 m_GrabNormal = Vector3.up;
		protected Vector3 m_GrabPoint = Vector3.up;
		
		protected virtual void Start () {
			
			gravity = Physics.gravity;
			if (_targetName != "" )
				target = gameObject.FindNameXTag(_targetName, _targetTag).transform;
			
			
			
			
			
			grapple = transform.Find("Grapple");
			adjustedTransform = transform.Find("Mesh");
			rigidbody = GetComponent<Rigidbody>();
			touchingGround = true;
			m_Animator = GetComponent<Animator>();
			
			rigidbody.useGravity = false;
			// freezepositionY was added due to the 100% (tiny) push that occurs while walking between two perfectly fit box colliders.
			//rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			CheckGroundStatus();
			
		}
		protected Vector3 stablePosition = Vector3.zero;
		
		public Vector3 GetPosition() {
			
			return rigidbody.position;
		}
		
		public void OnFall () {
			// teleports with .position
			rigidbody.position = stablePosition;
			rigidbody.velocity = Vector3.zero; /// stop falling
			
			if (!pressed) stationary = true;
			
		}
		protected NavMeshHit nvmhit;
		
		protected bool stationary = true; // for shooting... while mouse 1 still moves you.
		
		
		protected override void OnDisable(){
			base.OnDisable();
			doNotPin = false;
			StopAllCoroutines();
		}
		protected override void _OnPress(){
			base._OnPress();
			
			if ( Input.GetButtonDown("shift") )
			{
				stationary = true;
			}
			
		}
		protected override void OnHold() {
			
			if ( Input.GetButton("shift") )
			{
				stationary = true;
				return;
			}
			else
				stationary = false;
				
		}
		
		
		protected override void OnFixedUpdate () {
			if (!Time.deltaTime.IsZero())
				CheckGroundStatus(); //updates groundpoint (foot)
			
			if (touchingGround)
			{
				stablePosition = transform.position;
			}
            if (target != null)
			{
			
			
				
				//if (!agent.hasPath)
				// possibly count to 10 and reload the safe position, remove inertia, reset animator
			
				Vector3 direction = target.position - m_GroundPoint;
				
				
				Move(direction);
				
			}
            else
            {
                //agent.ResetPath();
                // We still need to call the character's move function, but we send zeroed input as the move param.
                Move(Vector3.zero);
				
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
			
			moveDirection = move;
			moveOnPlane =Vector3.ProjectOnPlane(move, m_GroundNormal);
			
			move = transform.InverseTransformDirection(move);
			
			move = Vector3.ProjectOnPlane(move, m_GroundNormal);
			
			m_TurnAmount = Mathf.Atan2(move.x, move.z);
			m_ForwardAmount = move.z;
			
			//if (isTurnAssisted)
				
			
			if (distance > 0.01f)
			{
				ApplyExtraTurnRotation();
			
				ApplyPhysicalMovement();
			}
			else if (!(touchingGround || grabbing))
				rigidbody.AddRelativeForce(gravity, ForceMode.Acceleration); // not flying ;)
			
			// send input and other state parameters to the animator
			if (m_Animator.enabled)
				UpdateAnimator(move);
		}
		
		
		
		protected float m_StationaryTurnSpeed = 360f;
		protected float m_MovingTurnSpeed = 180f;
		
		
		protected float m_StationarygroundForce = 0f;
		protected float m_MovinggroundForce = .1f;
		
		public float maxGroundSpeed = 2f;
		public float maxClimbSpeed = 1f;
		public float deadlySpeed = 4f;
		
		public float groundForce = 30f;
		public float stopForce = .1f;
		public float breakingDistance = 1f;
		public float climbForce = 15f;
		protected Vector3 currentHeading;
		public bool falling = false;
		protected void ApplyPhysicalMovement(){
			
			Vector3 alteredVelocity = rigidbody.velocity;
			currentHeading = Vector3.ProjectOnPlane(alteredVelocity, m_GroundNormal);
			float currentSpeed = currentHeading.magnitude;
			
			// probably falling, or thrown
			if (!(touchingGround || grabbing) && currentSpeed > deadlySpeed) 
			{
				falling = true;
				rigidbody.AddRelativeForce(gravity, ForceMode.Acceleration); // not flying ;)
				return;
			}
			falling = false;
			
			
			
			
			float savedY = rigidbody.velocity.y;
			
			
			
			if (grabbing) 
			{
				float fidelity = Vector3.Dot(m_GrabNormal, Vector3.up);
				
				if (fidelity > .98f && currentSpeed < maxClimbSpeed)
				{
					// flat ledge
					
					rigidbody.AddForce(Vector3.up * climbForce, ForceMode.Acceleration);
				}
				else if (fidelity > .68f && currentSpeed < maxGroundSpeed)
				{
					// diagonal
					
				
					rigidbody.position = m_GroundPoint;
					
					rigidbody.AddForce(Vector3.up * climbForce, ForceMode.Acceleration);
					rigidbody.AddForce(moveOnPlane * groundForce, ForceMode.Acceleration);
				}
				// else it's basically a sheer cliff
				

			}
			
			else if (!doNotPin && touchingGround) // not trying to jump, jumping, flying either!
			{
				
				savedY = 0;
				rigidbody.position = m_GroundPoint;
			}
			else
				
				rigidbody.AddRelativeForce(gravity, ForceMode.Acceleration);
			
			//falling! well, not necessarily
			
			
			
			
			if (stationary || distance < breakingDistance) // stopping distance essentially
			{
				
				
				
				if (currentSpeed > stopForce)
					rigidbody.AddForce(-currentHeading.normalized * stopForce, ForceMode.VelocityChange);
					
				else
					rigidbody.AddForce(-currentHeading, ForceMode.VelocityChange);
				
			}
			
			if (stationary) return;
			
			// probably falling, or thrown
			
			if  (grabbing || currentSpeed > maxGroundSpeed) 
			{
				alteredVelocity = rigidbody.velocity;
				alteredVelocity.y = savedY;
				
				rigidbody.velocity = alteredVelocity;
				return;
			}
			// else runnig
			
			rigidbody.AddForce( moveOnPlane *groundForce, ForceMode.Acceleration);
				
			alteredVelocity = rigidbody.velocity;
			
			
			alteredVelocity.y = savedY;
			rigidbody.velocity = alteredVelocity;
			
		}
		protected void ApplyExtraTurnRotation()
		{
			// help the character turn faster (this is instead of root rotation)
			
			
			//if (m_TurnAmount < .02f) return;
			
			float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
			
			rigidbody.rotation *= Quaternion.Euler(0, m_TurnAmount * turnSpeed  * Time.deltaTime, 0);
		}


		
		protected void UpdateAnimator(Vector3 move)
		{
			// update the animator parameters
			m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
			m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
			//m_Animator.SetBool("Crouch", m_Crouching);
			m_Animator.SetBool("OnGround", touchingGround);


			if (touchingGround && move.magnitude > 0)
			{
				m_Animator.speed = m_AnimSpeedMultiplier;
			}
			else
			{
				// don't use that while airborne
				m_Animator.speed = 1;
			}
		}


		public bool touchingGround = true;
		public bool doNotPin = false;
		public float m_GroundCheckDistance = 1f;
		[SerializeField] protected LayerMask activeLayers = 1;
		
		
		protected void CheckGroundStatus()
		{
			RaycastHit hitInfo;
			
#if UNITY_EDITOR
			// in scene view
			Debug.DrawLine(grapple.position + (Vector3.up * 0.02f), grapple.position + (Vector3.down * grapple.localPosition.y * grapple.lossyScale.y));
#endif
			
			
			// check if grapple available
			if (Physics.Raycast(grapple.position + (Vector3.up * 0.02f), Vector3.down, out hitInfo, grapple.localPosition.y * grapple.lossyScale.y, activeLayers))
			{
				
				grabbing = true;
				m_GrabNormal = hitInfo.normal; // I could check if it's flat enough to climb
				m_GrabPoint = hitInfo.point; // important
				
			}
			else
			{
				if (grabbing) 
				{
					Vector3 v = rigidbody.velocity;
					v.y = 0f;
					rigidbody.velocity = v;
					DelayGroundPin();
				}
				grabbing = false;
				m_GrabNormal = Vector3.up;
				m_GrabPoint = transform.position;
			}
			
			
			
			
			
			
#if UNITY_EDITOR
			// in scene view
			Debug.DrawLine(transform.position + (transform.up * 0.1f), transform.position + (-transform.up * m_GroundCheckDistance));
#endif
			// 0.1f is a small offset to start the ray from inside the character
			// it is also good to note that the transform position in the sample assets is at the base of the character
			
			if (Physics.Raycast(transform.position + (transform.up * 0.1f), -transform.up, out hitInfo, m_GroundCheckDistance, activeLayers))
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
				
		protected void DelayGroundPin(){
			StopCoroutine("_DelayGroundPin");
			StartCoroutine("_DelayGroundPin");
		}

		protected IEnumerator _DelayGroundPin(){
			doNotPin = true;
			yield return new WaitForSeconds(.1f);
			doNotPin = false;
		}
		protected override void OnUpdate(){
			base.OnUpdate();
			if (transform.position.y < -10f)
				OnFall();
		}
	}

}