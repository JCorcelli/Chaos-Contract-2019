using UnityEngine;
using UnityEngine.AI;
using System.Collections;



namespace Dungeon
{
	public class FastMovementTriggerOneStop : UpdateBehaviour {

		// Use this for initialization
		
		public string targetName = "PresenceIndicator";
		public float delay = 2f;
		
		public Transform exit; //assign exit
		
		
		protected int count = 0; 
		
		public float teleportTime = 1f;
		public float elapsedTime = 0f;
		
		public bool teleporting = false;
		
		protected Vector3 startPos = new Vector3();
		protected Vector3 endPos = new Vector3();
		
		
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
		protected void TeleportStart() {
			
			
			startPos = DungeonVars.keyObjectTransform.position;
					
			/*
			foreach (NavMeshAgent n in DungeonVars.teleportedAgents)
			{
				n.updatePosition = false;
			}
			*/
		}
		
		protected void Teleport() {
			
			elapsedTime += Time.deltaTime;
			
			if (elapsedTime >= teleportTime)
			{
				elapsedTime = 0f;
				FinishTeleport();
				teleporting = false;
				return;
			}
			
			endPos = exit.position;
			float frac = elapsedTime / teleportTime;
			foreach (Transform t in DungeonVars.teleportedThings)
			{
				t.position = Vector3.Lerp(startPos, endPos, frac);
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