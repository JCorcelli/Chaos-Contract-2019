using System;
using UnityEngine;
using UnityEngine.AI;

namespace NPCSystem 
{
    [RequireComponent(typeof (NavMeshAgent))]
    [RequireComponent(typeof (IMovable))]
    public class AICharacterControl : UpdateBehaviour
    {
        public NavMeshAgent agent { get; protected set; } // the navmesh agent required for the path finding
        public IMovable character { get; protected set; } // the character we are controlling
        public Transform target; // target to aim for
		

		public string _targetName;
		public string _targetTag;

		new protected Rigidbody rigidbody;
		
		protected void Start () {
			
			if (_targetName != "" )
				target = gameObject.FindNameXTag(_targetName, _targetTag).transform;
			
			rigidbody = GetComponent<Rigidbody>();
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<NavMeshAgent>();
			
            character = GetComponent<IMovable>();

	        agent.updateRotation = false;
	        agent.updatePosition = false;
        }


        // Update is called once per frame
        protected override void OnUpdate()
        {
            if (target != null)
            {
				if (agent.enabled)
				{
					agent.nextPosition = rigidbody.position;
					agent.SetDestination(target.position);
					
					transform.position = new Vector3(agent.nextPosition.x, rigidbody.position.y, agent.nextPosition.z);
				}
				if (!agent.hasPath) 	
				{
					return;
				}

				
				
                // use the values to move the character
                character.Move(agent.desiredVelocity * Vector3.Distance(transform.position, target.position));
            }
            else
            {
                // We still need to call the character's move function, but we send zeroed input as the move param.
                character.Move(Vector3.zero);
				if (agent.enabled)
					agent.ResetPath();
            }

        }


        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}
