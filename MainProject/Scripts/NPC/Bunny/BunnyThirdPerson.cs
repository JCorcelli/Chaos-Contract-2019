using UnityEngine;
using System.Collections;
using ActionSystem;
using Utility.Managers;
using SelectionSystem;
using Zone;
using CameraSystem;

namespace NPCSystem
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(Animator))]
	public class BunnyThirdPerson : AbstractAnyHandler, IMovable, IRepellable, IPosable
	{
		public Transform targetB {
			
			get{return CameraHolder.instance.targetB;}
		}
		
		// animation, basic movement
		public Transform target;
		public bool followTarget = false;
		public float m_JumpingTurnSpeed = 580;
		public float m_MovingTurnSpeed = 60;
		public float m_StationaryTurnSpeed = 90;
		public float moveSpeedMultiplier = 1f;
		public float animSpeedMultiplier = 1f;
		
		protected float _animSpeedMultiplier;
		protected float _moveSpeedMultiplier;
		
		protected Animator anim;
		
		protected float pose_delay = 1f;
		public int pose = 0;
		
		protected int baseState = 0;
		protected int subState = 0;
		public float runHop = 2f;
		public float walkHop = 1f;
		protected bool isWalking = false;
		protected bool isRunning = false;
		protected bool isTurnAssisted = false;
		// terrain
		[SerializeField] float m_GroundCheckDistance = 1f;
		[SerializeField] LayerMask activeLayers = 1;
		
		// movement
		public float stoppingDistance = 2.2f;
		public float cautionDistance = 1f;
			//rb
		new public Rigidbody rigidbody;
		protected const float k_Half = 0.5f;
		protected float m_TurnAmount;
		protected float m_ForwardAmount;
		protected Vector3 m_GroundNormal;
			//idle
		protected float idleTime = 0.0f;
		protected float idleDelay = 3f;
		protected bool idle = false;
		protected bool stationary = false;
		public bool standing = false;
			//skill
		public bool tense = false;
		public bool boxing = false;
		public float chargeTimer = 0f;
		public bool charged = false;
		public bool superCharged = false;
			//run
		protected float runDelay = 3f;
		protected float runTime = 3f;
		
		protected bool hyperactive = false;
		
		
			//jump
		public float superJumpForce = 5f;
		public float runJumpForce = 5f;
		public float maxJumpForce = 3.5f;
		public float jumpForce = 1.5f;
		public float diveForce = 1.5f;
		
		public bool bdiving = false;
		protected float tjumpStart = 0f;
		protected float tjumpThreshold = 0f;
		public bool jumping = false;
		public bool bjumping = false;
		public bool touchingGround = false;
		public bool doubleJumpEnabled = false;
		public bool doubleJumped = false;
		
		public float jumpAngle = 45f;
		public float jumpTime = 0f;
		
		
		public float defaultFriction = 4f;
		protected float friction = 4f;
		//climb
		public Transform bunnyFront;
		protected float climbCooldown = 0f;
		
		//tidying
		protected Vector3 lastValidPos = new Vector3();
		
		//custom
		protected EffectHUB effect;
		
		public float repelTimer = 5f;
		
		
		public void OnFall () {
			// teleports with .position
			rigidbody.position = lastValidPos;
			rigidbody.velocity = Vector3.zero; /// stop falling
			followTarget = false;
			
			if (!pressed) stationary = true;
			
		}
		public void Stomp(){
			anim.CrossFadeInFixedTime("Skill_Stomp", .1f, 0, 0f);
		}
		protected override void _OnPress(){
			base._OnPress();
			// Decision: does the zone command group appear here?
			idleTime = 0.0f; // it thought I was idle even if I pressed a button
			if (ZoneGlobal.inZone) return;
			Transform t = Camera.main.transform;
			
			if ( Input.GetButtonDown("mouse 1") )
			{
				followTarget = true;
				
			
				if (Input.GetButton("shift"))
				{
					
					bdiving = true;
					stationary = false;
				}
				if (Input.GetButton("skill"))
				{
					// boxing
					
					anim.CrossFadeInFixedTime("Skill_Punch", 0.1f, 0, 0f);
				}
				else
				{
					tense = false;
					stationary = false;
					boxing = false;
			
					CancelPose();
				
					
				}
				
				if (Input.GetButton("use"))
					Debug.Log("splash");
				if (standing)
					Debug.Log("dig");
				else 
					standing = false;
			}
				
			else if ( Input.GetButtonDown("shift") )
			{
				if (jumping) Jump();
				
				else if ( Input.GetButton("mouse 1") )
				{
					
					if (charged)
					{
						Discharge();
					}
					RunJump();
					
				}
				else
				{
					if (charged)
					{
						Stomp();
						Discharge();
					}
					else
						Jump();
					
				}
				
				tense = false;
			}
			else if ( Input.GetButtonDown("left") )
			{
				// this is a group of actions like frog hopos
				CameraHop(t.right * -1 );
			}
			else if ( Input.GetButtonDown("right") )
			{
				CameraHop(t.right);
			}
			else if ( Input.GetButtonDown("up") )
			{
				CameraHop(t.up);
			}
			else if ( Input.GetButtonDown("down") )
			{
				CameraHop(t.up * -1);
			}
			
			else if ( Input.GetButtonDown("skill") ) 
			{
				// Hold F
				// Initial step to parry or catch
				
				if (jumping) Jump(); //wk
				else if (tense)
				{
					boxing = true;
					SetPose(5); // boxing
					
				}
				else if (charged)
				{
					// kinetic gear
					Debug.Log("ke ga");
				}
					
				else if ( Input.GetButton("mouse 1") )
					Debug.Log("hit");
				else if (isRunning) Jump();
				else
					SetStationary();
				
				
			}
			else if ( Input.GetButtonDown("use") ) 
			{
				if (tense)
				{
					Debug.Log("stand");
					standing = true;
				}
			}
		}
		
		protected void Discharge() {
			charged = false;
			chargeTimer = 0f;
			animSpeedMultiplier = 1f;
			
		}
		protected void Charge() {
			chargeTimer += Time.deltaTime;
			if (chargeTimer > 2f)
			{
				charged = true;
				animSpeedMultiplier = 1.4f;
				
			}
			if (chargeTimer > 4f)
			{
				if (!superCharged) Debug.Log("WOTB");
				superCharged = true;
			}
		}
		protected void CancelCharge() {
			chargeTimer = 0f;
		}
		protected override void OnHold() {
			
			idleTime = 0.0f; // it thought I was idle even if I pressed a button
			
			if (!ZoneGlobal.inZone && Input.GetButton("shift") ) Charge();
			else
				CancelCharge();
				
				
			if ( Input.GetButton("skill") ) 
			{
				// Hold F
				// Catch
			}
		}
		protected void SetStationary() {
			
			stationary = true;
			rigidbody.velocity = Vector3.Scale(rigidbody.velocity, Vector3.up);
		}
		
		protected override void OnRelease() {
			if ( Input.GetButtonUp("shift") ) 
			{
				// Release Shift
				
				chargeTimer = 0f;
			}
			else if ( Input.GetButtonUp("skill") ) 
			{
				// Release F, brace, block
				tense = true;
				boxing = false;
				CancelPose();
			}
				
		}
		
		protected void Start()
		{
			if (target == null) target = targetB;
			effect = GetComponentInParent<EffectHUB>();
			anim = GetComponent<Animator>();
			rigidbody = GetComponent<Rigidbody>();
			
			

			rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			
			friction = defaultFriction;
			_animSpeedMultiplier = animSpeedMultiplier;
			_moveSpeedMultiplier = moveSpeedMultiplier;
		}

		
		
		
		
		
		
		public void Repel (float force, Vector3 source, float radius) {
			if (Time.time < 1f) return; // may be falsely repelled early
			
			// using IRepellable
			if (force > 0)
			{
				friction = 3f;
				if (source != Vector3.zero)
				{
					rigidbody.AddExplosionForce( force, source, radius, 0.0f, ForceMode.Acceleration);
				}
				
				if (!hyperactive)
				{
					Jump();
				}
				_animSpeedMultiplier = 1.5f;
				_moveSpeedMultiplier = .6f;
				StopCoroutine("RepelCooldown");
				if (gameObject.activeSelf) StartCoroutine("RepelCooldown");
					
			}
			hyperactive = true;
			StopCoroutine("HyperCooldown");
			if (gameObject.activeSelf) StartCoroutine("HyperCooldown");
		}
		
		protected IEnumerator HyperCooldown() {
			
			yield return new WaitForSeconds(repelTimer);
			hyperactive = false;
			_animSpeedMultiplier = animSpeedMultiplier;
			_moveSpeedMultiplier = moveSpeedMultiplier;
			
		}
		protected IEnumerator RepelCooldown() {
			
			yield return new WaitForSeconds(repelTimer);
			friction = defaultFriction;
		}
		public void CancelPose() {
			pose = 0;
			anim.SetInteger("Pose", pose); 
		}
		public void SetPose(int i) {
			// I suppose this'll be a base state change later or setting busy
			pose = i;
			anim.SetInteger("Pose", pose); 
			anim.SetTrigger("PoseT"); // <state>.Idle
			
			//StopCoroutine("DelayedEndPose");
			//StartCoroutine("DelayedEndPose");
		}
		
		protected IEnumerator DelayedEndPose()
		{
			
			yield return new WaitForSeconds(pose_delay);
			
			if (pose > 0)
			{
				pose = 0;
				anim.SetInteger("Pose", 0); // <state>.Idle
			}
		}
		
		public int GetPose() {
			return pose;
		}
		
		
		public void Move(Vector3 move)
		{
			// using IMovable
			// convert the world relative moveInput vector into a local-relative
			// turn amount and forward amount required to head in the desired
			// direction.
			if (stationary)
			{
				rigidbody.velocity = Vector3.Scale(rigidbody.velocity, Vector3.up);
				move = Vector3.zero;
			}
			if (pose > 0) 
			{
				if (hyperactive)
				{
					SetPose( 0);
				}
				return; // pose is fine and busy
			}
			isWalking = anim.GetCurrentAnimatorStateInfo(0).IsName("Walking");
			isRunning = anim.GetCurrentAnimatorStateInfo(0).IsName("Running");
			isTurnAssisted = isWalking || isRunning || jumping;
			
			float distance = move.magnitude;
			
			
			if (!stationary && distance > stoppingDistance) 
			{
				// select
				// run or walk?
				if (hyperactive ) 
				{
					anim.SetInteger("SubState", 2); // <state>.run
					subState = 2;
					
				}
				else
				{
					anim.SetInteger("SubState", 1); // <state>.walk
					subState = 1;
				}
				
				anim.SetInteger("BaseState", 1); // actual motion
				baseState = 1;
				idleTime = 0.0f;
				
					
				idle = false;
					// probably moving
			}
			else
			{
				idleTime += Time.deltaTime;
				
				
				if (!idle && !hyperactive && (idleDelay <= idleTime || stationary))
				{
					
					SetPose(1);
					
					idle = true;
					
					
				}
				anim.SetInteger("SubState", 0);  //Idle
				subState = 0;
				
				
					
			}
			move = transform.InverseTransformDirection(move);
			
			
			move = Vector3.ProjectOnPlane(move, m_GroundNormal);
			m_TurnAmount = Mathf.Clamp(Mathf.Atan2(move.x, move.z), -1f, 1f);
			
			
			if (distance < cautionDistance) 	
				m_ForwardAmount = (distance / cautionDistance);
			else
				
				m_ForwardAmount = 1f;

			
			if (isTurnAssisted)
				ApplyExtraTurnRotation();


			// send input and other state parameters to the animator
			UpdateAnimator(move);
			Climb();
		}



		
		protected virtual void UpdateAnimator(Vector3 move)
		{
			// update the animator parameters
			
			float nTime = Mathf.Repeat(anim.GetCurrentAnimatorStateInfo(0).normalizedTime, 1);
			
			anim.SetFloat("Forward", m_ForwardAmount , 0.1f , Time.deltaTime);
				
				
					
			
			anim.SetFloat("Turn", m_TurnAmount , 0.5f , Time.deltaTime);
			
			
			if (move.magnitude > 0)
			{
			
				// modify the upwards force, so I can climb small inclines
				if (touchingGround && nTime >= .3f && nTime < .7f)
				{
					if (isRunning)
					{
						AnimHop(runHop);
					}
					else if (isWalking)
						AnimHop(walkHop);
				}

				// the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
				// which affects the movement speed because of the root motion.
			
				
				anim.speed = _animSpeedMultiplier;
			}
			else
			{
				anim.speed = 1; // If it's standing still then it ought to play idle normally. perhaps.
			}
		}


		
		protected void ApplyExtraTurnRotation()
		{
			// help the character turn faster (this is instead of root rotation)
			
			float nTime = Mathf.Repeat(anim.GetCurrentAnimatorStateInfo(0).normalizedTime, 1);
			
			
			if (touchingGround && baseState == 1 && isWalking) // I want walking state basically
			{
				
				if (nTime > 0.412f && nTime < .9f)
				{
					return;
				}
					
			}
			float turnSpeed;
			if (jumping) 
				turnSpeed = m_JumpingTurnSpeed;
			else
				turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
			rigidbody.rotation *= Quaternion.Euler(0, m_TurnAmount * turnSpeed  * Time.deltaTime, 0);
		}

		
		public void StartJump() {
			
			if (!touchingGround)
			{
				bjumping = true;
				tjumpThreshold = Time.time + .5f;
				return;
			}
			jumping = true;
			touchingGround = false;
			followTarget = true;
			tjumpStart = 0.1f + Time.time;
			
			//anim.SetTrigger("JumpT");
			anim.CrossFadeInFixedTime("Action_Jump", .1f, 0, 0f);
		}

		protected void AnimHop(float force) {
			if (!touchingGround) return;
			
			rigidbody.AddRelativeForce( Vector3.up *force, ForceMode.VelocityChange);
			
		}
		protected void CameraHop(Vector3 force) {
			
			if (jumping)
				
			{
				if (!doubleJumpEnabled || doubleJumped ) return;
				doubleJumped = true;
			}
			else if (!touchingGround) return;
			StartJump();
			force = Vector3.ProjectOnPlane(force, m_GroundNormal) ;
			
			target.position = rigidbody.position + force.normalized;
			
			force = (force.normalized + Vector3.up) * jumpForce;
			
			rigidbody.velocity = Vector3.zero;
			rigidbody.AddForce( force , ForceMode.VelocityChange);
			
		}
		
		
		public void Dive() {
			if (jumping)
				
			{
				if (!doubleJumpEnabled || doubleJumped ) return;
				doubleJumped = true;
			}
			StartJump();
			
			
			Projectile p = new Projectile();
			
		// init variables
			p.endP = target.position; // projected to start plane?
			p.startP = transform.position;
		
			p.force = jumpForce = 0.1f;
			p.maxForce = maxJumpForce ;
			
			p.SetLowestAngle();
			
			
			jumpForce = p.force;
			jumpAngle = p.angle;
			jumpTime = Time.time + .2f;
			// angle of reach?
			
			rigidbody.velocity = Vector3.zero;
			rigidbody.AddForce(  p.vector , ForceMode.VelocityChange);
			// work on it
		}
		public void Jump() {
			if (jumping)
				
			{
				if (!doubleJumpEnabled || doubleJumped ) return;
				doubleJumped = true;
			}
			StartJump();
			
			if (!jumping) return;
			
			
			Projectile p = new Projectile();
			
		// init variables
			p.endP = target.position; // projected to start plane?
			p.startP = transform.position;
		
			p.force = jumpForce = 0.1f;
			p.maxForce = maxJumpForce ;
			
			p.SetBestAngle();
			
			
			jumpForce = p.force;
			jumpAngle = p.angle;
			jumpTime = Time.time + p.maxt ;
			// angle of reach?
			
			rigidbody.velocity = Vector3.zero;
			rigidbody.AddForce(  p.vector , ForceMode.VelocityChange);
			// work on it
		}
		public void RunJump() {
			
			if (!touchingGround) return;
			
			rigidbody.AddForce( Vector3.up * runJumpForce , ForceMode.VelocityChange);
			
		}
		
		protected void FrogHop(Vector3 force) {
			if (!touchingGround) return;
			StartJump();
			
			rigidbody.AddRelativeForce( force, ForceMode.VelocityChange);
			
			
		}
		public void OnAnimatorMove()
		{
			// we implement this function to override the default root motion.
			// this allows us to modify the positional speed before it's applied.
			if (Time.deltaTime > 0)
			{
				
				
				
				
				
			
				Vector3 v = (anim.deltaPosition * _moveSpeedMultiplier) / Time.deltaTime;

				
				if (!jumping)
					rigidbody.velocity = Vector3.MoveTowards(rigidbody.velocity, v, friction* Time.deltaTime);
					
				//else
				//	rigidbody.AddForce(v * Time.deltaTime, ForceMode.Impulse);
				
				if (!jumping) 
					rigidbody.rotation *= anim.deltaRotation; // I should probably make sure it points upwards on the ground normal here
				
				
				
			}
			
		}
		
		void CheckGroundStatus()
		{
#if UNITY_EDITOR
			// helper to visualise the ground check ray in the scene view
			Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
			// 0.1f is a small offset to start the ray from inside the character
			// it is also good to note that the transform position in the sample assets is at the base of the character
			RaycastHit hitInfo;
			if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance, activeLayers))
			{
				touchingGround = true;
				m_GroundNormal = hitInfo.normal;
				lastValidPos = transform.position;
				
				
				

				
				
				RaycastHit bHit;
				
				Physics.Raycast(transform.position + (Vector3.up * 0.1f) + (transform.forward * 0.1f), Vector3.down, out bHit, m_GroundCheckDistance, activeLayers);
				
				Vector3 aft = Vector3.ProjectOnPlane(transform.forward, hitInfo.normal) ;
				Vector3 fore = Vector3.ProjectOnPlane(transform.forward, bHit.normal) ;
     
				
				transform.rotation = Quaternion.LookRotation(Vector3.Lerp(aft,fore,.5f));
			}
			else
			{
				touchingGround = false;
				m_GroundNormal = Vector3.up;
			}
		}
		
		protected override void OnUpdate(){
			base.OnUpdate();
			
			if (!jumping && ZoneGlobal.inZone) 
				followTarget = false;
			
			
            if (followTarget && target != null)
            {
				
				Vector3 position = rigidbody.position;
				Vector3 direction = target.position - position;
				
                Move(direction);
            }
            else
            {
                Move(Vector3.zero);
				
            }
			
			if (transform.position.y < -10f)
				OnFall();
			
			
		}
		
		
		protected void TickJumpTimer(){
			
			if (tjumpStart < Time.time & touchingGround) 	
			{
			
				jumping = false;
				doubleJumped = false;
				
			}
			
		}
		protected void LateJump(){
			if (bjumping && touchingGround)
			{
				if (Time.time < tjumpThreshold)
					Jump();
			
				bjumping = false;
				
			}
		}
		protected void LateDive(){
			
			if (bdiving)
			{
				bdiving = false;
				Dive();
				
			}
		}
		protected override void OnFixedUpdate(){
			base.OnFixedUpdate();
			CheckGroundStatus();
		}
		protected override void OnLateUpdate(){
			base.OnLateUpdate();
			TickJumpTimer();
			
			
			LateJump();
			LateDive();
		}
		
		protected void Climb(){
			if (climbCooldown > Time.time)  return;
			RaycastHit hitInfo;
			if (Physics.Raycast(bunnyFront.position, Vector3.down, out hitInfo, bunnyFront.localPosition.y - .02f, activeLayers))
			{
				Vector3 p = transform.position;
				p.y = hitInfo.point.y;
				p += transform.forward * .02f;
				rigidbody.AddForce( (p - transform.position).normalized, ForceMode.VelocityChange);
				climbCooldown = Time.time + .1f;
			}
		}
	}
	
}
