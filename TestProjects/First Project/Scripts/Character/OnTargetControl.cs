using UnityEngine;
using UnityEngine.AI;
using System.Collections;

namespace TestProject 
{
    //[RequireComponent(typeof (ThirdPersonBunny))]
	[RequireComponent(typeof (CharacterMove))]
    [RequireComponent(typeof (NavMeshAgent))]
	
	public class OnTargetControl : MonoBehaviour {
		private CharacterMove character;
		public Transform target;
		private NavMeshAgent agent;
		// Use this for initialization
		void Start () {
			character = GetComponentInChildren<CharacterMove>();
			agent = GetComponentInChildren<NavMeshAgent>();
		}
		
		// Update is called once per frame
		void Update () {
		
            if (target != null)
			{
				if (agent.enabled)
					agent.SetDestination(target.position);
				character.Move(agent.desiredVelocity, false, false);
				
			}
            else
            {
                //agent.ResetPath();
                // We still need to call the character's move function, but we send zeroed input as the move param.
                character.Move(Vector3.zero, false, false);
            }
		}
		
        public void SetTarget(Transform target)
        {
            this.target = target;
        }
	}
}