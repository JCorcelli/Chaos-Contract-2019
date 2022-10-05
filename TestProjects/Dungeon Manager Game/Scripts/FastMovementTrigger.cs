using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;


namespace Dungeon
{
	public class FastMovementTrigger : UpdateBehaviour {

		// Use this for initialization
		
		public string targetName = "PresenceIndicator";
		public float delay = 2f;
		
		public Transform exit; //assign exit
		
		
		public List<Transform> waypoints = new List<Transform>();
		public List<float> timings = new List<float>();
		
		
		protected int count = 0; 
		public int waypointCounter = 0;
		
		public float elapsedTime = 0f;
		protected float removedTime = 0f;
		
		public bool teleporting = false;
		
		protected Vector3 startPos = new Vector3();
		protected Vector3 endPos = new Vector3();
		protected float teleportTime = 1f;
		
		
		
		protected void Start() {
			
			if (waypoints.Count < 1) Debug.LogError("need waypoints", gameObject);
			
			if (waypoints.Count != timings.Count) Debug.LogError("waypoints.Count != timings.Count", gameObject);
			
		}
		protected IEnumerator WaitAMoment()
		{
			yield return new WaitForSeconds(delay);
			
			TeleportStart();
			teleporting = true;
			
		}
		protected IEnumerator Finish()
		{
			yield return new WaitForSeconds(teleportTime);
			
			FinishTeleport();
		}
		
		
		protected override void OnUpdate() {
			
			if (!teleporting) return;
			
			Teleport();
		}
		
		protected void ResetTeleport() {
			endPos = waypoints[0].position;
			
			elapsedTime = 0f;
			
			waypointCounter = 0;
			
			teleportTime = timings[0];
			removedTime = 0;
			
			
		}
		protected void TeleportStart() {
			ResetTeleport();
			// technically a reset
			startPos = DungeonVars.keyObjectTransform.position;
			endPos = waypoints[0].position;
			teleportTime = timings[0];
			
					
		}
		
		protected void Teleport() {
			
			elapsedTime += Time.deltaTime;
			
			
				
			
			float frac = (elapsedTime - removedTime) / (teleportTime - removedTime);
			
			DungeonVars.keyObjectTransform.position = Vector3.Lerp(startPos, endPos, frac);
			
			
			if (elapsedTime >= timings[waypointCounter])
			{
				startPos = waypoints[waypointCounter].position;
				removedTime = timings[waypointCounter];
				
				waypointCounter ++;
				
				if (waypointCounter >= timings.Count)
				{
					teleporting = false;
					ResetTeleport();
					FinishTeleport();
					return;
				}
				
				
				endPos = waypoints[waypointCounter].position;
				teleportTime = timings[waypointCounter];
			}
			
		}
		protected void FinishTeleport() {
			
			
			foreach (NavMeshAgent n in DungeonVars.teleportedAgents)
			{
				//n.updatePosition = true;
				n.Warp(exit.position);
			}
			foreach (Transform t in DungeonVars.teleportedThings)
			{
				t.position = exit.position;
			}
		}
		
		void OnTriggerEnter( Collider col ) {
			if (targetName == col.name) {
				count ++;
				
				StartCoroutine("WaitAMoment");
				
			}
		}
		void OnTriggerExit( Collider col ) {
			if (targetName == col.name) {
				
				count --;
				if (count <= 0)
				{
					count = 0;
					StopCoroutine("WaitAMoment");
				}
				
			}
			
		}
		
	}
}