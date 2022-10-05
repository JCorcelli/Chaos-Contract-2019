using UnityEngine;
using System.Collections;

namespace NPCSystem
{
	public class JerriBedProcedure : RoutineNode
	{
		
		
		public Animator ianimate;
		protected AICharacterControl imove;
		protected ThirdPersonCharacter ichar;
		protected Rigidbody irigid;
		
		public string currentMessage = "";
		public int _substate = 0;
		public int substate{ protected set{_substate = value;} get{return _substate;}  }
		public Transform bed_target	;
		public Transform bed_contact;
		public Transform bed_face	;
		public Transform bed_goal	;
		new protected Transform transform;
		
		
		// 0 not in bed
		public int BED   = 1 ;
		public int EDGE  = 2 ;
		
		
		// used for lerping the transform, and sentinel bool
		protected bool _running = false;
		protected bool setPosition = false;
		protected bool setRotation = false;
		protected float timer = 0f;
		protected float delay = -1f;
		protected Quaternion startRotation;
		protected Vector3 startPosition;
		protected Quaternion endRotation;
		protected Vector3 endPosition;
		
		
		void Awake() {
			if (ianimate ==null) ianimate = GetComponentInParent<Animator>();
			if (ianimate == null) Debug.Log(name + " no animator found in parents",gameObject);
			
			
			GameObject go = ianimate.gameObject;
			ichar = go.GetComponent<ThirdPersonCharacter>();
			
			transform = go.GetComponent<Transform>();
			imove = go.GetComponent<AICharacterControl>();
			
			irigid = go.GetComponent<Rigidbody>();
		}
		
		
		protected override void OnUpdate()
		{
			if (substate == 0 ) 
			{
				state = EXITSTATE;
				if (nextState > 0)
					substate = 1;
			}
				
			else if (substate == 1)
			{
				
				// set navigation agent position next to bed
				imove.target.transform.position = bed_target.position;
				if (Vector3.Distance(bed_target.position, imove.transform.position) <= 0.7f)
					substate = 2;
				
			}
			
			else if (substate == 2)
			{
					
				// apply transform motion
				TurnTowardsBed();
				timeEstimate = delay;
				_running = true;
				substate = 4;
			}
			else if (substate == 4)
			{
				if (_running) return;
				// the animator state is set, I wait for it to finish getting in bed. I'm busy.
				
				substate = 5;
				
				NavOff(); // HACK, might result in lack of accuracy
				ClimbOnBed();
			}
			else if (substate == 5)
			{
				// continuation of previous
				if (_running) return;
				// the animator state is set, I wait for it to finish getting in bed. I'm busy.
				
				substate = 6;
				
				LieDownOnBed();
			}
			else if (substate == 6)
			{
				if (_running) return;
				state = BED; // I'm in bed
				
				if (nextState != BED) // I can't go backwards. only forwards
					substate = 7;
				
			}
			else if (substate == 7)
			{
				if (_running) return;
				GoToEdge();
				substate = 8;
			}
			else if (substate == 8)
			{
				if (_running) return;
				if (nextState == 3 || nextState == EXITSTATE )
					substate = 9;
				else if (nextState == BED)
				{
					substate = 5; // I can reuse lie down on bed
					
				}
				state = EDGE;
				
			}
			else if (substate == 9)
			{
				if (_running) return;
			
				GetUp();
				substate = 10;
			}
			
			else if (substate == 10)
			{
				if (_running) return;
				
				GetUpTransition();
				substate = 11;
					
			}
			else if (substate == 11)
			{
				if (_running) return;// swinging leg
				substate = 12;
			}
			else
			{
				
				busy = true;
				if (_running) return;
				
				substate = 0;
				state = EXITSTATE;
				nextState = 0;
				NavOn();
			}
				
				
		}
		
		
		protected override void OnLateUpdate() {
			
			if (timer < delay)
			{
				timer += Time.deltaTime;
				
				
				
				if (timer >= delay)
				{
					if (setPosition)
						transform.position = Vector3.Lerp(startPosition, endPosition, timer / delay);
				
					if (setRotation)
						transform.rotation = Quaternion.Slerp(startRotation, endRotation, timer / delay);
				
					_running = false;
					timer = 0f;
					delay = -1f;
				}
				
				else
				{
					if (setPosition)
						transform.position = Vector3.Lerp(startPosition, endPosition, timer / delay);
				
					if (setRotation)
						transform.rotation = Quaternion.Slerp(startRotation, endRotation, timer / delay);
				}
			}
			
			
		}

		protected void NavOn()
		{
			imove.agent.enabled =true;
			imove.enabled = true;
			irigid.useGravity = true;
			transform.position = imove.agent.nextPosition;
			busy = false;
		}
		protected void NavOff()
		{
			imove.agent.enabled =false;
			imove.enabled = false;
			ichar.Move(Vector3.zero);
			irigid.useGravity = false;
			
			irigid.velocity = Vector3.zero;
			busy = true;
		}
		
		
		protected void TurnTowardsBed()
		{
			imove.target.position = bed_face.position;
			
			// calculate
			Vector3 lookPos = bed_face.position - bed_contact.position;
			lookPos.y = 0;
			Quaternion rotation = Quaternion.LookRotation(lookPos);
			Quaternion saveRot = transform.rotation;
			
			Vector3 savePos = transform.position;
			Vector3 newPos = bed_target.position + Vector3.up * imove.agent.baseOffset;

			// store
			startRotation = saveRot;
			startPosition = savePos;
			
			endRotation = rotation;
			endPosition = newPos;
			
			setPosition = true;
			setRotation = true;
			delay = 1.2f;
			
			
			
	
		}
		
		
		protected void ClimbOnBed()
		{
			_running = true;
			ianimate.SetInteger("State", 1);
		
			endPosition = bed_contact.position + Vector3.up * imove.agent.baseOffset;
			
			startPosition = transform.position;
			ianimate.CrossFadeInFixedTime("GettingInBed", 1.2f, 0, 0f);
			delay = 1.2f; 
			
			setPosition = true;
			setRotation = false;
		}
			
		protected void LieDownOnBed()
		{
			_running = true;
			delay = .9f; 
			startPosition = transform.position;
			endPosition = bed_goal.position;
			
			ianimate.CrossFadeInFixedTime("LyingDown_Turn", .9f, 0, 0f);
			setPosition = true;
			setRotation = false;
			
		}

		protected void GetUp(){
			// state
			// crossfade
			// timer
			
			// toggle off set
			_running = true;
			ianimate.SetInteger("State", 0);
			setPosition = false;
			setRotation = false;
			delay = 0.9f; // it's the .3 transition time, and .6 exit time
			
		}
		protected void GetUpTransition()
		{
			
			
			_running = true;
			startPosition = transform.position ;
			endPosition = bed_target.position + Vector3.up * imove.agent.baseOffset;
			setPosition = true;
			setRotation = false;
			
			ianimate.CrossFadeInFixedTime("Grounded", 1.4f, 0, 0f);
			
			delay = 1.4f;
			
			
		}
		
		protected void GoToEdge(){
			// substate
			// position
			// timer
			
			_running = true;
			
			
			ianimate.CrossFadeInFixedTime("LyingDown_ArmOver_R", 1.1f, 0, 0f);
			
			delay = 1.1f;
			
			startPosition = transform.position;
			endPosition = bed_contact.position;
			
			
		}
	}
}
