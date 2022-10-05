using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using NPCSystem;
using Utility;
using NPC.BTree.Jerri;

namespace Anim.Jerri
{
	public class JACxPlayInput : JACxPlay {

		protected float height = 1f;
		protected float lookY = 1f;
		protected float turn = 0f;
		protected float lookX = 1f;
		
		new protected Transform transform;
		public AICharacterControl tpc;
		protected Rigidbody irigid;
		protected CapsuleCollider col; //better crawling y=.14 col.direction = z-axis
		
		public NavMeshAgent agent;
		protected JerriBeStateHUB hub ;
		
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
		protected Vector3 centerCrawl = new Vector3(0, 0.14f, 0);
		protected Vector3 standCrawl = new Vector3(0, 0.51f, 0);
		
		public Utility.GoClosestLate target;
		
		protected void Awake() {
			hub = GetComponentInParent<JerriBeStateHUB>();
			agent = GetComponent<NavMeshAgent>();
			transform = GetComponent<Transform>();
			tpc = GetComponent<AICharacterControl>();
			irigid = GetComponent<Rigidbody>();
			col = GetComponent<CapsuleCollider>(); 
		}
		
		protected NavMeshHit navHit;
		
		protected void GetOnBed()
		{
			delay = .4f; 
			_running = true;
			
			Vector3 heading = target.target.position - transform.position;
			float fidelity = Vector3.Dot(heading.normalized, transform.right);
			
			if (fidelity > .25f)
				ih.CrossFadeInFixedTime("Bed_StrideLean_Right", .4f, 0, 0f);
			else if (fidelity < -0.25f)
				ih.CrossFadeInFixedTime("Bed_StrideLean_Left", .4f, 0, 0f);
			else
				ih.CrossFadeInFixedTime("Bed_Lean_Front", .4f, 0, 0f);
			startPosition = transform.position;
			
			
			//Vector3 newPos = transform.position+ transform.up * .5f;
			
			tpc.enabled = false;
			
			
			endPosition = target.GetClosestPointOnLineSegment(transform.position); // technically i don't use endpos
			
			NavMesh.SamplePosition(endPosition, out navHit, 10f, (int)agent.areaMask);
			endPosition =  	navHit.position;
						
			
						
						
						
			//col.isTrigger = true;
			irigid.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			
			setPosition = true;
			setRotation = false;
			
		}
		
		protected override void OnLateUpdate() {
			if (target == null) return;
			if (timer < delay)
			{
				timer += Time.deltaTime;
				
				
				
				if (timer >= delay)
				{
					if (setPosition)
					{
						transform.position = Vector3.Lerp(startPosition, endPosition, timer / delay);
					}
				
					if (setRotation)
						transform.rotation = Quaternion.Slerp(startRotation, endRotation, timer / delay);
				
					_running = false;
					
					//col.isTrigger = false;
					irigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			
					timer = 0f;
					delay = -1f;
					agent.Warp(endPosition);
					tpc.enabled = true;
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
		
		
		protected ActiveStatesEnum bed = ActiveStatesEnum.WantBed;
		protected ActiveStatesEnum laptop = ActiveStatesEnum.WantLaptop;
		protected ActiveStatesEnum sleep = ActiveStatesEnum.WantSleep;
		protected ActiveStatesEnum door = ActiveStatesEnum.WantDoor;
		protected ActiveStatesEnum bunny = ActiveStatesEnum.WantBunny;
		
		
		protected void ToggleState (ActiveStatesEnum state) {
			if (hub.Has( state ))
				hub.Remove(state);
			else
				hub.Add(state);
			
			
		}
		
		protected override void OnUpdate(){
			
			if (Input.GetKeyUp("0")){
				ih.SetSubstate(0);
			}
			// getting in bed
			else if (Input.GetKeyUp("1")){
				ToggleState( bed );
			}
			else if (Input.GetKeyUp("2")){
				ToggleState( sleep);
			}
			else if (Input.GetKeyUp("3")){
				ToggleState( laptop);
			}
			else if (Input.GetKeyUp("4")){
				ToggleState( door);
			}
			else if (Input.GetKeyUp("5")){
				ToggleState( bunny);
			
			}
		}
	}
}