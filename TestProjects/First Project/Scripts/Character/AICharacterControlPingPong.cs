using System;
using UnityEngine;
using UnityEngine.AI;

namespace TestProject
{
    [RequireComponent(typeof (NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControlPingPong : MonoBehaviour
    {
        public NavMeshAgent agent { get; private set; } // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform target; // target to aim for
        public Transform target_a; // target to start at
        public Transform target_b; // target to end at
		
		//private Vector3 oldVec = Vector3.zero;
		//private bool moved;

        // Use this for initialization
        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<NavMeshAgent>();
			
            character = GetComponent<ThirdPersonCharacter>();

	        agent.updateRotation = false;
	        agent.updatePosition = true;
			
			// oldVec = target.position;
        }


        // Update is called once per frame
        private void Update()
        {
            if (target != null)
            {
				// test if the vector has changed
				/*
				float difference = (oldVec - target.position).magnitude;
				moved =  (difference <= 0.001f);
				if (moved) oldVec = target.position;
				*/
				
				//float difference = (transform.position - target.position).magnitude;
				
				
				if (agent.enabled)
				{
					agent.SetDestination(target.position);
				}
				if (agent.pathStatus != NavMeshPathStatus.PathComplete) 	
				{
					return; // I should probably replace it with a path that still functions
				}

				
				
                // use the values to move the character
                character.Move(agent.desiredVelocity, false, false);
            }
            else
            {
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
