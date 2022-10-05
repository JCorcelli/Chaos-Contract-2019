using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using NPCSystem;
using Utility;

namespace Anim.Jerri
{
	public class JACxEdgeOnBed : JACxPlay {

		protected float height = 1f;
		protected float lookY = 1f;
		protected float turn = 0f;
		protected float lookX = 1f;
		
		new protected Transform transform;
		protected AICharacterControl tpc;
		protected Rigidbody irigid;
		protected CapsuleCollider col; //better crawling y=.14 col.direction = z-axis
		
		protected NavMeshAgent agent;
		
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
		
		protected bool hasInit = false;
		protected void Init() {
			if (hasInit) return;
			hasInit = true;
			agent = ih.agent;
			transform = ih.transform;
			tpc = ih.tpc;
			irigid = ih.irigid;
			col = ih.col;
		}
		
		protected NavMeshHit navHit;
		
		protected void ReachOnBed()
		{
			
			delay = .5f;
			_running = true;
			
			
			
			startPosition = transform.position;
			
			
			//Vector3 newPos = transform.position+ transform.up * .5f;
			
			tpc.enabled = false;
			
			
			endPosition = target.GetClosestPointOnLineSegment(transform.position); // technically i don't use endpos
			
			
			
			NavMesh.SamplePosition(endPosition, out navHit, 10f, (int)agent.areaMask);
			endPosition =  	navHit.position;
						
			
			Vector3 newHeading = target.endTrans.position - target.startTrans.position;
			
			startRotation = transform.rotation;
			endRotation = Quaternion.LookRotation(newHeading.normalized);
						
						
			
			setPosition = true;
			setRotation = true;
			
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
		protected override void OnEnable() {
			
			base.OnEnable();
			Init();
			// climb on bed
			ReachOnBed();
				
		}
		protected void Cleanup(){
	
			timer = 0f;
			delay = -1f;
			tpc.enabled = true;
			
		}
		protected override void OnDisable() {
			
			base.OnDisable();
			Cleanup();
				
		}
		
		
		
	}
}