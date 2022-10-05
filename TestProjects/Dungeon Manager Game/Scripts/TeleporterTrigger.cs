using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;


namespace Dungeon
{
	public class TeleporterTrigger : UpdateBehaviour {

		// Use this for initialization
		
		public string targetName = "PlayerCollider";
		public float delay = 2f;
		
		public Transform exit; //assign exit
		
		public int count = 0; 
		
		protected IEnumerator WaitAMoment()
		{
			yield return new WaitForSeconds(delay);
			
			Teleport();
		}
		
		protected void Teleport() {
			
			
			foreach (NavMeshAgent n in DungeonVars.teleportedAgents)
			{
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