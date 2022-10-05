using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;


namespace Dungeon
{
	public class Teleporter : UpdateBehaviour {

		// Use this for initialization
		
		public float delay = 2f;
		public Transform exit; //assign exit
		
		public List<Transform> teleportedThings = new List<Transform>(); // assign all teleported things here
		
		public List<NavMeshAgent> teleportedAgents = new List<NavMeshAgent>(); // assign all teleported things here
		
		
		// Update is called once per frame
		protected override void OnEnable () {
			base.OnEnable();
			
			StartCoroutine("WaitAMoment");
		}
		
		protected IEnumerator WaitAMoment()
		{
			yield return new WaitForSeconds(delay);
			
			Teleport();
		}
		
		protected void Teleport() {
			
			
			foreach (NavMeshAgent n in teleportedAgents)
			{
				n.Warp(exit.position);
			}
			foreach (Transform t in teleportedThings)
			{
				t.position = exit.position;
			}
		}
		
	}
}